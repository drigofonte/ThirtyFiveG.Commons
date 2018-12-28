using System;
using System.Collections;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;

namespace ThirtyFiveG.Commons.Common
{
    public class DataBinder
    {
        private static readonly char[] expressionPartSeparator = new char[] { '.' };
        private static readonly char[] indexExprStartChars = new char[] { '[', '(' };
        private static readonly char[] indexExprEndChars = new char[] { ']', ')' };

        public static object Eval(object container, string expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }

            expression = expression.Trim();

            if (expression.Length == 0)
            {
                throw new ArgumentNullException("expression");
            }

            if (container == null)
            {
                return null;
            }

            string[] expressionParts = expression.Split(expressionPartSeparator);

            return DataBinder.Eval(container, expressionParts);
        }

        private static object Eval(object container, string[] expressionParts)
        {
            Debug.Assert((expressionParts != null) && (expressionParts.Length != 0),
                         "invalid expressionParts parameter");

            object prop;
            int i;

            for (prop = container, i = 0; (i < expressionParts.Length) && (prop != null); i++)
            {
                string expr = expressionParts[i];
                bool indexedExpr = expr.IndexOfAny(indexExprStartChars) >= 0;

                if (indexedExpr == false)
                {
                    prop = DataBinder.GetPropertyValue(prop, expr);
                }
                else
                {
                    prop = DataBinder.GetIndexedPropertyValue(prop, expr);
                }
            }

            return prop;
        }

        public static object GetPropertyValue(object container, string propName)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (String.IsNullOrEmpty(propName))
            {
                throw new ArgumentNullException("propName");
            }

            object prop = null;

            // get a PropertyDescriptor using case-insensitive lookup 
            PropertyInfo pd = container.GetType().GetProperty(propName);
            if (pd != null)
            {
                prop = pd.GetValue(container, null);
            }
            else
            {
                throw new Exception("Could not bind property: "+container.GetType().FullName + "." + propName);
            }

            return prop;
        }

        public static object GetIndexedPropertyValue(object container, string expr)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            if (String.IsNullOrEmpty(expr))
            {
                throw new ArgumentNullException("expr");
            }

            object prop = null;
            bool intIndex = false;

            int indexExprStart = expr.IndexOfAny(indexExprStartChars);
            int indexExprEnd = expr.IndexOfAny(indexExprEndChars, indexExprStart + 1);

            if ((indexExprStart < 0) || (indexExprEnd < 0) ||
                (indexExprEnd == indexExprStart + 1))
            {
                throw new ArgumentException("Invalid indexed expression: " + expr);
            }

            string propName = null;
            object indexValue = null;
            string index = expr.Substring(indexExprStart + 1, indexExprEnd - indexExprStart - 1).Trim();

            if (indexExprStart != 0)
                propName = expr.Substring(0, indexExprStart);

            if (index.Length != 0)
            {
                if (((index[0] == '"') && (index[index.Length - 1] == '"')) ||
                    ((index[0] == '\'') && (index[index.Length - 1] == '\'')))
                {
                    indexValue = index.Substring(1, index.Length - 2);
                }
                else
                {
                    if (Char.IsDigit(index[0]))
                    {
                        // treat it as a number
                        int parsedIndex;
                        intIndex = Int32.TryParse(index, NumberStyles.Integer, CultureInfo.InvariantCulture, out parsedIndex);
                        if (intIndex)
                        {
                            indexValue = parsedIndex;
                        }
                        else
                        {
                            indexValue = index;
                        }
                    }
                    else
                    {
                        // treat as a string 
                        indexValue = index;
                    }
                }
            }

            if (indexValue == null)
            {
                throw new ArgumentException("Invalid indexed expression: " + expr);
            }

            object collectionProp = null;
            if ((propName != null) && (propName.Length != 0))
            {
                collectionProp = DataBinder.GetPropertyValue(container, propName);
            }
            else
            {
                collectionProp = container;
            }

            if (collectionProp != null)
            {
                Array arrayProp = collectionProp as Array;
                if (arrayProp != null && intIndex)
                {
                    prop = arrayProp.GetValue((int)indexValue);
                }
                else if ((collectionProp is IList) && intIndex)
                {
                    prop = ((IList)collectionProp)[(int)indexValue];
                }
                else
                {
                    PropertyInfo propInfo =
                        collectionProp.GetType().GetProperty("Item", BindingFlags.Public | BindingFlags.Instance);
                    if (propInfo != null)
                    {
                        prop = propInfo.GetValue(collectionProp, new object[] { indexValue });
                    }
                    else
                    {
                        throw new ArgumentException("No indexed accessor: "+ collectionProp.GetType().FullName);
                    }
                }
            }

            return prop;
        }
    }
}
