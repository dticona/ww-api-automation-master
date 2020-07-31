// <copyright file="CheckMethodGetFeeFXDataWithPayer.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System;
    using System.Linq;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.TestData.Constants;
    using WhiteWingsApi.Utils.Asserts;
    using WhiteWingsApi.Utils.TestRail;
    using WhiteWingsApi.WWAPI;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodGetFeeFXDataWithPayer : BaseTest
    {
        private ParamFeeFxDataWithPayer paramFeeFxDataWithPayer;

        [SetUp]
        public void PrepareDataToRunGetFeeFXDataWithPayer()
        {
            paramFeeFxDataWithPayer = ConvertJsonToObject<ParamFeeFxDataWithPayer>("ParamFeeFxDataWithPayer.json");
        }

        [Test(Description = "Verify if GetFeeFXDataWithPayer() response shows fee of an Payer")]
        [AllureTag("qgin-4561", "C28586150")]
        [TestRailCaseId(28586150)]
        public void CheckGetFeeFxDataWithPayerShowsFeePayer()
        {
            var feesFXDataWithPayer = LogStep("1 - Run the `GetFeeFXDataWithPayer()` request on the soap API", () =>
            {
                var response = client.GetFeeFXDataWithPayer(paramFeeFxDataWithPayer);
                return response.FeesFxDataWithPayer;
            });

            LogStep("2 - Check The response should display correct fields", () =>
            {
                feesFXDataWithPayer.ToList().ForEach(fee =>
                {
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(fee.CustomerFee.ToString()),
                        $"Response has no AgentId field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(fee.PayerCommission.ToString()),
                        $"Response has no CustomerFXMax field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(fee.PurchaseFX.ToString()),
                        $"Response has no CustomerFXMin field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(fee.RetailFX.ToString()),
                        $"Response has no CustomerFeeMax field or it is empty/null");
                });
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetFeeFXDataWithPayer when DeliveryMethod field is empty")]
        [AllureTag("qgin-4562", "C28586151")]
        [TestRailCaseId(28586151)]
        public void CheckResponseGetFeeFxDataWithPayerHasErrorWhenDeliveryMethodFieldIsEmpty()
        {
            var errorMessages = LogStep("1 - Run the `GetFeeFXDataWithPayer()` request on the soap API", () =>
            {
                paramFeeFxDataWithPayer.DeliveryMethod = String.Empty;
                var response = client.GetFeeFXDataWithPayer(paramFeeFxDataWithPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Delivery method is required.' error message", () =>
            {
                var expectedMessage = "Delivery method is required.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetFeeFXDataWithPayer when DeliveryMethod field is Wrong")]
        [AllureTag("qgin-4564", "C28691048")]
        [TestRailCaseId(28691048)]
        public void CheckResponseGetFeeFxDataWithPayerHasErrorWhenDeliveryMethodFieldIsWrong()
        {
            var errorMessages = LogStep("1 - Run the `GetFeeFXDataWithPayer()` request on the soap API", () =>
            {
                paramFeeFxDataWithPayer.DeliveryMethod = DeliveryOptionConstants.InvalidDeliveryOption;
                var response = client.GetFeeFXDataWithPayer(paramFeeFxDataWithPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Invalid Delivery Method' error message", () =>
            {
                var expectedMessage = "Invalid Delivery Method";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetFeeFXDataWithPayer when RecipientCountry field is empty")]
        [AllureTag("qgin-4568", "C28691052")]
        [TestRailCaseId(28691052)]
        public void CheckResponseGetFeeFXDataWithPayerHasErrorWhenRecipientCountryFieldIsEmpty()
        {
            var errorMessages = LogStep("1 - Run the `GetFeeFXDataWithPayer()` request on the soap API", () =>
            {
                paramFeeFxDataWithPayer.RecipientCountry = String.Empty;
                var response = client.GetFeeFXDataWithPayer(paramFeeFxDataWithPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Recipient country is required.' error message", () =>
            {
                var expectedMessage = "Recipient country is required.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetFeeFXDataWithPayer when SendAmountCurrencyISOCode field is empty")]
        [AllureTag("qgin-4569", "C28691053")]
        [TestRailCaseId(28691053)]
        public void CheckResponseGetFeeFXDataWithPayerHasErrorWhenSendAmountCurrencyISOCodeFieldIsEmpty()
        {
            var errorMessages = LogStep("1 - Run the `GetFeesFXDataWithoutPayer()` request on the soap API", () =>
            {
                paramFeeFxDataWithPayer.SendAmountCurrencyISOCode = String.Empty;
                var response = client.GetFeeFXDataWithPayer(paramFeeFxDataWithPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Sender currency is required.' error message", () =>
            {
                var expectedMessage = "Sender currency is required.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetFeeFXDataWithPayer when a Invalid Agent ID is provided on the request")]
        [AllureTag("qgin-4563", "C28691047")]
        [TestRailCaseId(28586151)]
        public void CheckResponseGetFeeFxDataWhenAngetIdIsInvalid()
        {
            var errorMessages = LogStep("1 - Run the `GetFeeFXDataWithPayer()` request providing an invalid Agent ID", () =>
            {
                paramFeeFxDataWithPayer.AgentID = "InvalidAgendId";
                var response = client.GetFeeFXDataWithPayer(paramFeeFxDataWithPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Invalid Agent id.' error message", () =>
            {
                var expectedMessage = "Invalid Agent id.";
                Assert.Contains(expectedMessage, errorMessages, $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetFeeFXDataWithPayer when a Invalid Payer ID is provided on the request")]
        [AllureTag("qgin-4565", "C28691049")]
        [TestRailCaseId(28691049)]
        public void CheckResponseGetFeeFxDataWhenPayerIdIsInvalid()
        {
            var errorMessages = LogStep("1 - Run the `GetFeeFXDataWithPayer()` request providing an invalid Agent ID", () =>
            {
                paramFeeFxDataWithPayer.PayerId = 100;
                paramFeeFxDataWithPayer.SubPayerId = 0;
                var response = client.GetFeeFXDataWithPayer(paramFeeFxDataWithPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Invalid Payer ID.' error message", () =>
            {
                var expectedMessage = "Invalid Payer ID.";
                Assert.Contains(expectedMessage, errorMessages, $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetFeeFXDataWithPayer when a Invalid RecipientCountry on the request")]
        [AllureTag("qgin-4567", "C28691051")]
        [TestRailCaseId(28691051)]
        public void CheckResponseGetFeeFxDataWhenReceiverCountryIsInvalid()
        {
            var errorMessages = LogStep("1 - Run the `GetFeeFXDataWithPayer()` request providing an invalid Receiving Country", () =>
            {
                paramFeeFxDataWithPayer.RecipientCountry = "QWE";
                var response = client.GetFeeFXDataWithPayer(paramFeeFxDataWithPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Invalid recipient country.' error message", () =>
            {
                var expectedMessage = "Invalid recipient country.";
                Assert.Contains(expectedMessage, errorMessages, $"Response has no '{expectedMessage}' error message");
            });
        }
    }
}