// <copyright file="CheckMethodGetCountryList.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System.Linq;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.Utils.Asserts;
    using WhiteWingsApi.Utils.TestRail;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodGetCountryList : BaseTest
    {
        [Test(Description = "GetCountryList() API method should have a status field that shows the country status.")]
        [AllureTag("qgin-3636", "C9844347")]
        [TestRailCaseId(9844347)]
        public void CheckAllCountriesHasCountryStatusField()
        {
            var countries = LogStep("1 - Run the `GetCountryList()` request on the soap API", () =>
            {
                var response = client.GetCountryList();
                var isResponseHaveErrors = response.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method GetCountryList have error: '{response.ErrorMessage}'");
                return response.Countries;
            });

            LogStep("2 - Check all countries has CountryStatus field in response",() =>
            {
                countries.ToList().ForEach(country =>
                        SoftAssert.IsTrue(!string.IsNullOrEmpty(country.CountryStatus),
                            $"Country '{country.CountryID}' have not CountryStatus filed or it empty or null"));
            });
        }

        [Test(Description = "Verify if GetCountryList() response shows CountryId and CountryStatus field")]
        [AllureTag("qgin-4559", "C24698059")]
        [TestRailCaseId(24698059)]
        public void CheckAllCountriesHasCountryStatusFieldAndCountryId()
        {
            var countries = LogStep("1 - Run the `GetCountryList()` request on the soap API", () =>
            {
                var response = client.GetCountryList();
                var isResponseHaveErrors = response.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method GetCountryList have error: '{response.ErrorMessage}'");
                return response.Countries;
            });

            LogStep("2 - Check each country has CountryStatus and CountryId in response", () =>
            {
                countries.ToList().ForEach(country =>
                {
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(country.CountryStatus),
                        $"Country '{country.CountryStatus}' has no CountryStatus filed or it is empty or null");
                    SoftAssert.IsTrue(!string.IsNullOrEmpty(country.CountryID),
                        $"Country '{country.CountryID}' has no CountryId filed or it is empty or null");
                });
            });
        }

        [Test(Description = "Verify if it is possible get list of countries by GetCountryList() method.")]
        [AllureTag("qgin-4558", "C24681196")]
        [TestRailCaseId(24681196)]
        public void CheckPossibilityToGetCountryList()
        {
            var response = LogStep("1 - Run the `GetCountryList()` request on the soap API", () => client.GetCountryList());

            var expectedResultCode = 0;

            LogStep("2 - Check response has result code = 0", () =>
            {
                Assert.AreEqual(expectedResultCode, response.ResultCode, $"Result code of the response is {response.ResultCode} but expected was {expectedResultCode}");
            });

            var lengthOfEmptyList = 0;

            LogStep("3 - Check response shows the list of Countries", () =>
            {
                Assert.IsTrue(response.Countries.Length > lengthOfEmptyList, "The response does not show the list of Countries");
            });
        }
    }
}