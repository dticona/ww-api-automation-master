// <copyright file="CheckMethodGetPointOfContactListBySubPayer.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System;
    using System.Linq;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.Enums;
    using WhiteWingsApi.TestData.Constants;
    using WhiteWingsApi.TestData.Results;
    using WhiteWingsApi.Utils.Asserts;
    using WhiteWingsApi.Utils.TestRail;
    using WhiteWingsApi.WWAPI;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodGetPointOfContactListBySubPayer : BaseTest
    {
        [Test(Description = "Verify if an error message is displayed on GetPointOfContactListBySubPayer")]
        [AllureTag("qgin-4601", "C29360811")]
        [TestRailCaseId(29360811)]
        public void CheckErrorMessageInGetPointOfContactListBySubPayerResponse()
        {
            var response = LogStep("1 - Run the `GetPointOfContactListBySubPayer()` request on the soap API", () =>
            {
                return client.GetPointofContactListBySubpayer(SubPayerIdConstants.InvalidSubPayerIdTypeLong, true);
            });

            LogStep("2 - Check that response has Result Code=3 and 'Invalid SubPayer Id.' error message", () =>
                {
                    var expectedMessage = GetPointOfContactListBySubPayerResults.ResponseHasInvalidSubPayerIdResult().message;
                    var expectedResultCode = GetPointOfContactListBySubPayerResults.ResponseHasInvalidSubPayerIdResult().code;
                    SoftAssert.AreEquals(expectedResultCode, response.ResultCode, "The result code is differ from expected one");
                    SoftAssert.AreEquals(expectedMessage, response.ErrorMessage, $"'{expectedMessage}'error message is not returned in response!");
                });
        }
    }
}
