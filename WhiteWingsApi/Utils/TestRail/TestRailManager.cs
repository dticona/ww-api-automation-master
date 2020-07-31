// <copyright file="TestRailManager.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Utils.TestRail
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json.Linq;
    using NUnit.Framework;
    using NUnit.Framework.Interfaces;
    using WhiteWingsApi.Utils.Asserts;

    public static class TestRailManager
    {
        private static TestRailApiClient client;

        /// <summary>
        /// Adds a Result for a TC into the TestRail run
        /// </summary>
        /// <param name="testCaseId">Test case id in testrails</param>
        public static void AddResultTestCaseInTestRail(int testCaseId)
        {
            var data = new Dictionary<string, object>();
            UpdateData(data, GetStatus(), GetMessage(), GetException());
            ApiSendResults(testCaseId, data);
        }

        /// <summary>
        /// Get test case status.
        /// </summary>
        /// <returns>Status</returns>
        public static TestStatus GetStatus()
        {
            return SoftAssert.HasErrors() ? TestStatus.Failed : TestContext.CurrentContext.Result.Outcome.Status;
        }

        /// <summary>
        /// Get test case error message.
        /// </summary>
        /// <returns>Error message</returns>
        public static string GetMessage()
        {
            return SoftAssert.HasErrors() ? SoftAssert.GetErrorMessages() : TestContext.CurrentContext.Result.Message;
        }

        /// <summary>
        /// Get test case exception.
        /// </summary>
        /// <returns>Exception</returns>
        public static Exception GetException()
        {
            return SoftAssert.HasErrors() ? new Exception(GetMessage()) : new Exception(TestContext.CurrentContext.Result.Message);
        }

        /// <summary>
        /// Updates Data before sending to TR
        /// </summary>
        /// <param name="data">Data to be updated</param>
        /// <param name="testStatus">Test Result</param>
        /// <param name="testErrorMessage">Error type</param>
        /// <param name="error">Error exception</param>
        private static void UpdateData(Dictionary<string, object> data, TestStatus testStatus, string testErrorMessage, Exception error)
        {
            switch (testStatus)
            {
                case TestStatus.Failed:
                    data.Add("status_id", 5);
                    data.Add("comment", $"Type '{error.GetType().Name}', \nMessage: '{error.Message}'");
                    break;
                case TestStatus.Passed:
                    data.Add("status_id", 1);
                    data.Add("comment", "Passed by Automation Framework");
                    break;
                case TestStatus.Skipped:
                    data.Add("status_id", 6);
                    data.Add("comment", $"Skipped by Automation Framework due to: '{testErrorMessage}'");
                    break;
                default:
                    data.Add("status_id", 2);
                    data.Add("comment", $"Blocked by Automation Framework due to: '{testErrorMessage}'");
                    break;
            }
        }

        /// <summary>
        /// API send resulst to TestRail
        /// </summary>
        /// <param name="testCaseId">Int Case ID</param>
        /// <param name="data">Data to be sent</param>
        private static void ApiSendResults(int testCaseId, IDictionary<string, object> data)
        {
            var endpoint = "add_result_for_case/" + Configs.TestRailRunId + "/" + testCaseId;

            if (client == null)
            {
                client = new TestRailApiClient();
            }

            if (testCaseId == 0)
            {
                TestContext.WriteLine($"TestRail Case ID is 0. Return without sending any results");
                return;
            }

            try
            {
                var r = (JObject)client.SendPost(endpoint, data);
            }
            catch (Exception e)
            {
                TestContext.WriteLine($"Error while updating Test Rail Run '{Configs.TestRailRunId}'. Error:\n{e.Message}");
            }
        }
    }
}
