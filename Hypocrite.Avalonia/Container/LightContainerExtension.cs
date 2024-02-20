using Hypocrite.Core.Container;
using Hypocrite.Core.Container.Interfaces;
using Prism.Ioc;
using Prism.Ioc.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hypocrite.Container
{
    public class LightContainerExtension : IContainerExtension<ILightContainer>, IContainerInfo
    {
        private LightScopedProvider _currentScope;

        /// <summary>
        /// The instance of the wrapped container
        /// </summary>
        public ILightContainer Instance { get; }

#if !ContainerExtensions
        /// <summary>
        /// Constructs a default <see cref="LightContainerExtension" />
        /// </summary>
        public LightContainerExtension()
            : this(new LightContainer())
        {
        }

        /// <summary>
        /// Constructs a <see cref="LightContainerExtension" /> with the specified <see cref="ILightContainer" />
        /// </summary>
        /// <param name="container"></param>
        public LightContainerExtension(ILightContainer container)
        {
            Instance = container;
            Instance.RegisterInstance(typeof(ILightContainer), Instance);
            Instance.RegisterInstance(this.GetType(), this);
            Instance.RegisterInstance(typeof(IContainerExtension), this);
            Instance.RegisterInstance(typeof(IContainerProvider), this);
            // ExceptionExtensions.RegisterFrameworkExceptionType(typeof(ResolutionFailedException));
        }
#endif

        public IScopedProvider CurrentScope => _currentScope;

        public virtual IScopedProvider CreateScope()
        {
            return CreateScopeInternal();
        }

        public void FinalizeExtension()
        {

        }

        public Type GetRegistrationType(string key)
        {
            return Instance.Registrations.Get(0, key)?.Value?.MappedToType;
        }

        public Type GetRegistrationType(Type serviceType)
        {
            var matchingRegistration = Instance.Registrations.Get(serviceType.GetHashCode(), string.Empty);
            return matchingRegistration?.Value?.MappedToType;
        }

        public bool IsRegistered(Type type)
        {
            return Instance.IsRegistered(type);
        }

        public bool IsRegistered(Type type, string name)
        {
            return Instance.IsRegistered(type, name);
        }

        public IContainerRegistry Register(Type from, Type to)
        {
            Instance.RegisterType(from, to);
            return this;
        }

        public IContainerRegistry Register(Type from, Type to, string name)
        {
            Instance.RegisterType(from, to, name);
            return this;
        }

        public IContainerRegistry Register(Type type, Func<object> factoryMethod)
        {
            Instance.RegisterFactory(type, (c, t) => { return factoryMethod?.Invoke(); });
            return this;
        }

        public IContainerRegistry Register(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            Instance.RegisterFactory(type, (c, t) => { return factoryMethod?.Invoke(this); });
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance)
        {
            Instance.RegisterInstance(type, instance);
            return this;
        }

        public IContainerRegistry RegisterInstance(Type type, object instance, string name)
        {
            Instance.RegisterInstance(type, instance, name);
            return this;
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterMany(Type type, params Type[] serviceTypes)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterManySingleton(Type type, params Type[] serviceTypes)
        {
            throw new NotImplementedException();
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterScoped(Type from, Type to)
        {
            return Register(from, to);
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterScoped(Type type, Func<object> factoryMethod)
        {
            return Register(type, factoryMethod);
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterScoped(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            return Register(type, factoryMethod);
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to)
        {
            Instance.RegisterType(from, to, isSingleton: true);
            return this;
        }

        public IContainerRegistry RegisterSingleton(Type from, Type to, string name)
        {
            Instance.RegisterType(from, to, name, isSingleton: true);
            return this;
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterSingleton(Type type, Func<object> factoryMethod)
        {
            return Register(type, factoryMethod);
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public IContainerRegistry RegisterSingleton(Type type, Func<IContainerProvider, object> factoryMethod)
        {
            return Register(type, factoryMethod);
        }

        public object Resolve(Type type)
        {
            return Instance.Resolve(type);
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
        {
            return Instance.Resolve(type);
        }

        public object Resolve(Type type, string name)
        {
            return Instance.Resolve(type, name, true);
        }

        [Obsolete("Abdrakov.Container does not provide difficult registrations")]
        public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
        {
            return Instance.Resolve(type, name, true);
        }

        /// <summary>
        /// Creates a new Scope and provides the updated ServiceProvider
        /// </summary>
        /// <returns>A child <see cref="IUnityContainer" />.</returns>
        /// <remarks>
        /// This should be called by custom implementations that Implement IServiceScopeFactory
        /// </remarks>
        protected IScopedProvider CreateScopeInternal()
        {
            var child = Instance;
            _currentScope = new LightScopedProvider(child);
            return _currentScope;
        }

        private class LightScopedProvider : IScopedProvider
        {
            public LightScopedProvider(ILightContainer container)
            {
                Container = container;
            }

            public ILightContainer Container { get; private set; }
            public bool IsAttached { get; set; }
            public IScopedProvider CurrentScope => this;

            public IScopedProvider CreateScope() => this;

            public void Dispose()
            {
                Container.Dispose();
                Container = null;
            }

            public object Resolve(Type type) =>
                Resolve(type, Array.Empty<(Type, object)>());

            public object Resolve(Type type, string name) =>
                Resolve(type, name, Array.Empty<(Type, object)>());

            public object Resolve(Type type, params (Type Type, object Instance)[] parameters)
            {
                try
                {
                    // var overrides = parameters.Select(p => new DependencyOverride(p.Type, p.Instance)).ToArray();
                    return Container.Resolve(type);
                }
                catch (Exception ex)
                {
                    throw new ContainerResolutionException(type, ex);
                }
            }

            public object Resolve(Type type, string name, params (Type Type, object Instance)[] parameters)
            {
                try
                {
                    // Unity will simply return a new object() for unregistered Views
                    if (!Container.IsRegistered(type, name))
                        throw new KeyNotFoundException($"No registered type {type.Name} with the key {name}.");

                    // var overrides = parameters.Select(p => new DependencyOverride(p.Type, p.Instance)).ToArray();
                    return Container.Resolve(type, name, true);
                }
                catch (Exception ex)
                {
                    throw new ContainerResolutionException(type, name, ex);
                }
            }
        }
    }
}
