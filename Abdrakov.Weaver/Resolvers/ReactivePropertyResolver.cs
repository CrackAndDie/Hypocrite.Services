using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;
using Mono.Cecil.Rocks;
using Abdrakov.Weaver.Extensions;
using Mono.Cecil.Cil;
using Abdrakov.Weaver.Interfaces;

namespace Abdrakov.Weaver.Resolvers
{
    public class ReactivePropertyResolver : IResolver
    {
        public ModuleDefinition ModuleDefinition { get; set; }
        /// <summary>
        /// Executes this property weaver.
        /// </summary>
        /// <exception cref="Exception">
        /// reactiveObjectExtensions is null
        /// or
        /// raiseAndSetIfChangedMethod is null
        /// or
        /// reactiveAttribute is null
        /// or
        /// [Reactive] is decorating " + property.DeclaringType.FullName + "." + property.Name + ", but the property has no setter so there would be nothing to react to.  Consider removing the attribute.
        /// </exception>
        public void Execute()
        {
            if (ModuleDefinition is null)
            {
                // LogInfo?.Invoke("The module definition has not been defined.");
                return;
            }

            var engine = ModuleDefinition.AssemblyReferences.Where(x => x.Name == "Abdrakov.Engine").OrderByDescending(x => x.Version).FirstOrDefault();
            if (engine is null)
            {
                // LogInfo?.Invoke("Could not find assembly: ReactiveUI (" + string.Join(", ", ModuleDefinition.AssemblyReferences.Select(x => x.Name)) + ")");
                return;
            }

            // LogInfo?.Invoke($"{helpers.Name} {helpers.Version}");
            var bindableObject = new TypeReference("Abdrakov.Engine.MVVM", "BindableObject", ModuleDefinition, engine);
            var targetTypes = ModuleDefinition.GetAllTypes().Where(x => x.BaseType != null && bindableObject.IsAssignableFrom(x.BaseType)).ToArray();
            var raiseAndSetIfChangedMethod = ModuleDefinition.ImportReference(bindableObject.Resolve().Methods.Single(x => x.Name == "SetProperty")) ?? throw new Exception("raiseAndSetIfChangedMethod is null");
            var reactiveAttribute = ModuleDefinition.FindType("Abdrakov.Engine.MVVM.Attributes", "BindableAttribute", engine) ?? throw new Exception("reactiveAttribute is null");
            foreach (var targetType in targetTypes)
            {
                foreach (var property in targetType.Properties.Where(x => x.IsDefined(reactiveAttribute)).ToArray())
                {
                    if (property.SetMethod is null)
                    {
                        // LogError?.Invoke($"Property {property.DeclaringType.FullName}.{property.Name} has no setter, therefore it is not possible for the property to change, and thus should not be marked with [Reactive]");
                        continue;
                    }

                    // Declare a field to store the property value
                    var field = new FieldDefinition("$" + property.Name, Mono.Cecil.FieldAttributes.Private, property.PropertyType);
                    targetType.Fields.Add(field);

                    // Remove old field (the generated backing field for the auto property)
                    var oldField = (FieldReference)property.GetMethod.Body.Instructions.Single(x => x.Operand is FieldReference).Operand;
                    var oldFieldDefinition = oldField.Resolve();
                    targetType.Fields.Remove(oldFieldDefinition);

                    // See if there exists an initializer for the auto-property
                    var constructors = targetType.Methods.Where(x => x.IsConstructor);
                    foreach (var constructor in constructors)
                    {
                        var fieldAssignment = constructor.Body.Instructions.SingleOrDefault(x => Equals(x.Operand, oldFieldDefinition) || Equals(x.Operand, oldField));
                        if (fieldAssignment != null)
                        {
                            // Replace field assignment with a property set (the stack semantics are the same for both,
                            // so happily we don't have to manipulate the bytecode any further.)
                            var setterCall = constructor.Body.GetILProcessor().Create(property.SetMethod.IsVirtual ? OpCodes.Callvirt : OpCodes.Call, property.SetMethod);
                            constructor.Body.GetILProcessor().Replace(fieldAssignment, setterCall);
                        }
                    }

                    // Build out the getter which simply returns the value of the generated field
                    property.GetMethod.Body = new MethodBody(property.GetMethod);
                    property.GetMethod.Body.Emit(il =>
                    {
                        il.Emit(OpCodes.Ldarg_0);                                   // this
                        il.Emit(OpCodes.Ldfld, field.BindDefinition(targetType));   // pop -> this.$PropertyName
                        il.Emit(OpCodes.Ret);                                       // Return the field value that is lying on the stack
                    });

                    TypeReference genericTargetType = targetType;
                    if (targetType.HasGenericParameters)
                    {
                        var genericDeclaration = new GenericInstanceType(targetType);
                        foreach (var parameter in targetType.GenericParameters)
                        {
                            genericDeclaration.GenericArguments.Add(parameter);
                        }

                        genericTargetType = genericDeclaration;
                    }

                    var methodReference = raiseAndSetIfChangedMethod.MakeGenericMethod(genericTargetType, property.PropertyType);

                    // Build out the setter which fires the RaiseAndSetIfChanged method
                    if (property.SetMethod is null)
                    {
                        throw new Exception("[Reactive] is decorating " + property.DeclaringType.FullName + "." + property.Name + ", but the property has no setter so there would be nothing to react to.  Consider removing the attribute.");
                    }

                    property.SetMethod.Body = new MethodBody(property.SetMethod);
                    property.SetMethod.Body.Emit(il =>
                    {
                        il.Emit(OpCodes.Ldarg_0);                                   // this
                        il.Emit(OpCodes.Ldarg_0);                                   // this
                        il.Emit(OpCodes.Ldflda, field.BindDefinition(targetType));  // pop -> this.$PropertyName
                        il.Emit(OpCodes.Ldarg_1);                                   // value
                        il.Emit(OpCodes.Ldstr, property.Name);                      // "PropertyName"
                        il.Emit(OpCodes.Call, methodReference);                     // pop * 4 -> this.RaiseAndSetIfChanged(this.$PropertyName, value, "PropertyName")
                        il.Emit(OpCodes.Pop);                                       // We don't care about the result of RaiseAndSetIfChanged, so pop it off the stack (stack is now empty)
                        il.Emit(OpCodes.Ret);                                       // Return out of the function
                    });
                }
            }
        }
    }
}
