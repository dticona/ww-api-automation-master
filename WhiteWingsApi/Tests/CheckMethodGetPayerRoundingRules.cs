// <copyright file="CheckMethodGetPayerRoundingRules.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System.Linq;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.TestData.Constants;
    using WhiteWingsApi.Utils.TestRail;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodGetPayerRoundingRules : BaseTest
    {
        private string destinationCountry = string.Empty;
        private long payerID = 0;
        private bool payerIDSpecified = false;
        private long subPayerID = 0;
        private bool subPayerIDSpecified = false;
        private string recipientCurrency = string.Empty;
        private string deliveryMethod = string.Empty;
        private long roundingMultiple = 0;
        private bool roundingMultipleSpecified = false;

        [Test(Description = "Verify that the GetPayerRoundingRules () does not display the 'Round Receiving Amount Nearest Multiple' rules if the PayerID is invalid.")]
        [AllureTag("qgin-4534", "C19355906")]
        [TestRailCaseId(19355906)]
        public void CheckErrorWhenGetPayerRoundingRulesResponseWithInvalidPayerIDField()
        {
            var errorMessages = LogStep("1 - Run the `GetPayerRoundingRules()` request on the soap API", () =>
            {
                payerID = PayerIdConstants.InvalidPayerID;
                payerIDSpecified = true;

                var response = client.GetPayerRoundingRules(
                  destinationCountry,
                  payerID,
                  payerIDSpecified,
                  subPayerID,
                  subPayerIDSpecified,
                  recipientCurrency,
                  deliveryMethod,
                  roundingMultiple,
                  roundingMultipleSpecified);
            return response.ErrorMessage;
            });

            LogStep("2 - The response should display following error message: Invalid payer ID.", () =>
            {
                Assert.Contains("Invalid payer ID.", errorMessages.ToList(), "'Invalid payer ID.'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify that the GetPayerRoundingRules () does not display the 'Round Receiving Amount Nearest Multiple' rules if the SubPayerID is invalid.")]
        [AllureTag("qgin-4535", "C19355905")]
        [TestRailCaseId(19355905)]
        public void CheckErrorWhenGetPayerRoundingRulesResponseWithInvalidSubPayerIDField()
        {
            var errorMessages = LogStep("1 - Run the `GetPayerRoundingRules()` request on the soap API", () =>
            {
                payerID = PayerIdConstants.ValidPayerID;
                payerIDSpecified = true;
                subPayerID = SubPayerIdConstants.InvalidSubPayerIdTypeLong;
                subPayerIDSpecified = true;

                var response = client.GetPayerRoundingRules(
                  destinationCountry,
                  payerID,
                  payerIDSpecified,
                  subPayerID,
                  subPayerIDSpecified,
                  recipientCurrency,
                  deliveryMethod,
                  roundingMultiple,
                  roundingMultipleSpecified);
                return response.ErrorMessage;
            });

            LogStep("2 - The response should display following error message: Invalid SubPayer payer ID.", () =>
            {
                Assert.Contains("Invalid SubPayer payer ID.", errorMessages.ToList(), "'Invalid SubPayer payer ID.'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if the GetPayerRoundingRules () does not display the response of 'Round Receiving Amount Nearest Multiple' rules when 2 delivery methods are entered.")]
        [AllureTag("qgin-4538", "C19355904")]
        [TestRailCaseId(19355904)]
        public void CheckErrorWhenGetPayerRoundingRulesResponseWithInvalidDeliveryMethodField()
        {
            var errorMessages = LogStep("1 - Run the `GetPayerRoundingRules()` request on the soap API", () =>
            {
                deliveryMethod = DeliveryMethodConstants.InvalidDeliveryMethod;

                var response = client.GetPayerRoundingRules(
                  destinationCountry,
                  payerID,
                  payerIDSpecified,
                  subPayerID,
                  subPayerIDSpecified,
                  recipientCurrency,
                  deliveryMethod,
                  roundingMultiple,
                  roundingMultipleSpecified);
                return response.ErrorMessage;
            });

            LogStep("2 - The response should display following error message: Invalid Delivery Method.", () =>
            {
                Assert.Contains("Invalid Delivery Method.", errorMessages.ToList(), "'Invalid Delivery Method.'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify that the GetPayerRoundingRules () does not display the 'Round Receiving Amount Nearest Multiple' rules if the Recipient Currency is invalid.")]
        [AllureTag("qgin-4541", "C19593575")]
        [TestRailCaseId(19593575)]
        public void CheckErrorWhenGetPayerRoundingRulesResponseWithInvalidRecipientCurrencyField()
        {
            var errorMessages = LogStep("1 - Run the `GetPayerRoundingRules()` request on the soap API", () =>
            {
                destinationCountry = CountryIdConstants.ColombiaId;
                recipientCurrency = RecipientCurrencyConstants.InvalidRecipientCurrency;
                deliveryMethod = DeliveryMethodConstants.HomeDelivery;

                var response = client.GetPayerRoundingRules(
                  destinationCountry,
                  payerID,
                  payerIDSpecified,
                  subPayerID,
                  subPayerIDSpecified,
                  recipientCurrency,
                  deliveryMethod,
                  roundingMultiple,
                  roundingMultipleSpecified);
                return response.ErrorMessage;
            });

            LogStep("2 - The response should display following error message: Invalid Recipient Currency.", () =>
            {
                Assert.Contains("Invalid Recipient Currency.", errorMessages.ToList(), "'Invalid Recipient Currency.'error message is not returned in response!");
            });
        }
    }
}