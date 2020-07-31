// <copyright file="CheckMethodProcessTransaction.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System.Linq;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.Enums;
    using WhiteWingsApi.TestData.Constants;
    using WhiteWingsApi.Utils.Asserts;
    using WhiteWingsApi.Utils.TestRail;
    using WhiteWingsApi.WWAPI;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodProcessTransaction : BaseTest
    {
        private ParamProcessTransaction paramProcessTransaction;

        [SetUp]
        public void CreateNewCpasIdAndPrepareDataToProcessTransaction()
        {
            var response = client.CreateNewCpasId();
            paramProcessTransaction = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
            paramProcessTransaction.CpasId = response.CPASId;
        }

        [Test(Description = "Process a transaction that has PROCESSING HOLD status")]
        [AllureTag("qgin-1409", "C4737406")]
        [TestRailCaseId(4737406)]
        public void CheckProcessTransactionHasHoldStatus()
        {
            var orderStatus = LogStep("1 - Run the `ProcessTransaction()` request on the soap API", () =>
            {
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.ProcessingHold;
                var response = client.ProcessTransaction(paramProcessTransaction);
                var isResponseHaveErrors = response.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method ProcessTransaction have error: '{response.ErrorMessage}'");
                return response.OrderStatus;
            });

            LogStep("2 - Check that transaction has 'Processing hold' status", () =>
            {
                var expectedStatus = "Transaction is on Processing Hold";
                Assert.AreEqual(expectedStatus, orderStatus, $"Actual order status of the transaction is '{orderStatus}', but expected was '{expectedStatus}'");
            });
        }

        [Test(Description = "Get ProcessTransaction () using an 'Invalid POC ID'.")]
        [AllureTag("qgin-4604", "C17154707")]
        [TestRailCaseId(17154707)]
        public void CheckErrorMessageProcessTransactionWithInvalidPocId()
        {
            var response = LogStep("1 - Run the `ProcessTransaction()` request on the soap API", () =>
            {
                paramProcessTransaction.POCID = PocIdConstants.InvalidPocId;
                return client.ProcessTransaction(paramProcessTransaction);
            });

            LogStep("2 - Check that response has 'Invalid Point of Contact' error message", () =>
            {
                var expectedMessage = "Invalid Point of Contact";
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Get ProcessTransaction () using an invalid 'ComplianceAgentID'.")]
        [AllureTag("qgin-4605", "C17160072")]
        [TestRailCaseId(17160072)]
        public void CheckErrorMessageProcessTransactionWithInvalidComplianceAgentId()
        {
            var response = LogStep("1 - Run the `ProcessTransaction()` request on the soap API", () =>
            {
                paramProcessTransaction.ComplianceAgentID = ComplianceAgentIdConstants.InvalidComplianceAgentId;
                return client.ProcessTransaction(paramProcessTransaction);
            });

            LogStep("2 - Check that response has 'Invalid Compliance Agent ID' error message", () =>
            {
                var expectedMessage = "Invalid Compliance Agent ID";
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Process a transaction that has Operation Hold status by ProcessTransaction() Method")]
        [AllureTag("qgin-4509", "C23701050")]
        [TestRailCaseId(23701050)]
        public void CheckProcessTransactionHasOperationHoldStatus()
        {
            var orderStatus = LogStep("1 - Run the `ProcessTransaction()` request on the soap API", () =>
            {
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.OperationHold;
                var response = client.ProcessTransaction(paramProcessTransaction);
                var isResponseHaveErrors = response.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method ProcessTransaction have error: '{response.ErrorMessage}'");
                return response.OrderStatus;
            });

            LogStep("2 - Check that transaction has 'Operation Hold' status", () =>
            {
                var expectedStatus = "Operation Hold";
                Assert.AreEqual(expectedStatus, orderStatus, $"Actual order status of the transaction is '{orderStatus}', but expected was '{expectedStatus}'");
            });
        }

        [Test(Description = "Process a transaction that has Compliance Hold status by ProcessTransaction() Method.")]
        [AllureTag("qgin-4510", "C23702112")]
        [TestRailCaseId(23702112)]
        public void CheckProcessTransactionHasComplianceHoldStatus()
        {
            var orderStatus = LogStep("2 - Run the `ProcessTransaction()` request on the soap API", () =>
            {
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.ComplianseHold;
                var response = client.ProcessTransaction(paramProcessTransaction);
                var isResponseHaveErrors = response.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method ProcessTransaction have error: '{response.ErrorMessage}'");
                return response.OrderStatus;
            });

            LogStep("3 - Check that transaction has 'Transaction is on Compliance Hold' status", () =>
            {
                var expectedStatus = "Transaction is on Compliance Hold";
                Assert.AreEqual(expectedStatus, orderStatus, $"Actual order status of the transaction is '{orderStatus}', but expected was '{expectedStatus}'");
            });
        }

        [Test(Description = "Verify if process a transaction with HoldOrderFlag:0 a error message is displayed")]
        [AllureTag("qgin-3927", "C17369170")]
        [TestRailCaseId(17369170)]
        public void CheckErrorMessageProcessTransactionWithZeroHoldOrderFlag()
        {
            var response = LogStep("1 - Run the `ProcessTransaction()` request on the soap API", () =>
            {
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.ZeroHoldOrderFlag;
                return client.ProcessTransaction(paramProcessTransaction);
            });

            LogStep("2 - Check that response has 'Hold order flag is required' error message", () =>
            {
                var expectedMessage = "Hold order flag is required";
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = " Verify that processing a transaction using an 'invalid Payer ID' an error message is displayed")]
        [AllureTag("qgin-3948", "C17153355")]
        [TestRailCaseId(17153355)]
        public void CheckErrorMessageProcessTransactionWithInvalidPayerId()
        {
            var response = LogStep("1 - Run the `ProcessTransaction()` request on the soap API", () =>
            {
                paramProcessTransaction.PayerId = PayerIdConstants.InvalidPayerID;
                return client.ProcessTransaction(paramProcessTransaction);
            });

            LogStep("2 - Check that response has 'Invalid Payer Id.' error message", () =>
            {
                var expectedMessage = "Invalid Payer Id.";
                Assert.Contains(expectedMessage, response.ErrorMessage.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Process a transaction that has UnPaid status by ProcessTransaction() Method")]
        [AllureTag("qgin-3924", "C17270217")]
        [TestRailCaseId(17270217)]
        public void CheckProcessTransactionUnPaidStatus()
        {
            var orderStatus = LogStep("1 - Run the `ProcessTransaction()` request", () =>
            {
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.UnPaid;
                var response = client.ProcessTransaction(paramProcessTransaction);
                var isResponseHaveErrors = response.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method ProcessTransaction have error: '{response.ErrorMessage}'");
                return response.OrderStatus;
            });

            LogStep("2 - Check that transaction has 'UNPAID' status", () =>
            {
                Assert.AreEqual("UNPAID", orderStatus, "Unexpected Transaction status");
            });
        }

        [Test(Description = "Verify that processing a transaction using an 'Invalid Payment Method' a error message is returned on the response")]
        [AllureTag("qgin-3920", "C17153354")]
        [TestRailCaseId(17153354)]
        public void CheckProcessTransactionWithInvalidPaymentMehod()
        {
            var errorMessage = LogStep("1 - Run the ProcessTransaction() request using an Invalid Mode of payment", () =>
            {
                paramProcessTransaction.PaymentMethod = "Invalid";
                var response = client.ProcessTransaction(paramProcessTransaction);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Invalid Payment Type' error", () =>
            {
                Assert.Contains("Invalid Payment Type", errorMessage.ToList());
            });
        }
    }
}
