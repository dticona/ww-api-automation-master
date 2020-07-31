// <copyright file="CheckMethodValidateTransferToAccount.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System;
    using System.Linq;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.TestData.Constants;
    using WhiteWingsApi.TestData.Results;
    using WhiteWingsApi.Utils.Asserts;
    using WhiteWingsApi.Utils.TestRail;
    using WhiteWingsApi.WWAPI;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodValidateTransferToAccount : BaseTest
    {
        private ParamValidateTransferToAccount paramValidateTransferToAccount = new ParamValidateTransferToAccount();

        [Test(Description = "Verify that a Result Code 4 = Blank Input is displayed in API method 'ValidateTransferToAccount()' response when the fields: Phone number / SubPayerID are empty")]
        [AllureTag("QGIN-4628", "C24253302")]
        [TestRailCaseId(24253302)]
        public void CheckValidateTransferToAccountResponseResultWhenFieldsAreEmpty()
        {
            paramValidateTransferToAccount.Msisdn = String.Empty;
            paramValidateTransferToAccount.SubPayerID = String.Empty;

            var response = LogStep("1 - Run the `ValidateTransferToAccount()` request on the soap API", () =>
            {
                return client.ValidateTransferToAccount(paramValidateTransferToAccount);
            });

            LogStep($"2 - Check response for method `ValidateTransferToAccount()` have correct error messages and result code'", () =>
            {
                var expectedResultCode = ValidateTransferToAccountResults.ResponseHasEmptyFieldsResult().code;
                var expectedErrorPhoneMessage = ValidateTransferToAccountResults.ResponseHasEmptyFieldsResult().messageForPhone;
                var expectedErrorSubPayerIdMessage = ValidateTransferToAccountResults.ResponseHasEmptyFieldsResult().messageForSubPayerId;
                var actualErrorMessages = response.ErrorMessage.ToList();
                SoftAssert.IsTrue(actualErrorMessages.ToList().Contains(expectedErrorPhoneMessage), $"Error message incorrect for method ValidateTransferToAccount()");
                SoftAssert.IsTrue(actualErrorMessages.ToList().Contains(expectedErrorSubPayerIdMessage), $"Error message incorrect for method ValidateTransferToAccount()");
                SoftAssert.AreEquals(expectedResultCode, response.ResultCode, $"Result code is {response.ResultCode}, but should be {expectedResultCode} ");
            });
        }

        [Test(Description = "Verify that a Result Code 5 = Invalid input SubPayerID Format is displayed in API method 'ValidateTransferToAccount()' response when the SubPayerID is filled with an invalid code")]
        [AllureTag("QGIN-4629", "C24253303")]
        [TestRailCaseId(24253303)]
        public void CheckValidateTransferToAccountResponseResultWhenSubPayerIdIsInvalid()
        {
            paramValidateTransferToAccount.Msisdn = MsisdnConstants.ValidMsisdn;
            paramValidateTransferToAccount.SubPayerID = SubPayerIdConstants.InvalidSubPayerID;

            var response = LogStep("1 - Run the `ValidateTransferToAccount()` request on the soap API", () =>
            {
                return client.ValidateTransferToAccount(paramValidateTransferToAccount);
            });

            LogStep($"2 - Check response for method `ValidateTransferToAccount()` have correct error messages and result code'", () =>
            {
                var expectedResultCode = ValidateTransferToAccountResults.ResponseHasInvalidSubPayerIdResult().code;
                var expectedErrorMessage = ValidateTransferToAccountResults.ResponseHasInvalidSubPayerIdResult().message;
                var actualErrorMessages = response.ErrorMessage.ToList();
                SoftAssert.IsTrue(actualErrorMessages.ToList().Contains(expectedErrorMessage), $"Error message incorrect for method ValidateTransferToAccount()");
                SoftAssert.AreEquals(expectedResultCode, response.ResultCode, $"Result code is {response.ResultCode}, but should be {expectedResultCode} ");
            });
        }

        [Test(Description = "Verify that a Result Code 6 = Invalid SubPayerID is displayed in API method 'ValidateTransferToAccount()' response when Subpayer is not listed for Mobile Wallet in WWC Database")]
        [AllureTag("QGIN-4630", "C24253304")]
        [TestRailCaseId(24253304)]
        public void CheckValidateTransferToAccountResponseResultWhenSubPayerIdIsNotListedForMobileWallet()
        {
            paramValidateTransferToAccount.Msisdn = MsisdnConstants.ValidMsisdn;
            paramValidateTransferToAccount.SubPayerID = SubPayerIdConstants.SubPayerIDNotListedForMobileWallet;

            var response = LogStep("1 - Run the `ValidateTransferToAccount()` request on the soap API", () =>
            {
                return client.ValidateTransferToAccount(paramValidateTransferToAccount);
            });

            LogStep($"2 - Check response for method `ValidateTransferToAccount()` have correct error messages and result code'", () =>
            {
                var expectedResultCode = ValidateTransferToAccountResults.ResponseHasSubPayerIdNotListedForMobileWalletResult().code;
                var expectedErrorMessage = ValidateTransferToAccountResults.ResponseHasSubPayerIdNotListedForMobileWalletResult().message;
                var actualErrorMessages = response.ErrorMessage.ToList();
                SoftAssert.IsTrue(actualErrorMessages.ToList().Contains(expectedErrorMessage), $"Error message incorrect for method ValidateTransferToAccount()");
                SoftAssert.AreEquals(expectedResultCode, response.ResultCode, $"Result code is {response.ResultCode}, but should be {expectedResultCode} ");
            });
        }
    }
}
