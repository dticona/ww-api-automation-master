// <copyright file="EntityAssert.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Utils.Asserts
{
    using System.Collections;

    public static class EntityAssert
    {
        /// <summary>
        /// Check property values for entity but if expected entity have null value of property this property do not check.
        /// </summary>
        /// <param name="expected">Expected property.</param>
        /// <param name="actual">Actual property.</param>
        /// <param name="commonErrorMessage">Common error message.</param>
        public static void SoftAssertNotNullPropertyValues<T>(T expected, T actual, string commonErrorMessage)
        {
            var expectedPropertiesDictionary = EntityHelper.GetProperties(expected);
            var actualPropertiesDictionary = EntityHelper.GetProperties(actual);

            foreach (var key in expectedPropertiesDictionary.Keys)
            {
                var expectedPropertyValue = expectedPropertiesDictionary[key];

                if (expectedPropertyValue.GetType().IsArray)
                {
                    var expectedArray = ConvertToArray(expectedPropertyValue);
                    var actualArray = ConvertToArray(actualPropertiesDictionary[key]);
                    SoftAssert.AssertArrays(expectedArray, actualArray, $"{commonErrorMessage} Incorrect value for property '{key}'");
                }
                else
                {
                    SoftAssert.AssertIfExpectedNotNull(expectedPropertyValue, actualPropertiesDictionary[key], $"{commonErrorMessage} Incorrect value for property '{key}'");
                }
            }
        }

        /// <summary>
        /// Cast instance of object to type object[].
        /// </summary>
        /// <param name="obj">Object for cast.</param>
        /// <returns>Casted array.</returns>
        private static object[] ConvertToArray(object obj)
        {
            var enumerable = obj as IEnumerable;
            var list = new ArrayList();
            foreach (var item in enumerable)
            {
                list.Add(item);
            }

            return list.ToArray();
        }
    }
}
