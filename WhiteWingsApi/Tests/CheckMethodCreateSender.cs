// <copyright file="CheckMethodCreateSender.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.Utils;
    using WhiteWingsApi.Utils.TestRail;
    using WhiteWingsApi.WWAPI;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodCreateSender : BaseTest
    {
        [Test(Description = "Verify that it is not possible to create a Sender with an invalid 'DOB'.")]
        [AllureTag("qgin-4557", "C17165285")]
        [TestRailCaseId(17165285)]
        public void CheckItIsNotPossibleCreateNewSenderWithInvalidDOB()
        {
            var paramCreateSender = ConvertJsonToObject<ParamCreateSender>("ParamCreateSender.json");

            var errorMessages = LogStep("1 - Run the `CreateSender()` request on the soap API", () =>
            {
                paramCreateSender.DOB = DateTime.Today.AddMonths(1).ToString("MM-dd-yyyy");
                return client.CreateSender(paramCreateSender).ErrorMessage;
            });

            LogStep("2 - Check that response has 'Invalid date of birth.' error message", () =>
            {
                var expectedMessage = "Invalid date of birth.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify that is possible to create a sender")]
        [AllureTag("MTSQ-4516", "C17163460")]
        [TestRailCaseId(17163460)]
        public void CheckIfIsPossibleToCreateSender()
        {
            var paramCreateSender = ConvertJsonToObject<ParamCreateSender>("ParamCreateSender.json");

            var newSender = LogStep("1. Make a request to CreateSender() with the following values", () =>
            {
                paramCreateSender.SenderFirstName = StringHelper.GenerateRandomString(5);
                paramCreateSender.SenderMiddleName = StringHelper.GenerateRandomString(5);
                paramCreateSender.SenderLastName = StringHelper.GenerateRandomString(5);
                return client.CreateSender(paramCreateSender);
            });

            LogStep("2. Verify if the response returned the Sender Id and has not errors", () =>
            {
                Assert.IsNull(newSender.ErrorMessage);
                Assert.NotZero(newSender.MTS_Sender_UID, "The sender was not created");
            });
        }
    }
}
