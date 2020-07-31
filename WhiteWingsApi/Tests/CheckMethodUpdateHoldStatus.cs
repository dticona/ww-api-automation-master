// <copyright file="CheckMethodUpdateHoldStatus.cs" company="IDT">
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
    public class CheckMethodUpdateHoldStatus : BaseTest
    {
        [Test(Description = "Verify that an error message is displayed in the API method 'UpdateHoldStatus()' response when the NewStatus field is a number boolean.")]
        [AllureTag("qgin-4614", "C23305021")]
        [TestRailCaseId(23305021)]
        public void CheckErrorMessageInUpdateHoldStatusResponseWithNewStatusBooleanValue()
        {
            var errorMessages = LogStep("1 - Run the `UpdateHoldStatus()` request on the soap API", () =>
            {
                var response = client.UpdateHoldStatus(CPasIdConstants.ValidCpasId, HoldStatusConstants.NewStatusIsBoolean);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Invalid New Status' error message", () =>
            {
                var expectedMessage = UpdateHoldStatusResults.StatusIsInvalid().message;
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify that a Result Code = 0 Transactions status changed is displayed in API method 'UpdateHoldStatus()' response when request is processed successfully.")]
        [AllureTag("qgin-4607", "C23190240")]
        [TestRailCaseId(23190240)]
        public void CheckErrorMessageInUpdateHoldStatusResponseWithTransactionThatStatusWasProcessingHold()
        {
            var paramProcessTransaction = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");

            LogStep("1 - Run the `ProcessTransaction()` request on the soap API that has Processing Hold status", () =>
            {
                paramProcessTransaction.CpasId = client.CreateNewCpasId().CPASId;
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.ProcessingHold;
                var processTransactionResponse = client.ProcessTransaction(paramProcessTransaction);
                var isResponseHaveErrors = processTransactionResponse.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method ProcessTransaction have error: '{processTransactionResponse.ErrorMessage}'");
            });

            var response = LogStep("2 - Run the `UpdateHoldStatus()` request on the soap API", () =>
            {
                return client.UpdateHoldStatus(paramProcessTransaction.CpasId, HoldStatusConstants.NewOpsHoldStatus);
            });

            LogStep("3 - Check that response has Result Code=0 and 'Transactions status changed from Processing Hold to Ops Hold' error message", () =>
                {
                    var expectedMessage = UpdateHoldStatusResults.StatusChangedToOpsHoldResult().message;
                    var expectedResultCode = UpdateHoldStatusResults.StatusChangedToOpsHoldResult().code;
                    SoftAssert.AreEquals(expectedResultCode, response.ResultCode, "The result code is differ from expected one");
                    Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
                });
        }

        [Test(Description = "Verify that a Result Code = 1 Invalid New Status is displayed in API method 'UpdateHoldStatus()' response when the Status is invalid.")]
        [AllureTag("qgin-4608", "C23190241")]
        [TestRailCaseId(23190241)]
        public void CheckErrorMessageInUpdateHoldStatusResponseWithNewInvalidStatus()
        {
            var response = LogStep("1 - Run the `UpdateHoldStatus()` request on the soap API with invalid new status", () =>
             {
                 return client.UpdateHoldStatus(CPasIdConstants.ValidCpasId, HoldStatusConstants.NewStatusIsInvalid);
             });

            LogStep("2 - Check that Result Code = 1 and response has 'Invalid New Status' error message", () =>
            {
                var expectedMessage = UpdateHoldStatusResults.StatusIsInvalid().message;
                var expectedResultCode = UpdateHoldStatusResults.StatusIsInvalid().code;
                SoftAssert.AreEquals(expectedResultCode, response.ResultCode, "The result code is differ from expected one");
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify that a Result Code = 3 = Transaction not on compliance /Processing Hold in API method 'UpdateHoldStatus()' response when the Transaction status is Unpaid.")]
        [AllureTag("qgin-4609", "C23190243")]
        [TestRailCaseId(23190243)]
        public void CheckErrorMessageInUpdateHoldStatusResponseWithNewTransactionOnProcessingHoldStatus()
        {
            var paramProcessTransaction = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");

            LogStep("1 - Run the `ProcessTransaction()` request on the soap API that has Processing Hold status", () =>
            {
                paramProcessTransaction.CpasId = client.CreateNewCpasId().CPASId;
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.OperationHold;
                var processTransactionResponse = client.ProcessTransaction(paramProcessTransaction);
                var isResponseHaveErrors = processTransactionResponse.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method ProcessTransaction have error: '{processTransactionResponse.ErrorMessage}'");
            });

            var response = LogStep("2 - Run the `UpdateHoldStatus()` request on the soap API", () =>
             {
                 return client.UpdateHoldStatus(paramProcessTransaction.CpasId, HoldStatusConstants.NewOpsHoldStatus);
             });

            LogStep("3 - Check that response has Result Code=3 and 'Transaction is not on Compliance/Processing Hold' error message", () =>
            {
                var expectedMessage = UpdateHoldStatusResults.StatusTransactionIsNotOnCompliance().message;
                var expectedResultCode = UpdateHoldStatusResults.StatusTransactionIsNotOnCompliance().code;
                SoftAssert.AreEquals(expectedResultCode, response.ResultCode, "The result code is differ from expected one");
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify that a Result Code = 4 = Required field alert in API method 'UpdateHoldStatus()' response when a new status field is not provide in the request.")]
        [AllureTag("qgin-4610", "C23190244")]
        [TestRailCaseId(23190244)]
        public void CheckErrorMessageInUpdateHoldStatusResponseWithNewStatusFieldNotProvided()
        {
            var response = LogStep("1 - Run the `UpdateHoldStatus()` request on the soap API with invalid new status", () =>
             {
                 return client.UpdateHoldStatus(CPasIdConstants.ValidCpasId, String.Empty);
             });

            LogStep("2 - Check that response has Result Code=4 and 'New Status is required.' error message", () =>
            {
                var expectedMessage = UpdateHoldStatusResults.StatusIsRequired().message;
                var expectedResultCode = UpdateHoldStatusResults.StatusIsRequired().code;
                SoftAssert.AreEquals(expectedResultCode, response.ResultCode, "The result code is differ from expected one");
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if 'UpdateHoldStatus()' shows Result Code 5 on a transaction that is already on compliance hold.")]
        [AllureTag("qgin-4611", "C23190245")]
        [TestRailCaseId(23190245)]
        public void CheckErrorMessageInUpdateHoldStatusResponseWithNewTransactionIsNotOnComplianceHold()
        {
            var paramProcessTransaction = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");

            LogStep("1 - Run the `ProcessTransaction()` request on the soap API that has Compliance Hold status", () =>
            {
                paramProcessTransaction.CpasId = client.CreateNewCpasId().CPASId;
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.ComplianseHold;
                var processTransactionResponse = client.ProcessTransaction(paramProcessTransaction);
                var isResponseHaveErrors = processTransactionResponse.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method ProcessTransaction have error: '{processTransactionResponse.ErrorMessage}'");
            });

            var response = LogStep("2 - Run the `UpdateHoldStatus()` request on the soap API", () =>
            {
                return client.UpdateHoldStatus(paramProcessTransaction.CpasId, HoldStatusConstants.NewOnTransactionHoldStatus);
            });

            LogStep("3 - Check that response has Result Code=5 and 'Transaction is already on Compliance Hold' error message", () =>
            {
                var expectedMessage = UpdateHoldStatusResults.StatusTransactionIsAlreadyOnCompliance().message;
                var expectedResultCode = UpdateHoldStatusResults.StatusTransactionIsAlreadyOnCompliance().code;
                SoftAssert.AreEquals(expectedResultCode, response.ResultCode, "The result code is differ from expected one");
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if 'UpdateHoldStatus()' method response shows Result Code = 6 when the Cpas ID field is invalid.")]
        [AllureTag("qgin-4612", "C23190247")]
        [TestRailCaseId(23190247)]
        public void CheckErrorMessageInUpdateHoldStatusResponseWithInvalidCpasId()
        {
            var response = LogStep("1 - Run the `UpdateHoldStatus()` request on the soap API with invalid CpasId", () =>
            {
                return client.UpdateHoldStatus(CPasIdConstants.InvalidCpasId, HoldStatusConstants.NewOpsHoldStatus);
            });

            LogStep("2 - Check that response has Result Code=6 and 'Invalid CPAS ID' error message", () =>
            {
                var expectedMessage = UpdateHoldStatusResults.CpasIsInvalid().message;
                var expectedResultCode = UpdateHoldStatusResults.CpasIsInvalid().code;
                SoftAssert.AreEquals(expectedResultCode, response.ResultCode, "The result code is differ from expected one");
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify that an error message is displayed in the API method 'UpdateHoldStatus()' response when the NewStatus field is a negative number.")]
        [AllureTag("qgin-4613", "C23302014")]
        [TestRailCaseId(23302014)]
        public void CheckErrorMessageInUpdateHoldStatusResponseWhenNewStatusFieldIsNegativeNumber()
        {
            var response = LogStep("1 - Run the `UpdateHoldStatus()` request on the soap API when New Status Field Is Negative Number", () =>
            {
                return client.UpdateHoldStatus(CPasIdConstants.ValidCpasId, HoldStatusConstants.NewStatusIsNegativeNumber);
            });

            LogStep("2 - Check that response has 'Invalid Status' error message", () =>
            {
                var expectedMessage = UpdateHoldStatusResults.StatusIsInvalid().message;
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if its possible change status Compliance Hold to Operation Hold by API method 'UpdateHoldStatus()'.")]
        [AllureTag("qgin-4618", "C23423074")]
        [TestRailCaseId(23423074)]
        public void CheckItsPossibleChangeStatusComplianceHoldToOperationHoldByUpdateHoldStatusResponse()
        {
            var paramProcessTransaction = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");

            LogStep("1 - Run the `ProcessTransaction()` request on the soap API that has Compliance Hold status", () =>
            {
                paramProcessTransaction.CpasId = client.CreateNewCpasId().CPASId;
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.ComplianseHold;
                var processTransactionResponse = client.ProcessTransaction(paramProcessTransaction);
                var isResponseHaveErrors = processTransactionResponse.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method ProcessTransaction have error: '{processTransactionResponse.ErrorMessage}'");
            });

            var response = LogStep("2 - Run the `UpdateHoldStatus()` request on the soap API", () =>
            {
                return client.UpdateHoldStatus(paramProcessTransaction.CpasId, HoldStatusConstants.NewOpsHoldStatus);
            });

            LogStep("3 - Check that response has Result Code=5 and 'Transactions status changed from Compliance Hold to Ops Hold' message", () =>
            {
                var expectedMessage = UpdateHoldStatusResults.StatusChangedFromComplianceHoldToOpsHoldResult().message;
                var expectedResultCode = UpdateHoldStatusResults.StatusChangedFromComplianceHoldToOpsHoldResult().code;
                SoftAssert.AreEquals(expectedResultCode, response.ResultCode, "The result code is differ from expected one");
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if its possible change status Processing Hold to Compliance Hold by API method 'UpdateHoldStatus()'.")]
        [AllureTag("qgin-4619", "C23423075")]
        [TestRailCaseId(23423075)]
        public void CheckItIsPossibleToChangeStatusProcessingHoldToComplianceHoldByUpdateHoldStatusResponse()
        {
            var paramProcessTransaction = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");

            LogStep("1 - Run the `ProcessTransaction()` request on the soap API that has Processing Hold status", () =>
            {
                paramProcessTransaction.CpasId = client.CreateNewCpasId().CPASId;
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.ProcessingHold;
                var processTransactionResponse = client.ProcessTransaction(paramProcessTransaction);
                var isResponseHaveErrors = processTransactionResponse.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method ProcessTransaction have error: '{processTransactionResponse.ErrorMessage}'");
            });

            var response = LogStep("2 - Run the `UpdateHoldStatus()` request on the soap API", () =>
            {
                return client.UpdateHoldStatus(paramProcessTransaction.CpasId, HoldStatusConstants.NewOnTransactionHoldStatus);
            });

            LogStep("3 - Check that response return 'Transactions status changed from Processing Hold to Compliance Hold' message", () =>
            {
                var expectedMessage = UpdateHoldStatusResults.StatusChangedToComplianceHold().message;
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if 'UpdateHoldStatus()' method response shows Result Code = 4 when a CpasId field is Empty")]
        [AllureTag("qgin-4620", "C23701051")]
        [TestRailCaseId(23701051)]
        public void CheckUpdateHoldStatusResponseShowsResultCodeWhenCpasFieldIsEmpty()
        {
            var response = LogStep("1 - Run the `UpdateHoldStatus()` request on the soap API with empty CpasId", () =>
            {
                return client.UpdateHoldStatus(String.Empty, HoldStatusConstants.NewOpsHoldStatus);
            });

            LogStep("2 - Check that response has Result Code=4 and 'CPAS ID is required.' error message", () =>
            {
                var expectedMessage = UpdateHoldStatusResults.CpasIsRequired().message;
                var expectedResultCode = UpdateHoldStatusResults.CpasIsRequired().code;
                SoftAssert.AreEquals(expectedResultCode, response.ResultCode, "The result code is differ from expected one");
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify that an error message is displayed in the API method 'UpdateHoldStatus' response when the New Status field is 0.")]
        [AllureTag("qgin-4616", "C26857346")]
        [TestRailCaseId(26857346)]
        public void CheckErrorMessageInUpdateHoldStatusResponseWithZeroNewStatus()
        {
            var errorMessages = LogStep("1 - Run the `UpdateHoldStatus()` request on the soap API", () =>
            {
                var response = client.UpdateHoldStatus(CPasIdConstants.ValidCpasId, HoldStatusConstants.NewStatusIsZero);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Invalid New Status' error message", () =>
            {
                var expectedMessage = UpdateHoldStatusResults.StatusIsInvalid().message;
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify Result Code = 3 when Transaction not on compliance /Processing Hold")]
        [AllureTag("QGIN-3925", "C17270228")]
        [TestRailCaseId(17270228)]
        public void CheckResultCodeWhenTheTransactionIsNotOnHold()
        {
            var unPaidTransaction = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
            unPaidTransaction.CpasId = client.CreateNewCpasId().CPASId;
            unPaidTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.UnPaid;
            var transactionResponse = client.ProcessTransaction(unPaidTransaction);

            var updateHoldStatus = LogStep("1 - Run the `UpdateHoldStatus()` request using the UnPaidTransction", () =>
            {
                var response = client.UpdateHoldStatus(transactionResponse.CPASId, HoldStatusConstants.NewOnTransactionHoldStatus);
                return response;
            });

            LogStep("2 - Check that the response has the ResultCode=3", () =>
            {
                var expectedMessage = "Transaction is not on Compliance/Processing Hold";
                SoftAssert.Contains(expectedMessage, updateHoldStatus.ErrorMessage[0], $"Expected error message is {expectedMessage} but was {updateHoldStatus.ErrorMessage.ToList()}");
                SoftAssert.AreEquals(3, updateHoldStatus.ResultCode, "Wrong ResultCode Updating Hold Transaction status");
            });
        }
    }
}
