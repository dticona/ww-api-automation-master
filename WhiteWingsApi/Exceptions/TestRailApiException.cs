// <copyright file="TestRailApiException.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Exceptions
{
    using System;
    using System.Runtime.Serialization;

    public class TestRailApiException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="APIException"/> class.
        /// </summary>
        public TestRailApiException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APIException"/> class
        /// with a message.
        /// </summary>
        /// <param name="message">Message sent to APIException</param>
        public TestRailApiException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APIException"/> class
        /// with a message and expection.
        /// </summary>
        /// <param name="message">Message sent to APIException</param>
        /// <param name="innerException">Inner exception</param>
        public TestRailApiException(
            string message,
            Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APIException"/> class
        /// with a info and context.
        /// </summary>
        /// <param name="info">Info message</param>
        /// <param name="context">Context message</param>
        protected TestRailApiException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
