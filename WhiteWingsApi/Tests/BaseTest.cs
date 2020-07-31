// <copyright file="BaseTest.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi
{
    using System;
    using Allure.Commons;
    using NUnit.Allure.Core;
    using NUnit.Framework;
    using WhiteWingsApi.Framework.Api;
    using WhiteWingsApi.Utils;
    using WhiteWingsApi.Utils.Asserts;
    using WhiteWingsApi.WWAPI;

    [AllureNUnit]
    public class BaseTest
    {
        protected WhiteWingsClient client;
        protected JsonUtils jsonUtils;

        /// <summary>
        /// Execute and create the client according to the URL service
        /// </summary>
        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            client = WhiteWingsClient.InstanceClient();
            jsonUtils = new JsonUtils();
        }

        /// <summary>
        /// Executes process after finish all the tests.
        /// </summary>
        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            WhiteWingsClient.CloseInstanceClient();
        }

        /// <summary>
        /// Executes process after each test method.
        /// </summary>
        [TearDown]
        public void TestMethodTearDown()
        {
            SoftAssert.AssertAll();
            SoftAssert.CleanErrorsMessages();
        }

        /// <summary>
        /// Log step in allure report and action in step.
        /// </summary>
        /// <param name="stepName">Step name.</param>
        /// <param name="action">Step Action.</param>
        /// <returns>Step action result.</returns>
        public T LogStep<T>(string stepName, Func<T> action)
        {
            T result = default(T);
            AllureLifecycle.Instance.WrapInStep(() => { result = action(); }, stepName);
            return result;
        }

        /// <summary>
        /// Log step in allure report and action in step.
        /// </summary>
        /// <param name="stepName">Step name.</param>
        /// <param name="action">Step Action.</param>
        public void LogStep(string stepName, Action action)
        {
            AllureLifecycle.Instance.WrapInStep(() => { action(); }, stepName);
        }

        /// <summary>
        /// Find file by file name and deserialized json file.
        /// </summary>
        /// <param name="fileName">File name.</param>
        /// <returns>Deserialized object.</returns>
        public T ConvertJsonToObject<T>(string fileName)
        {
            return jsonUtils.ConvertJsonToObjectByFileName<T>(fileName);
        }
    }
}
