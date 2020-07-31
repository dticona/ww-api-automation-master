// <copyright file="CheckMethodGetOrderStatus.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System;
    using System.Collections;
    using System.Linq;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.Enums;
    using WhiteWingsApi.TestData.Constants;
    using WhiteWingsApi.Utils.Asserts;
    using WhiteWingsApi.Utils.DataBase;
    using WhiteWingsApi.Utils.TestRail;
    using WhiteWingsApi.WWAPI;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodGetOrderStatus : BaseTest
    {
        [Test(Description = "Verify that GetOrderStatus returns status 'Expired' for Expired transactions")]
        [AllureTag("MTSQ-4799", "C78196163")]
        [TestRailCaseId(78196163)]
        public void CheckGetOrderStatusForExpiredTransaction()
        {
            var transaction = LogStep("1 - Process an UnPaid transaction.", () =>
            {
                var cpasId = client.CreateNewCpasId().CPASId;
                var transactionParameters = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
                transactionParameters.CpasId = cpasId;
                transactionParameters.HoldOrderFlag = 2;
                return client.ProcessTransaction(transactionParameters);
            });

            LogStep("2 - Change the transaction status to Expired", () =>
            {
                DataBaseConnection.ExecuteSqlQuery($"update Transactions set IsExpired=1 where SendRef = '{transaction.InvRef}'");
            });

            LogStep("3 - Verify if the GetOrderStatus() method return 'Expired' on CurrentOrderStatus field", () =>
            {
                var getOrderStatus = client.GetOrderStatus(transaction.CPASId);
                Assert.AreEqual("UNPAID", getOrderStatus.LastOrderStatus);
                Assert.AreEqual("Expired", getOrderStatus.CurrentOrderStatus);
            });
        }

        [Test(Description = "Verify that the GetOrderStatus() returns 'In Payer Review' status.")]
        [AllureTag("MTSQ-4863", "C84950222")]
        [TestRailCaseId(84950222)]
        public void CheckGetOrderStatusForInPayerReviewTransaction()
        {
            var transaction = LogStep("1 - Process an UnPaid transaction.", () =>
            {
                var cpasId = client.CreateNewCpasId().CPASId;
                var transactionParameters = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
                transactionParameters.CpasId = cpasId;
                transactionParameters.HoldOrderFlag = 2;
                return client.ProcessTransaction(transactionParameters);
            });

            LogStep("2 - Change the transaction status to In Payer Review", () =>
            {
                DataBaseConnection.ExecuteSqlQuery($"update Transactions set InPayerReview=1 where SendRef = '{transaction.InvRef}'");
            });

            LogStep("3 - Verify if the GetOrderStatus() method return 'Expired' on CurrentOrderStatus field", () =>
            {
                var getOrderStatus = client.GetOrderStatus(transaction.CPASId);
                Assert.AreEqual("UNPAID", getOrderStatus.LastOrderStatus);
                Assert.AreEqual("In Payer Review", getOrderStatus.CurrentOrderStatus);
            });
        }

        [Test(Description = "Verify GetOrderStatus() returns the status of an Operational Hold transaction")]
        [AllureTag("MTSQ-4864", "C66554148")]
        [TestRailCaseId(66554148)]
        public void CheckGetOrderStatusForOperationalHoldTransaction()
        {
            var transaction = LogStep("1 - Process an Operational Hold transaction.", () =>
            {
                var cpasId = client.CreateNewCpasId().CPASId;
                var transactionParameters = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
                transactionParameters.CpasId = cpasId;
                transactionParameters.HoldOrderFlag = 1;
                return client.ProcessTransaction(transactionParameters);
            });

            LogStep("2 - Verify if the GetOrderStatus() method return 'Operational Hold' on CurrentOrderStatus field", () =>
            {
                var getOrderStatus = client.GetOrderStatus(transaction.CPASId);
                Assert.AreEqual("Hold", getOrderStatus.LastOrderStatus);
                Assert.AreEqual("Operation Hold", getOrderStatus.CurrentOrderStatus);
            });
        }

        [Test(Description = "Verify GetOrderStatus() returns the status of an UnPaid transaction")]
        [AllureTag("MTSQ-4864", "C66554146 ")]
        [TestRailCaseId(66554146)]
        public void CheckGetOrderStatusForUnPaidTransaction()
        {
            var transaction = LogStep("1 - Process an Operational Hold transaction.", () =>
            {
                var cpasId = client.CreateNewCpasId().CPASId;
                var transactionParameters = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
                transactionParameters.CpasId = cpasId;
                transactionParameters.HoldOrderFlag = 1;
                return client.ProcessTransaction(transactionParameters);
            });

            LogStep("2 - Release the transation using the ReleaseOrder method", () =>
            {
                return client.ReleaseOrder(transaction.CPASId, UserIdConstants.ValidUserID);
            });

            LogStep("3 - Verify if the GetOrderStatus() method return 'Operational Hold' on CurrentOrderStatus field", () =>
            {
                var getOrderStatus = client.GetOrderStatus(transaction.CPASId);
                Assert.AreEqual("Hold", getOrderStatus.LastOrderStatus);
                Assert.AreEqual("UNPAID", getOrderStatus.CurrentOrderStatus);
            });
        }

        [Test(Description = "Verify GetOrderStatus() returns the status of a Void transaction")]
        [AllureTag("MTSQ-4864", "C66554147 ")]
        [TestRailCaseId(66554147)]
        public void CheckGetOrderStatusForVoidTransaction()
        {
            var transaction = LogStep("1 - Process an Operational Hold transaction.", () =>
            {
                var cpasId = client.CreateNewCpasId().CPASId;
                var transactionParameters = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
                transactionParameters.CpasId = cpasId;
                transactionParameters.HoldOrderFlag = 1;
                return client.ProcessTransaction(transactionParameters);
            });

            LogStep("2 - Cancel the transation using the CancelOrder method", () =>
            {
                var cancelationReason = client.GetCancellationReasonsList().ListCancellationReasons.First();
                return client.CancelOrderRequest(transaction.CPASId, cancelationReason.ReasonID, true, cancelationReason.Description, cancelationReason.Reason, AgentIdConstants.ValidAgentID, UserIdConstants.ValidUserID);
            });

            LogStep("3 - Verify if the GetOrderStatus() method return 'Operational Hold' on CurrentOrderStatus field", () =>
            {
                var getOrderStatus = client.GetOrderStatus(transaction.CPASId);
                Assert.AreEqual("Hold", getOrderStatus.LastOrderStatus);
                Assert.AreEqual("VOID", getOrderStatus.CurrentOrderStatus);
            });
        }

        [Ignore("Ignore a fixture")]
        [Test(Description = "Verify GetOrderStatus() returns the status of an Compliance Hold transaction")]
        [AllureTag("MTSQ-4864", "C66554149", "@skypped")]
        [TestRailCaseId(66554149)]
        public void CheckGetOrderStatusForComplianceHoldTransaction()
        {
            var transaction = LogStep("1 - Process a Compliance Hold transaction.", () =>
            {
                var cpasId = client.CreateNewCpasId().CPASId;
                var transactionParameters = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
                transactionParameters.CpasId = cpasId;
                transactionParameters.HoldOrderFlag = 3;
                return client.ProcessTransaction(transactionParameters);
            });

            LogStep("2 - Verify if the GetOrderStatus() method return 'Operational Hold' on CurrentOrderStatus field", () =>
            {
                var getOrderStatus = client.GetOrderStatus(transaction.CPASId);
                Assert.AreEqual("Hold", getOrderStatus.LastOrderStatus);
                Assert.AreEqual("Comliance Hold", getOrderStatus.CurrentOrderStatus);
            });
        }

        [Test(Description = "Verify that the GetOrderStatus() returns 'Cancel' status.")]
        [AllureTag("MTSQ-4864", "C86964400 ")]
        [TestRailCaseId(86964400)]
        public void CheckGetOrderStatusForCancelTransaction()
        {
            var transaction = LogStep("1 - Process an Operational Hold transaction.", () =>
            {
                var cpasId = client.CreateNewCpasId().CPASId;
                var transactionParameters = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
                transactionParameters.CpasId = cpasId;
                transactionParameters.HoldOrderFlag = 1;
                return client.ProcessTransaction(transactionParameters);
            });

            LogStep("2 - Release the transation using the ReleaseOrder method", () =>
            {
                return client.ReleaseOrder(transaction.CPASId, UserIdConstants.ValidUserID);
            });

            LogStep("3 - Mark the transaction as trasmitted", () =>
            {
                DataBaseConnection.ExecuteSqlQuery($"update Transactions set IsTransmitted=1 where SendRef = '{transaction.InvRef}'");
            });

            LogStep("4 - Cancel the transaction", () =>
            {
                DataBaseConnection.ExecuteSqlQuery($"update Transactions set CancelFlag=1 where SendRef = '{transaction.InvRef}'");
            });

            LogStep("5 - Verify if the GetOrderStatus() method return 'Operational Hold' on CurrentOrderStatus field", () =>
            {
                var getOrderStatus = client.GetOrderStatus(transaction.CPASId);
                Assert.AreEqual("UNPAID", getOrderStatus.LastOrderStatus);
                Assert.AreEqual("CANCELLED", getOrderStatus.CurrentOrderStatus);
            });
        }

        [Test(Description = "Verify that the GetOrderStatus() returns 'Paid' status.")]
        [AllureTag("MTSQ-4864", "C86964399")]
        [TestRailCaseId(86964399)]
        public void CheckGetOrderStatusForPaidTransaction()
        {
            var transaction = LogStep("1 - Process an Operational Hold transaction.", () =>
            {
                var cpasId = client.CreateNewCpasId().CPASId;
                var transactionParameters = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
                transactionParameters.CpasId = cpasId;
                transactionParameters.HoldOrderFlag = 1;
                return client.ProcessTransaction(transactionParameters);
            });

            LogStep("2 - Release the transation using the ReleaseOrder method", () =>
            {
                return client.ReleaseOrder(transaction.CPASId, UserIdConstants.ValidUserID);
            });

            LogStep("3 - Mark the transaction as Paid", () =>
            {
                DataBaseConnection.ExecuteSqlQuery($"update Transactions set IsTransmitted=1, Status=2 where SendRef = '{transaction.InvRef}'");
            });

            LogStep("4 - Verify if the GetOrderStatus() method return 'Operational Hold' on CurrentOrderStatus field", () =>
            {
                var getOrderStatus = client.GetOrderStatus(transaction.CPASId);
                Assert.AreEqual("UNPAID", getOrderStatus.LastOrderStatus);
                Assert.AreEqual("PAID", getOrderStatus.CurrentOrderStatus);
            });
        }
    }
}
