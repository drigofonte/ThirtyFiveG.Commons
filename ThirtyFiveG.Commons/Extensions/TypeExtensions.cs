using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using ThirtyFiveG.Commons.Collections;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class TypeExtensions
    {
        #region Private static variables
        private static readonly Regex CollectionPathAttribute = new Regex(@"(\[(.*?)\])");
        private static readonly JsonSerializerSettings DeserializerSettings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore, PreserveReferencesHandling = PreserveReferencesHandling.All, TypeNameHandling = TypeNameHandling.Auto };
        private static readonly IDictionary<string, PropertyInfo[]> _baseProperties = new Dictionary<string, PropertyInfo[]>();
        private static readonly IDictionary<string, PropertyInfo[]> _virtualProperties = new Dictionary<string, PropertyInfo[]>();
        private static readonly string _keyPattern = "{{\"type\":{0}, \"ignoreBaseMethods\":{1}, \"applyJsonIgnore\": {2}}}";
        #endregion

        #region Public static methods
        public static MethodInfo GetGenericMethod(this Type type, string name, Type[] typeArgs, Type[] argTypes, BindingFlags flags)
        {
            int typeArity = typeArgs.Length;
            var methods = type.GetMethods()
                .Where(m => m.Name == name)
                .Where(m => m.GetGenericArguments().Length == typeArity);

            methods = methods.Select(m => m.MakeGenericMethod(typeArgs));

            MethodInfo method = null;
            foreach (var m in methods)
            {
                ParameterInfo[] parameters = m.GetParameters();
                Type[] parameterTypes = parameters.Select(p => p.ParameterType).ToArray();

                if (parameterTypes.Length == argTypes.Length)
                {
                    bool matches = true;
                    for (int i = 0; i < parameterTypes.Length; i++)
                        matches &= parameterTypes[i].Equals(argTypes[i]);
                    if (matches)
                    {
                        method = m;
                        break;
                    }
                }
            }

            return method;
        }

        public static Type AsIEnumerable(this Type type, bool forceCast = false)
        {
            if (type.IsIEnumerable() && !forceCast)
                return type;
            IEnumerable<Type> t = type.GetInterfaces().Where(i => i.IsIEnumerable(true));

            return t.Single();
        }

        public static bool IsIEnumerable(this Type type, bool forceGeneric = false)
        {
            return (type.IsGenericType && type.GetInterfaces().Contains(typeof(IEnumerable)))
                || (!forceGeneric && type.Equals(typeof(IEnumerable)));
        }

        public static Expression<Func<T, bool>> PropertyEqualsLambda<T>(this Type type, string propertyName, object value)
        {
            return PropertyEqualsLambda(type, propertyName, value) as Expression<Func<T, bool>>;
        }

        public static LambdaExpression PropertyEqualsLambda(this Type type, string propertyName, object value)
        {
            PropertyInfo property = type.GetProperties().Single(p => p.Name.Equals(propertyName));
            ParameterExpression e = Expression.Parameter(type, "t");
            return GetWherePropertyEquals(e, property, value);
        }

        public static LambdaExpression PropertiesEqualLambda(this Type type, IDictionary<string, object> properties)
        {
            IDictionary<PropertyInfo, object> actualProperties = properties.ToDictionary(p => type.GetProperties().Single(pr => pr.Name.Equals(p.Key)), p => p.Value);
            ParameterExpression e = Expression.Parameter(type, "t");
            return GetWherePropertiesEqual(e, type, actualProperties);
        }

        public static T DeserializeObject<T>(string json)
        {
            return (T)DeserializeObject(typeof(T), json);
        }

        public static object DeserializeObject(this Type t, string json)
        {
            return JsonConvert.DeserializeObject(json, t, DeserializerSettings);
        }

        public static Type Eval(this Type type, string path)
        {
            Type t = type;
            path = CollectionPathAttribute.Replace(path, "");
            if (!path.EndsWith("."))
                path = path + ".";

            if (!path.Trim().Equals("."))
            {
                string propertyName = path.Substring(1, path.IndexOf('.', 1) - 1);
                PropertyInfo property = type.GetProperty(propertyName);
                if (property == null)
                    throw new ArgumentNullException("The property '" + propertyName + "' is not defined by type '" + type.FullName + "'");

                Type propertyType = property.GetGetMethod().ReturnType;
                if (propertyType.IsIEnumerable())
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                }
                t = propertyType.Eval(path.Substring(propertyName.Length + 1));
            }
            return t;
        }

        public static PropertyInfo[] GetBaseProperties(this Type type, bool ignoreBaseMethods = false, bool applyJsonIgnore = false)
        {
            string key = string.Format(_keyPattern, type.FullName, ignoreBaseMethods, applyJsonIgnore);
            PropertyInfo[] properties;
            if (!_baseProperties.TryGetValue(key, out properties))
            {
                properties = type.GetProperties().Where(p =>
                    (!p.GetGetMethod().IsVirtual || (p.GetGetMethod().IsFinal && p.GetGetMethod().IsVirtual))
                    && (!ignoreBaseMethods || (ignoreBaseMethods && p.GetGetMethod(false).GetBaseDefinition() == p.GetGetMethod(false)))
                    && (!applyJsonIgnore || (applyJsonIgnore && !Attribute.IsDefined(p, typeof(JsonIgnoreAttribute))))).ToArray();
                _baseProperties.Add(key, properties);
            }
            return properties;
        }

        public static PropertyInfo[] GetVirtualProperties(this Type type, bool ignoreBaseMethods = false, bool applyJsonIgnore = false)
        {
            string key = string.Format(_keyPattern, type.FullName, ignoreBaseMethods, applyJsonIgnore);
            PropertyInfo[] properties;
            if (!_virtualProperties.TryGetValue(key, out properties))
            {
                properties = type.GetProperties().Where(p =>
                    !p.GetGetMethod().IsFinal
                    && p.GetGetMethod().IsVirtual
                    && (!ignoreBaseMethods || (ignoreBaseMethods && p.GetGetMethod(false).GetBaseDefinition() == p.GetGetMethod(false)))
                    && (!applyJsonIgnore || (applyJsonIgnore && !Attribute.IsDefined(p, typeof(JsonIgnoreAttribute))))).ToArray();
                _virtualProperties.Add(key, properties);
            }
            return properties;
        }

        public static Type GetBaseType(this Type type)
        {
            if (type.BaseType != null && type.Namespace == "System.Data.Entity.DynamicProxies")
                return type.BaseType;
            return type;
        }

        public static bool IsINotifyCollectionChanged(this Type type)
        {
            return type.IsGenericType
                && type.GetInterfaces().Contains(typeof(INotifyCollectionChanged));
        }

        public static bool IsIObservableRangeCollection(this Type type)
        {
            return type.GetInterfaces().Contains(typeof(IObservableRangeCollection));
        }

        public static bool ImplementsInterface(this Type type, Type interfaceType)
        {
            return type.GetInterfaces().Any(t => t.Equals(interfaceType));
        }
        #endregion

        #region Private static methods
        private static LambdaExpression GetWherePropertyEquals(ParameterExpression parameterExpression, PropertyInfo property, object value)
        {
            Type function = typeof(Func<,>).MakeGenericType(property.ReflectedType, typeof(bool));
            LambdaExpression propertyEquals = default(LambdaExpression);
            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                MemberExpression propertyExpression = Expression.Property(parameterExpression, property);
                MemberExpression propertyHasValue = Expression.Property(propertyExpression, "HasValue");
                MemberExpression propertyActualValue = Expression.Property(propertyExpression, "Value");
                BinaryExpression hasValue = Expression.Equal(propertyHasValue, Expression.Constant(true));
                BinaryExpression equalsValue = Expression.Equal(propertyActualValue, Expression.Convert(Expression.Constant(value), property.PropertyType.GetGenericArguments()[0]));
                propertyEquals = Expression.Lambda(function, Expression.AndAlso(hasValue, equalsValue), parameterExpression);
            }
            else
            {
                MemberExpression propertyExpression = Expression.Property(parameterExpression, property);
                BinaryExpression propertyEqualsValue = Expression.Equal(propertyExpression, Expression.Convert(Expression.Constant(value), property.PropertyType));
                propertyEquals = Expression.Lambda(function, propertyEqualsValue, parameterExpression);
            }
            return propertyEquals;
        }

        private static LambdaExpression GetWherePropertiesEqual(ParameterExpression parameterExpression, Type reflectedType, IDictionary<PropertyInfo, object> properties)
        {
            Type function = typeof(Func<,>).MakeGenericType(reflectedType, typeof(bool));
            return Expression.Lambda(function, GetWherePropertiesEqual(parameterExpression, properties), parameterExpression);
        }

        private static Expression GetWherePropertiesEqual(ParameterExpression parameterExpression, IDictionary<PropertyInfo, object> properties)
        {
            Expression expression = Expression.Constant(true);
            foreach(KeyValuePair<PropertyInfo, object> property in properties)
            {
                if (property.Key.PropertyType.IsGenericType && property.Key.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    expression = Expression.AndAlso(expression, GetWhereNullablePropertyEquals(parameterExpression, property.Key, property.Value));
                else
                    expression = Expression.AndAlso(expression, GetWhereNonNullablePropertyEquals(parameterExpression, property.Key, property.Value));
            }
            return expression;
        }

        private static BinaryExpression GetWhereNullablePropertyEquals(ParameterExpression parameterExpression, PropertyInfo property, object value)
        {
            MemberExpression propertyExpression = Expression.Property(parameterExpression, property);
            MemberExpression propertyHasValue = Expression.Property(propertyExpression, "HasValue");
            MemberExpression propertyActualValue = Expression.Property(propertyExpression, "Value");
            BinaryExpression hasValue = Expression.Equal(propertyHasValue, Expression.Constant(true));
            BinaryExpression equalsValue = Expression.Equal(propertyActualValue, Expression.Convert(Expression.Constant(value), property.PropertyType.GetGenericArguments()[0]));
            return Expression.AndAlso(hasValue, equalsValue);
        }

        private static BinaryExpression GetWhereNonNullablePropertyEquals(ParameterExpression parameterExpression, PropertyInfo property, object value)
        {
            MemberExpression propertyExpression = Expression.Property(parameterExpression, property);
            return Expression.Equal(propertyExpression, Expression.Convert(Expression.Constant(value), property.PropertyType));
        }
        #endregion
    }
}
