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
    public class CheckMethodCancelOrderRequest : BaseTest
    {
        [Test(Description = "Cancel transaction from New Retailer Portal by CancelOrderRequest method.")]
        [AllureTag("qgin-3928", "C1077424")]
        [TestRailCaseId(1077424)]
        public void CheckTransactionCanBeCancelledByCancelOrderRequest()
        {
            var cpasId = LogStep("1 - Run 'CreateNewCpasId()' request on the soap API", () =>
            {
                return client.CreateNewCpasId().CPASId;
            });

            var paramProcessTransaction = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");

            var orderStatus = LogStep("2 - Run the `ProcessTransaction()` request on the soap API", () =>
            {
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.UnPaid;
                paramProcessTransaction.CpasId = cpasId;
                var processTransactionResponse = client.ProcessTransaction(paramProcessTransaction);
                var isResponseHaveErrors = processTransactionResponse.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method ProcessTransaction have error: '{processTransactionResponse.ErrorMessage}'");
                return processTransactionResponse.OrderStatus;
            });

            var response = LogStep("3 - Run the `CancelOrderRequest()` request on the soap API", () =>
            {
                var cancelationReason = client.GetCancellationReasonsList().ListCancellationReasons.First();
                return client.CancelOrderRequest(cpasId, cancelationReason.ReasonID, true, cancelationReason.Description, cancelationReason.Reason, AgentIdConstants.ValidAgentID, UserIdConstants.ValidUserID);
            });

            LogStep("4 - Check 'Transaction is Cancelled Successfully' Error message is returned", () =>
            {
                var expectedErrorMessage = "  Transaction is Cancelled Successfully";
                var actualErrorsList = response.ErrorMessage.ToList();
                Assert.Contains(expectedErrorMessage, actualErrorsList);
            });
        }

        [Test(Description = "Verify if an error message is displayed in CancelOrderRequest method when UserID field is Invalid")]
        [AllureTag("qgin-4553", "C28699781")]
        [TestRailCaseId(28699781)]
        public void CheckCancelOrderRequestHasInvalidUserId()
        {
            var response = LogStep("1 - Run the `CancelOrderRequest()` on the soap API", () =>
            {
                var cancelationReason = client.GetCancellationReasonsList().ListCancellationReasons.First();
                return client.CancelOrderRequest(CPasIdConstants.ValidCpasId, cancelationReason.ReasonID, true, cancelationReason.Description, cancelationReason.Reason, AgentIdConstants.ValidAgentID, UserIdConstants.InvalidUserID);
            });

            LogStep("2 - Check 'Invalid UserID' Error message is returned", () =>
            {
                var expectedErrorMessage = "Invalid UserID";
                var actualErrorsList = response.ErrorMessage.ToList();
                SoftAssert.IsTrue(actualErrorsList.Contains(expectedErrorMessage), $"'{expectedErrorMessage}' doesn't exist in Actual Error messages");
            });
        }

        [Test(Description = "Verify if an error message is displayed in CancelOrderRequest method when AgentID field is Invalid")]
        [AllureTag("qgin-4552", "C28699780")]
        [TestRailCaseId(28699780)]
        public void CheckCancelOrderRequestHasInvalidAgentId()
        {
            var response = LogStep("1 - Run the `CancelOrderRequest()` on the soap API", () =>
            {
                var cancelationReason = client.GetCancellationReasonsList().ListCancellationReasons.First();
                return client.CancelOrderRequest(CPasIdConstants.ValidCpasId, cancelationReason.ReasonID, true, cancelationReason.Description, cancelationReason.Reason, AgentIdConstants.InvalidAgentID, UserIdConstants.ValidUserID);
            });

            LogStep("2 - Check 'Invalid AgentID' Error message is returned", () =>
            {
                var expectedErrorMessage = "Invalid AgentID";
                var actualErrorsList = response.ErrorMessage.ToList();
                SoftAssert.IsTrue(actualErrorsList.Contains(expectedErrorMessage), $"'{expectedErrorMessage}' doesn't exist in Actual Error messages");
            });
        }
    }
}
