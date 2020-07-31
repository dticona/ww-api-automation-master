// <copyright file="StringHelper.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class StringHelper
    {
        /// <summary>
        /// Obtains a random alphanumeric string by giving length value.
        /// </summary>
        /// <param name="length"> It's the length of the required random string.</param>
        /// <returns> Random string.</returns>
        public static string GenerateRandomString(int length)
        {
            return GenerateRandom("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", length);
        }

        /// <summary>
        /// Obtains a random string by giving length value.
        /// </summary>
        /// <param name="characters">It's the characters</param>
        /// <param name="length">It's the length of the required random string.</param>
        /// <returns>Random string.</returns>
        public static string GenerateRandom(string characters, int length)
        {
            Random random = new Random();
            return new string(Enumerable.Repeat(characters, length)
                .Select(generateString => generateString[random.Next(generateString.Length)]).ToArray());
        }
    }
}
