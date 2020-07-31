// <copyright file="AllureHelper.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Utils
{
    using System.Text;
    using Allure.Commons;

    /// <summary>
    /// This class handles the AllureHelper.
    /// </summary>
    public static class AllureHelper
    {
        /// <summary>
        /// Add attachement for allure report in xml format.
        /// </summary>
        /// <param name="name">Attachment name which will be visible in report.</param>
        /// <param name="xml">Xml for attach.</param>
        public static void AddAtachmentXml(string name, string xml)
        {
            AllureLifecycle.Instance.AddAttachment(name, "application/xml", Encoding.UTF8.GetBytes(xml), "xml");
        }
    }
}
