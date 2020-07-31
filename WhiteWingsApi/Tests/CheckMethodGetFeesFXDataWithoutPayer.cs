// <copyright file="CheckMethodGetFeesFXDataWithoutPayer.cs" company="IDT">
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
    public class CheckMethodGetFeesFXDataWithoutPayer : BaseTest
    {
        private ParamFeeFXDataWithoutPayer paramFeeFXDataWithoutPayer;

        [SetUp]
        public void PrepareDataToRunCreateRecipient()
        {
            paramFeeFXDataWithoutPayer = ConvertJsonToObject<ParamFeeFXDataWithoutPayer>("ParamFeeFXDataWithoutPayer.json");
        }

        [Test(Description = "Verify if GetFeesFXDataWithoutPayer shows fee")]
        [AllureTag("qgin-4571", "C28590397")]
        [TestRailCaseId(28590397)]
        public void CheckGetFeesFXDataWithoutPayerShowsFee()
        {
            var feesFXDataWithoutPayer = LogStep("1 - Run the `GetFeesFXDataWithoutPayer()` request on the soap API", () =>
            {
                var response = client.GetFeesFXDataWithoutPayer(paramFeeFXDataWithoutPayer);
                return response.FeesFXDataWithoutPayer;
            });

            LogStep("2 - Check The response should display correct fields", () =>
            {
                feesFXDataWithoutPayer.ToList().ForEach(fee =>
                {
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(fee.AgentId),
                        $"Response has no AgentId field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(fee.CustomerFXMax.ToString()),
                        $"Response has no CustomerFXMax field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(fee.CustomerFXMin.ToString()),
                        $"Response has no CustomerFXMin field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(fee.CustomerFeeMax.ToString()),
                        $"Response has no CustomerFeeMax field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(fee.CustomerFeeMin.ToString()),
                        $"Response has no CustomerFeeMin field or it is empty/null");
                });
            });
        }

        [Test(Description = "Verify if an error message is displayed on GetFeesFXDataWithoutPayer response when DeliveryOption field is empty")]
        [AllureTag("qgin-4572", "C28611413")]
        [TestRailCaseId(28611413)]
        public void CheckResponseGetFeesFXDataWithoutPayerHasErrorWhenDeliveryOptionFieldIsEmpty()
        {
            var errorMessages = LogStep("1 - Run the `GetFeesFXDataWithoutPayer()` request on the soap API", () =>
            {
                paramFeeFXDataWithoutPayer.DeliveryOption = String.Empty;
                var response = client.GetFeesFXDataWithoutPayer(paramFeeFXDataWithoutPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Delivery option is required.' error message", () =>
            {
                var expectedMessage = "Delivery option  is required.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed on GetFeesFXDataWithoutPayer response when AgentId field is empty")]
        [AllureTag("qgin-4573", "C28611414")]
        [TestRailCaseId(28611414)]
        public void CheckResponseGetFeesFXDataWithoutPayerHasErrorWhenAgentIdFieldIsEmpty()
        {
            var errorMessages = LogStep("1 - Run the `GetFeesFXDataWithoutPayer()` request on the soap API", () =>
            {
                paramFeeFXDataWithoutPayer.AgentId = String.Empty;
                var response = client.GetFeesFXDataWithoutPayer(paramFeeFXDataWithoutPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Agent id is required.' error message", () =>
            {
                var expectedMessage = "Agent id is required.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed on GetFeesFXDataWithoutPayer response when AgentId field is invalid")]
        [AllureTag("qgin-4574", "C28692876")]
        [TestRailCaseId(28692876)]
        public void CheckResponseGetFeesFXDataWithoutPayerHasErrorWhenAgentIdFieldIsInvalid()
        {
            var errorMessages = LogStep("1 - Run the `GetFeesFXDataWithoutPayer()` request on the soap API", () =>
            {
                paramFeeFXDataWithoutPayer.AgentId = AgentIdConstants.InvalidAgentID;
                var response = client.GetFeesFXDataWithoutPayer(paramFeeFXDataWithoutPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Invalid Agent id.' error message", () =>
            {
                var expectedMessage = "Invalid Agent id.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed on GetFeesFXDataWithoutPayer response when DeliveryOption field is invalid")]
        [AllureTag("qgin-4575", "C28693294")]
        [TestRailCaseId(28693294)]
        public void CheckResponseGetFeesFXDataWithoutPayerHasErrorWhenDeliveryOptionFieldIsInvalid()
        {
            var errorMessages = LogStep("1 - Run the `GetFeesFXDataWithoutPayer()` request on the soap API", () =>
            {
                paramFeeFXDataWithoutPayer.DeliveryOption = DeliveryOptionConstants.InvalidDeliveryOption;
                var response = client.GetFeesFXDataWithoutPayer(paramFeeFXDataWithoutPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Invalid delivery option.' error message", () =>
            {
                var expectedMessage = "Invalid delivery option.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed on GetFeesFXDataWithoutPayer response when RecipientCountry field is empty")]
        [AllureTag("qgin-4578", "C28693298")]
        [TestRailCaseId(28693298)]
        public void CheckResponseGetFeesFXDataWithoutPayerHasErrorWhenRecipientCountryFieldIsEmpty()
        {
            var errorMessages = LogStep("1 - Run the `GetFeesFXDataWithoutPayer()` request on the soap API", () =>
            {
                paramFeeFXDataWithoutPayer.RecipientCountry = String.Empty;
                var response = client.GetFeesFXDataWithoutPayer(paramFeeFXDataWithoutPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Recipient country id is required.' error message", () =>
            {
                var expectedMessage = "Recipient country id is required.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify if an error message is displayed on GetFeesFXDataWithoutPayer response when SendAmountCurrencyISOCode field is empty")]
        [AllureTag("qgin-4580", "C28695903")]
        [TestRailCaseId(28695903)]
        public void CheckResponseGetFeesFXDataWithoutPayerHasErrorWhenSendAmountCurrencyISOCodeIsEmpty()
        {
            var errorMessages = LogStep("1 - Run the `GetFeesFXDataWithoutPayer()` request on the soap API", () =>
            {
                paramFeeFXDataWithoutPayer.SendAmountCurrencyISOCode = String.Empty;
                var response = client.GetFeesFXDataWithoutPayer(paramFeeFXDataWithoutPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Send currency is required.' error message", () =>
            {
                var expectedMessage = "Send currency is required.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"Response has no '{expectedMessage}' error message");
            });
        }

        [Test(Description = "Verify that the error message is displayed on GetFeesFXDataWithoutPayer response when RecipientCountry is invalid")]
        [AllureTag("qgin-4579", "C28693716")]
        [TestRailCaseId(28693716)]
        public void CheckResponseGetFeesFXDataWithoutPayerWhenRecipientCountryIsInvalid()
        {
            var errorMessages = LogStep("1 - Run the `GetFeesFXDataWithoutPayer()` providing an Invalid Country on the Request", () =>
            {
                paramFeeFXDataWithoutPayer.RecipientCountry = "QWE";
                var response = client.GetFeesFXDataWithoutPayer(paramFeeFXDataWithoutPayer);
                return response.ErrorMessage;
            });

            LogStep("2 - Check that response has 'Invalid recipient country.' error message", () =>
            {
                var expectedMessage = "Invalid recipient country.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"Response has no '{expectedMessage}' error message");
            });
        }
    }
}