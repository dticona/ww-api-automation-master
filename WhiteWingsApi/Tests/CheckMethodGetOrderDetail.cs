// <copyright file="CheckMethodGetOrderDetail.cs" company="IDT">
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
    using WhiteWingsApi.Utils.Asserts;
    using WhiteWingsApi.Utils.DataBase;
    using WhiteWingsApi.Utils.TestRail;
    using WhiteWingsApi.WWAPI;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodGetOrderDetail : BaseTest
    {
        private OutputParamOrderDetail orderDetails;

        [Test(Description = "Verify if an error message when an invalid CPAS is provided ")]
        [AllureTag("QGIN-4583", "C28819029")]
        [TestRailCaseId(28819029)]
        public void CheckGetOrderDetailInvalidCpas()
        {
            var orderDetailsError = LogStep("1 - Run the `GetOrderDetail()` request providing an invalid Cpas", () =>
            {
                var response = client.GetOrderDetail("999999999999");
                return response.ErrorMessage;
            });

            LogStep("2 - Check that the error message response contains Invalid CPAS ID.", () =>
            {
                var expectedErrorMessage = "Invalid CPAS ID.";
                Assert.AreEqual(expectedErrorMessage, orderDetailsError, "Unexpected error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed on GetOrderDetail when CpasId field is empty")]
        [AllureTag("QGIN-4582", "C28819028")]
        [TestRailCaseId(28819028)]
        public void CheckGetOrderDetailWithEmptyCpasId()
        {
            var orderDetailsError = LogStep("1 - Run the `GetOrderDetail()` request providing empty Cpas", () =>
            {
                var response = client.GetOrderDetail(String.Empty);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that the error message response contains CPAS Id is required.", () =>
            {
                var expectedErrorMessage = "CPAS Id is required.";
                Assert.AreEqual(expectedErrorMessage, orderDetailsError, "Expected error message '{expectedErrorMessage}' was not returned in response");
            });
        }

        [Test(Description = "Verify that GetOrderDetail returns status Expired for Expired transactions")]
        [AllureTag("MTSQ-4799", "C78196162")]
        [TestRailCaseId(78196162)]
        public void CheckGetOrderDetailForExpiredTransaction()
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

            LogStep("3 - Verify if the GetOrderDetails() method return 'Expired' on OrderStatus field", () =>
            {
                var getOrderDetails = client.GetOrderDetail(transaction.CPASId);
                Assert.AreEqual("Expired", getOrderDetails.OrderStatus);
            });
        }

        [Test(Description = "Verify that the GetOrderDetail() returns 'In Payer Review' status.")]
        [AllureTag("MTSQ-4863", "C84950223")]
        [TestRailCaseId(84950223)]
        public void CheckGetOrderDetailForInPayerReviewTransaction()
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

            LogStep("3 - Verify if the GetOrderDetails() method return 'Expired' on OrderStatus field", () =>
            {
                var getOrderDetails = client.GetOrderDetail(transaction.CPASId);
                Assert.AreEqual("In Payer Review", getOrderDetails.OrderStatus);
            });
        }

        [Test(Description = "Verify GetOrderDetail() returns the status of an UnPaid transaction")]
        [AllureTag("MTSQ-4865", "C86964404")]
        [TestRailCaseId(86964404)]
        public void CheckGetOrderDetailForUnPaidTransaction()
        {
            var transaction = LogStep("1 - Process an Operational Hold transaction.", () =>
            {
                var cpasId = client.CreateNewCpasId().CPASId;
                var transactionParameters = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
                transactionParameters.CpasId = cpasId;
                transactionParameters.HoldOrderFlag = 2;
                return client.ProcessTransaction(transactionParameters);
            });

            LogStep("2 - Release the transation using the ReleaseOrder method", () =>
            {
                return client.ReleaseOrder(transaction.CPASId, UserIdConstants.ValidUserID);
            });

            LogStep("3 - Verify if the GetOrderDetail() method return 'UnPaid' on OrderStatus field", () =>
            {
                var getOrderDetail = client.GetOrderDetail(transaction.CPASId);
                Assert.AreEqual("UNPAID", getOrderDetail.OrderStatus);
            });
        }

        [Test(Description = "Verify GetOrderDetail() returns the status of a Void transaction")]
        [AllureTag("MTSQ-4865", "C86964403")]
        [TestRailCaseId(86964403)]
        public void CheckGetOrderDetailForVoidTransaction()
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

            LogStep("3 - Verify if the GetOrderDetail() method return 'Void' on OrderStatus field", () =>
            {
                var getOrderDetail = client.GetOrderDetail(transaction.CPASId);
                Assert.AreEqual("VOID", getOrderDetail.OrderStatus);
            });
        }

        [Test(Description = "Verify GetOrderDetail() returns the status of an Operational Hold transaction")]
        [AllureTag("MTSQ-4865", "C86964405")]
        [TestRailCaseId(86964405)]
        public void CheckGetOrderDetailForOperationalHoldTransaction()
        {
            var transaction = LogStep("1 - Process an Operational Hold transaction.", () =>
            {
                var cpasId = client.CreateNewCpasId().CPASId;
                var transactionParameters = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
                transactionParameters.CpasId = cpasId;
                transactionParameters.HoldOrderFlag = 1;
                return client.ProcessTransaction(transactionParameters);
            });

            LogStep("2 - Verify if the GetOrderStatus() method return 'Operational Hold' on OrderStatus field", () =>
            {
                var getOrderDetail = client.GetOrderDetail(transaction.CPASId);
                Assert.AreEqual("Operation Hold", getOrderDetail.OrderStatus);
            });
        }

        [Ignore("Ignore a fixture")]
        [Test(Description = "Verify GetOrderDetail() returns the status of an Compliance Hold transaction")]
        [AllureTag("MTSQ-4865", "C86964406", "@skypped")]
        [TestRailCaseId(86964406)]
        public void CheckGetOrderDetailForComplianceHoldTransaction()
        {
            var transaction = LogStep("1 - Process a Compliance Hold transaction.", () =>
            {
                var cpasId = client.CreateNewCpasId().CPASId;
                var transactionParameters = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
                transactionParameters.CpasId = cpasId;
                transactionParameters.HoldOrderFlag = 3;
                return client.ProcessTransaction(transactionParameters);
            });

            LogStep("2 - Verify if the GetOrderStatus() method return 'Operational Hold' on OrderStatus field", () =>
            {
                var getOrderDetail = client.GetOrderDetail(transaction.CPASId);
                Assert.AreEqual("Compliance Hold", getOrderDetail.OrderStatus);
            });
        }

        [Test(Description = "Verify that the GetOrderDetail() returns 'Paid' status.")]
        [AllureTag("MTSQ-4865", "C86964402")]
        [TestRailCaseId(86964402)]
        public void CheckGetOrderDetailForPaidTransaction()
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

            LogStep("4 - Verify if the GetOrderDetail() method return 'PAID' on OrderStatus field", () =>
            {
                var getOrderDetail = client.GetOrderDetail(transaction.CPASId);
                Assert.AreEqual("PAID", getOrderDetail.OrderStatus);
            });
        }

        [Test(Description = "Verify that the GetOrderDetail() returns 'Cancelled' status.")]
        [AllureTag("MTSQ-4865", "C86969635")]
        [TestRailCaseId(86969635)]
        public void CheckGetOrderDetailForCancelTransaction()
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

            LogStep("5 - Verify if the GetOrderDetail() method return 'Operational Hold' on OrderStatus field", () =>
            {
                var getOrderDetail = client.GetOrderDetail(transaction.CPASId);
                Assert.AreEqual("CANCELLED", getOrderDetail.OrderStatus);
            });
        }
    }
}