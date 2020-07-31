// <copyright file="CheckMethodGetPayersByBank.cs" company="IDT">
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

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodGetPayersByBank : BaseTest
    {
        [Test(Description = "Verify if GetPayersByBank() response shows Payers and subpayers by Bank")]
        [AllureTag("qgin-4594", "C28998068")]
        [TestRailCaseId(28998068)]
        public void CheckGetPayersByBankResponseShowsPayersAndSubPayersByBank()
        {
            var payers = LogStep("1 - Run the `GetPayersByBank()` request on the soap API", () =>
            {
                var response = client.GetPayersByBank(CountryIdConstants.MexicoId, BankNamesConstants.Bancoppel);
                return response.Payers;
            });

            LogStep("2 - Check response should display Payers by Bank", () =>
            {
                payers.ToList().ForEach(payer =>
                {
                    SoftAssert.IsTrue(payer.PayerId > 0,
                        $"Payer from bank with '{payer.BankId}' id was not returned in response");
                });
            });

            LogStep("3 - Check response should display SubPayers by Bank", () =>
            {
                payers.ToList().ForEach(payer =>
                {
                    payer.SubPayerId.ToList().ForEach(subPayer =>
                        SoftAssert.IsTrue(subPayer.Id > 0,
                            $"SubPayer of payer with '{payer.PayerId}' id was not returned in response"));
                });
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetPayersByBank() when CountryId field is Empty")]
        [AllureTag("qgin-4595", "C28998069")]
        [TestRailCaseId(28998069)]
        public void CheckErrorWhenGetPayersByBankResponseUsingEmptyCountriIdField()
        {
            var errorMessages = LogStep("1 - Run the `GetPayersByBank()` request on the soap API", () =>
            {
                var response = client.GetPayersByBank(String.Empty, BankNamesConstants.Bancoppel);
                return response.ErrorMessage;
            });

            LogStep("2 - The response should display following error message: Country Id is required.", () =>
            {
                var expectedMessage = "Country id is required.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetPayersByBank() when BankName field is Empty")]
        [AllureTag("qgin-4597", "C28998071")]
        [TestRailCaseId(28998071)]
        public void CheckErrorWhenGetPayersByBankResponseUsingEmptyBankNameField()
        {
            var errorMessages = LogStep("1 - Run the `GetPayersByBank()` request on the soap API", () =>
            {
                var response = client.GetPayersByBank(CountryIdConstants.MexicoId, String.Empty);
                return response.ErrorMessage;
            });

            LogStep("2 - The response should display following error message: Bank Name is required.", () =>
            {
                var expectedMessage = "Bank Name is required.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }
    }
}