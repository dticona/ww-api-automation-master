// <copyright file="CheckMethodGetPayerCommission.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
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
    public class CheckMethodGetPayerCommission : BaseTest
    {
        [Test(Description = "Verify if GetPayerCommission() response shows Commission of Payer by Country")]
        [AllureTag("qgin-4585", "C28991169")]
        [TestRailCaseId(28991169)]
        public void CheckGetPayerCommissionResponseShowsCommissionOfPayerByCountry()
        {
            var payerCommissions = LogStep("1 - Run the `GetPayerCommission()` request on the soap API", () =>
            {
                var response = client.GetPayerCommission(CountryIdConstants.ColombiaId, PayerIdConstants.ValidPayerID, true);
                var isResponseHaveErrors = response.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method GetCountryList have error: '{response.ErrorMessage}'");
                return response.PayerCommissions;
            });

            LogStep("2 - The response should display the Payer with Commission nodes with correct fields", () =>
            {
                payerCommissions.ToList().ForEach(payerCommission =>
                {
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.SubPayerId.ToString()),
                        $"Payer commission has no 'SubPayerId' field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.BackOfficeId),
                        $"Payer commission with '{payerCommission.SubPayerId}' subPayerId has no 'BackOfficeId' field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.DeliveryMethod),
                        $"Payer commission with '{payerCommission.SubPayerId}' subPayerId has no 'DeliveryMethod' field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.DestinationCountryId),
                        $"Payer commission with '{payerCommission.SubPayerId}' subPayerId has no 'DestinationCountryId' field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.DestinationCurrencyISOCode),
                        $"Payer commission with '{payerCommission.SubPayerId}' subPayerId has no 'DestinationCurrencyISOCode' field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.PayerFlatFee.ToString()),
                        $"Payer commission with '{payerCommission.SubPayerId}' subPayerId has no 'PayerFlatFee' field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.PayerId.ToString()),
                        $"Payer commission with '{payerCommission.SubPayerId}' subPayerId has no 'PayerId' field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.PayerPercentageFee.ToString()),
                        $"Payer commission with '{payerCommission.SubPayerId}' subPayerId has no 'PayerPercentageFee' field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.PayerPercentageFeeApplyOn),
                        $"Payer commission with '{payerCommission.SubPayerId}' subPayerId has no 'PayerPercentageFeeApplyOn' field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.SendAmountRangeBegin.ToString()),
                        $"Payer commission with '{payerCommission.SubPayerId}' subPayerId has no 'SendAmountRangeBegin' field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.SendAmountRangeCurrencyISOCode),
                        $"Payer commission with '{payerCommission.SubPayerId}' subPayerId has no 'SendAmountRangeCurrencyISOCode' field or it is empty/null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(payerCommission.SendAmountRangeEnd.ToString()),
                        $"Payer commission with '{payerCommission.SubPayerId}' subPayerId has no 'SendAmountRangeEnd' field or it is empty/null");
                });
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetPayerCommission() when CountryId field is Invalid")]
        [AllureTag("qgin-4586", "C28992139")]
        [TestRailCaseId(28992139)]
        public void CheckErrorWhenGetPayerCommissionResponseWithInvalidCountryIdField()
        {
            var errorMessages = LogStep("1 - Run the `GetPayerCommission()` request on the soap API", () =>
            {
                var response = client.GetPayerCommission(CountryIdConstants.InvalidCountryId, PayerIdConstants.ValidPayerID,
                    true);
                return response.ErrorMessage;
            });

            LogStep("2 - The response should display following error message: Invalid country ID", () =>
            {
                var expectedMessage = "Invalid country ID.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetPayerCommission() when CountryId contain more than 3 characters")]
        [AllureTag("qgin-4587", "C28993109")]
        [TestRailCaseId(28993109)]
        public void CheckErrorWhenGetPayerCommissionResponseWithCountryIdMoreThanThreeCharacters()
        {
            var errorMessages = LogStep("1 - Run the `GetPayerCommission()` request on the soap API", () =>
            {
                var response = client.GetPayerCommission(CountryIdConstants.MoreThanThreeCharCountryId, PayerIdConstants.ValidPayerID,
                    true);
                return response.ErrorMessage;
            });

            LogStep("2 - The response should display following error message: Country id must be three character only.", () =>
            {
                var expectedMessage = "Country id must be three character only.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetPayerCommission() when PayerId field is Invalid")]
        [AllureTag("qgin-4588", "C28993110")]
        [TestRailCaseId(28993110)]
        public void CheckErrorWhenGetPayerCommissionResponseWithInvalidPayerIdField()
        {
            var errorMessages = LogStep("1 - Run the `GetPayerCommission()` request on the soap API", () =>
            {
                var response = client.GetPayerCommission(CountryIdConstants.ColombiaId, PayerIdConstants.InvalidPayerID,true);
                return response.ErrorMessage;
            });

            LogStep("2 - The response should display following error message: Invalid payer ID.", () =>
            {
                var expectedMessage = "Invalid payer ID.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if a message is displayed in GetPayerCommission() when PayerId don't have any payer in the selected country")]
        [AllureTag("qgin-4589", "C28993111")]
        [TestRailCaseId(28993111)]
        public void CheckErrorWhenGetPayerCommissionResponseUsingPayerIdWithoutPayers()
        {
            var errorMessages = LogStep("1 - Run the `GetPayerCommission()` request on the soap API", () =>
            {
                var response = client.GetPayerCommission(CountryIdConstants.ColombiaId, PayerIdConstants.PayerIDWithoutPayersInColombia, true);
                return response.ErrorMessage;
            });

            LogStep("2 - The response should display following error message: No Payee fee commision found against COL.", () =>
            {
                var expectedMessage = "No Payee fee commision found against COL.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }
    }
}