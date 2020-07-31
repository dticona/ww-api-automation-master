// <copyright file="DuplicateFileException.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Exceptions
{
    using System;

    public class DuplicateFileException : Exception
    {
        public DuplicateFileException(string message)
            : base(message) { }
    }
}
