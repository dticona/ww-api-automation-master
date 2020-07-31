// <copyright file="CheckMethodReleaseOrder.cs" company="IDT">
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
    using WhiteWingsApi.Utils.TestRail;
    using WhiteWingsApi.WWAPI;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodReleaseOrder : BaseTest
    {
        private ParamProcessTransaction paramProcessTransaction;

        [SetUp]
        public void CreateNewCpasIdAndPrepareDataToProcessTransaction()
        {
            var response = client.CreateNewCpasId();
            paramProcessTransaction = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
            paramProcessTransaction.CpasId = response.CPASId;
        }

        [Test(Description = "There should be possible to release a Transactions that has HoldOrderFlag=3 (ComplianceHold)")]
        [AllureTag("QGIN-3635", "C617574")]
        [TestRailCaseId(617574)]
        public void CheckReleaseOrderWithComplianceHoldStatus()
        {
            var transactionCpas = LogStep("1. Process a transaction that is on Compliance Hold Status", () =>
            {
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.ComplianseHold;
                var response = client.ProcessTransaction(paramProcessTransaction);
                var responseHasErrors = response.ErrorMessage != null;
                Assert.False(responseHasErrors, $"Api method ProcessTransaction have error: '{response.ErrorMessage}'");
                return response.CPASId;
            });

            var releaseOrderResponse = LogStep("2. Release the transaction using the ReleaseOrder method", () =>
            {
                return client.ReleaseOrder(transactionCpas, UserIdConstants.ValidUserID);
            });

            LogStep("3. Verify if the transaction staus is Unpaid and the message 'Released' on the response", () =>
            {
                Assert.AreEqual(0, releaseOrderResponse.ResultCode);
                Assert.AreEqual("UNPAID", releaseOrderResponse.OrderStatus);
                Assert.Contains("Released", releaseOrderResponse.ErrorMessage.ToList());
            });
        }

        [Test(Description = "There should be possible to release a Transactions that has HoldOrderFlag=1 (OperationalHold)")]
        [AllureTag("MTSQ-4401", "C63248010")]
        [TestRailCaseId(63248010)]
        public void CheckReleaseOrderWithOperationalHoldStatus()
        {
            var transactionCpas = LogStep("1. Process a transaction that is on Operational Hold Status", () =>
            {
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.OperationHold;
                var response = client.ProcessTransaction(paramProcessTransaction);
                var responseHasErrors = response.ErrorMessage != null;
                Assert.False(responseHasErrors, $"Api method ProcessTransaction have error: '{response.ErrorMessage}'");
                return response.CPASId;
            });

            var releaseOrderResponse = LogStep("2. Release the transaction using the ReleaseOrder method", () =>
            {
                return client.ReleaseOrder(transactionCpas, UserIdConstants.ValidUserID);
            });

            LogStep("3. Verify if the transaction staus is Unpaid and the message 'Released' on the response", () =>
            {
                Assert.AreEqual(0, releaseOrderResponse.ResultCode);
                Assert.AreEqual("UNPAID", releaseOrderResponse.OrderStatus);
                Assert.Contains("Released", releaseOrderResponse.ErrorMessage.ToList());
            });
        }


        [Test(Description = "There shouldn't be possible to release a Transactions that has HoldOrderFlag=4(Processing Hold)")]
        [AllureTag("MTSQ-4403", "C63248011")]
        [TestRailCaseId(63248011)]
        public void CheckReleaseOrderWithProcessingHoldStatus()
        {
            var transactionCpas = LogStep("1. Process a transaction that is on Processing Hold Status", () =>
            {
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.ProcessingHold;
                var response = client.ProcessTransaction(paramProcessTransaction);
                var responseHasErrors = response.ErrorMessage != null;
                Assert.False(responseHasErrors, $"Api method ProcessTransaction have error: '{response.ErrorMessage}'");
                return response.CPASId;
            });

            var releaseOrderResponse = LogStep("2. Release the transaction using the ReleaseOrder method", () =>
            {
                return client.ReleaseOrder(transactionCpas, UserIdConstants.ValidUserID);
            });

            LogStep("3. Verify if the transaction staus is Processing Hold and the message 'Release Order cannot be processed'", () =>
            {
                Assert.AreEqual(9, releaseOrderResponse.ResultCode);
                Assert.AreEqual("Processing Hold", releaseOrderResponse.OrderStatus);
                Assert.Contains("Release Order cannot be processed, Transaction is on Processing Hold.", releaseOrderResponse.ErrorMessage.ToList());
            });
        }

        [Test(Description = "There shouldn't be possible to release a Transactions that has been released")]
        [AllureTag("MTSQ-4404", "C63248012")]
        [TestRailCaseId(63248012)]
        public void CheckReleaseOrderWithAorderAlreadyReleased()
        {
            var transactionCpas = LogStep("1. Process a transaction that is on Operational Hold Status", () =>
            {
                paramProcessTransaction.HoldOrderFlag = (int)HoldOrderFlagEnum.OperationHold;
                var response = client.ProcessTransaction(paramProcessTransaction);
                var responseHasErrors = response.ErrorMessage != null;
                Assert.False(responseHasErrors, $"Api method ProcessTransaction have error: '{response.ErrorMessage}'");
                return response.CPASId;
            });

            var releaseOrderResponse = LogStep("2. Release the transaction twice using the ReleaseOrder method", () =>
            {
                client.ReleaseOrder(transactionCpas, UserIdConstants.ValidUserID);
                return client.ReleaseOrder(transactionCpas, UserIdConstants.ValidUserID);
            });

            LogStep("3. Verify if the transaction staus is Processing Hold and the message 'Order not on hold.'", () =>
            {
                Assert.AreEqual(3, releaseOrderResponse.ResultCode);
                Assert.AreEqual("UNPAID", releaseOrderResponse.OrderStatus);
                Assert.Contains("Order not on hold.", releaseOrderResponse.ErrorMessage.ToList());
            });
        }
    }
}
