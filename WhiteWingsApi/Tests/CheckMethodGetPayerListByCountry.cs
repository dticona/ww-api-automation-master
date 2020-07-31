// <copyright file="CheckMethodGetPayerListByCountry.cs" company="IDT">
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
    public class CheckMethodGetPayerListByCountry : BaseTest
    {
        [Test(Description = "Verify if GetPayerListByCountry() response shows Payers by Country")]
        [AllureTag("qgin-4590", "C28995927")]
        [TestRailCaseId(28995927)]
        public void CheckGetPayerListByCountryResponseShowsPayersByCountry()
        {
            var payersByCountry = LogStep("1 - Run the `GetPayerListByCountry()` request on the soap API", () =>
            {
                var response = client.GetPayerListByCountry(CountryIdConstants.ColombiaId);
                var isResponseHaveErrors = response.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method GetCountryList have error: '{response.ErrorMessage}'");
                return response.Payers;
            });

            LogStep("2 - Check response shows Payers by Country", () =>
            {
                SoftAssert.IsTrue(payersByCountry.Length > 0,$"There are no payers returned in the response");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetPayerListByCountry() when DestinationCountryId field is Invalid")]
        [AllureTag("qgin-4591", "C28995928")]
        [TestRailCaseId(28995928)]
        public void CheckGetPayerListByCountryResponseWhenDestinationCountryIdInvalid()
        {
            var errorMessages = LogStep("1 - Run the `GetPayerListByCountry()` request on the soap API", () =>
            {
                var response = client.GetPayerListByCountry(CountryIdConstants.InvalidCountryId);
                return response.ErrorMessage;
            });

            LogStep("2 - Check an error message 'Invalid Country' is displayed in response", () =>
            {
                var expectedMessage = "Invalid Country";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetPayerListByCountry() when DestinationCountryId field is Empty")]
        [AllureTag("qgin-4592", "C28995929")]
        [TestRailCaseId(28995929)]
        public void CheckGetPayerListByCountryResponseWhenDestinationCountryIdFieldIsEmpty()
        {
            var errorMessages = LogStep("1 - Run the `GetPayerListByCountry()` request on the soap API", () =>
            {
                var response = client.GetPayerListByCountry(String.Empty);
                return response.ErrorMessage;
            });

            LogStep("2 - Check an error message 'Country Id is required.' is displayed in response", () =>
            {
                var expectedMessage = "Country Id is required.";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }

        [Test(Description = "Verify if an error message is displayed in GetPayerListByCountry() when DestinationCountryId field contain more than 3 characters")]
        [AllureTag("qgin-4593", "C28995930")]
        [TestRailCaseId(28995930)]
        public void CheckGetPayerListByCountryResponseWhenDestinationCountryIdFieldIsMoreThanThreeCharacters()
        {
            var errorMessages = LogStep("1 - Run the `GetPayerListByCountry()` request on the soap API", () =>
            {
                var response = client.GetPayerListByCountry(CountryIdConstants.MoreThanThreeCharCountryId);
                return response.ErrorMessage;
            });

            LogStep("2 - Check an error message 'Country id must be three character only' is displayed in response", () =>
            {
                var expectedMessage = "Country id must be three character only";
                Assert.Contains(expectedMessage, errorMessages.ToList(), $"'{expectedMessage}'error message is not returned in response!");
            });
        }
    }
}