// <copyright file="FileManager.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Utils
{
    using System;
    using System.IO;
    using System.Reflection;
    using Newtonsoft.Json;
    using WhiteWingsApi.Exceptions;

    public class JsonUtils
    {
        private readonly string defaultTestDataFolderName = "TestData";

        /// <summary>
        /// Find file by file name and deserialized json file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Deserialized object.</returns>
        public T ConvertJsonToObjectByFileName<T>(string fileName)
        {
            return ConvertJsonToObject<T>(GetFilePath(fileName));
        }

        /// <summary>
        /// Manages file path for a deserialized json file.
        /// </summary>
        /// <param name="fullFilePath">Path to a file.</param>
        /// <returns>Deserialized object.</returns>
        public T ConvertJsonToObject<T>(string fullFilePath)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(fullFilePath));
        }

        /// <summary>
        /// Searchs for the requested file in Debug folder.
        /// </summary>
        /// <param name="fileName">File name with its extention (Name.txt, Name.json, etc).</param>
        /// <returns>Path of requested file.</returns>
        public string GetFilePath(string fileName)
        {
            var debugPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), defaultTestDataFolderName);
            if (Directory.GetFiles(new Uri(debugPath).LocalPath, fileName, SearchOption.AllDirectories).Length == 0)
            {
                throw new FileNotFoundException("File not found.");
            }

            if (Directory.GetFiles(new Uri(debugPath).LocalPath, fileName, SearchOption.AllDirectories).Length > 1)
            {

                throw new DuplicateFileException($"Duplicate files with name '{fileName}'.");
            }

            return Directory.GetFiles(new Uri(debugPath).LocalPath, fileName, SearchOption.AllDirectories)[0];
        }
    }
}
