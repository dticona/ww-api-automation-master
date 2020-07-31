// <copyright file="CheckMethodGetSubPayerListByPayer.cs" company="IDT">
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
    public class CheckMethodGetSubPayerListByPayer : BaseTest
    {
        [Test(Description = "Verify that subpayer status is returned as 'Disabled' in the API method 'GetSubPayerListByPayer' response when all POP's of the subpayer are disabled")]
        [AllureTag("qgin-3942", "C9844583")]
        [TestRailCaseId(9844583)]
        public void CheckSubPayerStatusIsReturnedAsDisabled()
        {
            var payerId = 167;
            var subPayers = LogStep("1 - Run the `GetSubPayerListByPayer()` request on the soap API", () =>
            {
                var response = client.GetSubPayerListByPayer(payerId, true);
                var isResponseHaveErrors = response.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method GetCountryList have error: '{response.ErrorMessage}'");
                return response.SubPayers;
            });

            LogStep("2 - Check The subPayer status is disabled in response", () =>
            {
                var expectedStatus = "Disabled";
                subPayers.ToList().ForEach(subPayer =>
                        SoftAssert.AreEquals(expectedStatus,subPayer.SubPayerStatus,
                            $"SubPayer with '{subPayer.CountryID}' ID has not 'Disabled' status"));
            });
        }
    }
}