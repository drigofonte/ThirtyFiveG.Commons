using System;

namespace ThirtyFiveG.Commons.Extensions
{
    public static class ObjectExtensions
    {
        public static object ChangeType(this object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return Convert.ChangeType(value, t, null);
        }

        public static bool IsNumber(this object value)
        {
            return value is sbyte
                || value is byte
                || value is short
                || value is ushort
                || value is int
                || value is uint
                || value is long
                || value is ulong
                || value is float
                || value is double
                || value is decimal;
        }

        public static bool ExtendedEquals(this object n1, object n2)
        {
            bool equals = n1 != null && n2 != null;
            if (equals)
                equals &= IsNumber(n1) && IsNumber(n2) ? Convert.ToDecimal(n1).Equals(Convert.ToDecimal(n2)) : n1.Equals(n2);
            return equals;
        }
    }
}
