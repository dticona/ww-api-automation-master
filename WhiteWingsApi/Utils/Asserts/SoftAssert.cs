// <copyright file="SoftAssert.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Utils.Asserts
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;

    public static class SoftAssert
    {
        private static List<string> errorsList = new List<string>();

        /// <summary>
        /// Soft assert test.
        /// </summary>
        /// <param name="expected">Expected object.</param>
        /// <param name="actual">Actual object.</param>
        /// <param name="errorMessage">Error message.</param>
        public static void AreEqualsIgnoreCase(string expected, string actual, string errorMessage)
        {
            if (!expected.Equals(actual, StringComparison.OrdinalIgnoreCase))
            {
                AddErrorMessage($"{errorMessage}. Objects must be equal: expected '{expected}', but actual: '{actual}'");
            }
        }

        /// <summary>
        /// Soft assert test.
        /// </summary>
        /// <param name="expected">Expected object.</param>
        /// <param name="actual">Actual object.</param>
        /// <param name="errorMessage">Error message.</param>
        public static void AreEquals(object expected, object actual, string errorMessage)
        {
            if (!expected.Equals(actual))
            {
                AddErrorMessage($"{errorMessage}. Objects must be equal: expected '{expected}', but actual: '{actual}'");
            }
        }

        /// <summary>
        /// Soft assert test.
        /// </summary>
        /// <param name="expected">Expected object.</param>
        /// <param name="actual">Actual object.</param>
        /// <param name="errorMessage">Error message.</param>
        public static void AreNotEqual(object expected, object actual, string errorMessage)
        {
            if (expected.Equals(actual))
            {
                AddErrorMessage($"{errorMessage}. Objects must not be equal: expected '{expected}', but actual: '{actual}'");
            }
        }

        /// <summary>
        /// Soft assert test.
        /// </summary>
        /// <param name="condition">Bool condition for check.</param>
        /// <param name="errorMessage">Error message.</param>
        public static void IsTrue(bool condition, string errorMessage)
        {
            if (!condition)
            {
                AddErrorMessage(errorMessage);
            }
        }

        /// <summary>
        /// Soft assert test.
        /// </summary>
        /// <param name="condition">Bool condition for check.</param>
        /// <param name="errorMessage">Error message.</param>
        public static void IsFalse(bool condition, string errorMessage)
        {
            if (condition)
            {
                AddErrorMessage(errorMessage);
            }
        }

        /// <summary>
        /// Soft assert test.
        /// </summary>
        /// <param name="partString">Part string for contains.</param>
        /// <param name="fullString">Full string where find part string.</param>
        /// <param name="errorMessage">Error message.</param>
        public static void Contains(string partString, string fullString, string errorMessage)
        {
            if (!fullString.Contains(partString))
            {
                AddErrorMessage($"{errorMessage}. Full string: '{fullString}' does not contain: '{partString}'");
            }
        }

        /// <summary>
        /// Assert If Expected value Not Null if null this assert wil be pass.
        /// </summary>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual vallue.</param>
        /// <param name="errorMessage">Error message.</param>
        public static void AssertIfExpectedNotNull<T>(T expected, T actual, string errorMessage)
        {
            if (expected != null)
            {
                AreEquals(expected, actual, errorMessage);
            }
        }

        /// <summary>
        /// Assert for array elements.
        /// </summary>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual vallue.</param>
        /// <param name="errorMessage">Error message.</param>
        public static void AssertArrays<T>(T[] expected, T[] actual, string errorMessage)
        {
            if (expected.Length != actual.Length)
            {
                AreEquals(expected.Length, actual.Length, $"{errorMessage} Array length differents");
                return;
            }

            for (var i = 0; i < expected.Length; i++)
            {
                AreEquals(expected[i], actual[i], errorMessage);
            }
        }

        /// <summary>
        /// Asserts test with all errors messages.
        /// And delete all errors messages.
        /// </summary>
        public static void AssertAll()
        {
            var errors = GetErrorMessages();
            if (HasErrors())
            {
                CleanErrorsMessages();
                Assert.Fail(errors);
            }
        }

        /// <summary>
        /// Get state of sort asserts.
        /// </summary>
        /// <returns>Bool state</returns>
        public static bool HasErrors()
        {
            return errorsList.Count > 0;
        }

        /// <summary>
        /// Delete all errors messages.
        /// </summary>
        public static void CleanErrorsMessages()
        {
            if (errorsList.Count > 0)
            {
                errorsList.Clear();
            }
        }

        /// <summary>
        /// Get error message.
        /// </summary>
        /// <returns>Error message</returns>
        public static string GetErrorMessages()
        {
            var messages = new StringBuilder("Soft assert errors: ");
            errorsList.ForEach(errorMessage => messages.Append("\n").Append(errorMessage));
            return messages.ToString();
        }

        private static void AddErrorMessage(string message)
        {
            errorsList.Add(message);
        }
    }
}
