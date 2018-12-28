using System;
using System.Collections.Generic;

namespace ThirtyFiveG.Commons.Common
{
    public class Enum<T> where T : struct, IConvertible
    {
        #region Public properties
        public static IEnumerable<T> AsEnumerable
        {
            get
            {
                Type enumType = GetEnumType();

                Array enumValArray = Enum.GetValues(enumType);
                List<T> enumValList = new List<T>(enumValArray.Length);

                foreach (int val in enumValArray)
                {
                    enumValList.Add((T)Enum.Parse(enumType, val.ToString(), true));
                }

                return enumValList;
            }
        }

        public static string[] AsNamesArray
        {
            get
            {
                Type enumType = GetEnumType();
                return Enum.GetNames(enumType) as string[];
            }
        }
        #endregion

        #region Public methods
        public static IEnumerable<int> StartWith(string value)
        {
            value = value.ToLower();
            string[] names = AsNamesArray;
            List<int> indices = new List<int>();

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i].ToLower().StartsWith(value))
                {
                    indices.Add(i);
                }
            }

            return indices;
        }
        #endregion

        #region Private methods
        private static Type GetEnumType()
        {
            Type enumType = typeof(T);

            // Can't use generic type constraints on value types,
            // so have to do check like this
            if (enumType.BaseType != typeof(Enum))
                throw new ArgumentException("T must be of type System.Enum");
            return enumType;
        }
        #endregion
    }
}
