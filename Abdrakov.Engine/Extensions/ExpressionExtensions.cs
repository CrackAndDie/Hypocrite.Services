using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Abdrakov.Engine.Extensions
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Gets the MemberInfo where a Expression is pointing towards.
        /// Can handle MemberAccess and Index types and will handle
        /// going through the Conversion Expressions.
        /// </summary>
        /// <param name="expression">The expression.</param>
        /// <returns>The member info from the expression.</returns>
        public static MemberInfo GetMemberInfo(this Expression expression)
        {
            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            MemberInfo info;
            switch (expression.NodeType)
            {
                case ExpressionType.Index when expression is IndexExpression indexExpression:
                    info = indexExpression.Indexer;
                    break;
                case ExpressionType.MemberAccess when expression is MemberExpression memberExpression:
                    info = memberExpression.Member;
                    break;
                case ExpressionType.ConvertChecked:
                case ExpressionType.Convert:
                    if (expression is UnaryExpression unaryExpression)
                        return GetMemberInfo(unaryExpression.Operand);
                    info = null;
                    break;
                default:
                    throw new NotSupportedException($"Unsupported expression type: '{expression.NodeType}'");
            }

            return info;
        }
    }
}
