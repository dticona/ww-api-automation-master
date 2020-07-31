// <copyright file="EntityHelper.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Utils
{
    using System.Collections.Generic;
    using System.Linq;

    public static class EntityHelper
    {
        /// <summary>
        /// Parse object property values to dictionary.
        /// </summary>
        /// <param name="entity">Instance of object for parse.</param>
        /// <returns>Parsed dictionary.</returns>
        public static Dictionary<string, object> GetProperties<T>(T entity)
        {
            var propertiesDictionary = new Dictionary<string, object>();
            entity.GetType().GetProperties().ToList().ForEach(property => propertiesDictionary.Add(property.Name, property.GetValue(entity)));
            return propertiesDictionary;
        }
    }
}
