using Aq.ExpressionJsonSerializer;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using ThirtyFiveG.Commons.Expressions;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class ExpressionExtensions
    {
        #region Private variables
        private static JsonSerializerSettings _settings;
        #endregion

        #region Public methods
        public static JsonSerializerSettings GetSettings(Assembly[] assemblies)
        {
            if (_settings == null)
            {
                _settings = new JsonSerializerSettings();
                _settings.Converters.Add(new ExpressionJsonConverter(Assembly.GetCallingAssembly(), assemblies));
            }
            return _settings;
        }

        public static string AsString(this Expression e, bool literalize = false)
        {
            return AsString(e, new Assembly[] { }, literalize);
        }

        public static string AsString(this Expression e, Assembly[] assemblies, bool literalize = false)
        {
            Expression expression = e;
            if (literalize)
                expression = new Literalizer().Visit(expression);
            return JsonConvert.SerializeObject(expression, GetSettings(assemblies));
        }

        public static Expression CallAny(this Expression collection, Expression predicateExpression)
        {
            Type collectionType = collection.Type.AsIEnumerable(true);
            Type elemType = collectionType.GetGenericArguments()[0];
            Type predType = typeof(Func<,>).MakeGenericType(elemType, typeof(bool));

            return CallCollectionMethod(collection, predicateExpression, "Any", predType, new[] { elemType });
        }

        public static Expression CallSelect(this Expression collection, Expression predicateExpression, Type resultingType)
        {
            Type collectionType = collection.Type.AsIEnumerable(true);
            Type elemType = collectionType.GetGenericArguments()[0];
            Type predType = typeof(Func<,>).MakeGenericType(elemType, resultingType);

            return CallCollectionMethod(collection, predicateExpression, "Select", predType, new[] { elemType, resultingType });
        }

        public static Expression<TDelegate> Not<TDelegate>(this Expression<TDelegate> expression)
        {
            return Expression.Lambda<TDelegate>(Expression.Not(expression.Body), expression.Parameters);
        }
        #endregion

        #region Private methods
        private static Expression CallCollectionMethod(this Expression collection, Expression predicateExpression, string methodName, Type predicateType, Type[] methodGenericTypes)
        {
            Type collectionType = collection.Type.AsIEnumerable(true);
            collection = Expression.Convert(collection, collectionType);

            MethodInfo method = typeof(Enumerable).GetGenericMethod(methodName, methodGenericTypes, new[] { collectionType, predicateType }, BindingFlags.Static);

            return Expression.Call(
                method,
                collection,
                predicateExpression);
        }
        #endregion
    }
}
