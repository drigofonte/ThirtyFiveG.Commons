using System.Linq.Expressions;
using System.Reflection;

namespace ThirtyFiveG.Commons.Expressions
{
    public class Literalizer : ExpressionVisitor
    {
        #region Overrides
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression != null && node.Expression.NodeType == ExpressionType.Constant)
            {
                object target = ((ConstantExpression)node.Expression).Value, value;
                PropertyInfo property = node.Member as PropertyInfo;
                FieldInfo field = node.Member as FieldInfo;
                if (property != null)
                {
                    value = property.GetValue(target, null);
                }
                else if (field != null)
                {
                    value = field.GetValue(target);
                }
                else
                {
                    value = target = null;
                }

                if (target != null) return Expression.Constant(value, node.Type);
            }
            
            return base.VisitMember(node);
        }
        #endregion
    }
}
