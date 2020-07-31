// <copyright file="CheckMethodGetReceipientPromotionRules.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using Entities.GetReceipientPromotionRules;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using Utils.Asserts;
    using Utils.TestRail;
    using WWAPI;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodGetReceipientPromotionRules : BaseTest
    {
        [Test(Description = "Verify that the Result Code = 4 in GetReceipientPromotionRules when the agency doesn't have Promotion Rules")]
        [AllureTag("qgin-4603", "C29448242")]
        [TestRailCaseId(29448242)]
        public void CheckAgencyHaveNotPromotionRules()
        {
            var receipientPromotionRulesRequest = ConvertJsonToObject<ReceipientPromotionRulesRequest>("ReceipientPromotionRulesRequest.json");
            receipientPromotionRulesRequest.AgentCode = "WWMT07";
            var response = LogStep("1 - Run the `GetReceipientPromotionRules()` request on the soap API", () => client.GetReceipientPromotionRules(receipientPromotionRulesRequest));

            var expectedResultCode = 4;
            var expectedErrorMessage = "Rule does not exists.";

            LogStep($"2 - Check response shows result code = {expectedResultCode} and error message is '{expectedErrorMessage}'", () =>
            {
                SoftAssert.AreEquals(expectedResultCode, response.ResultCode, $"Incorrect result code");
                SoftAssert.AreEquals(expectedErrorMessage, response.ErrorMessage[0], $"Incorrect result code");
            });
        }

        [Test(Description = "Verify if GetReceipientPromotionRules() response shows detail information of PromotionRules")]
        [AllureTag("qgin-4602", "C29448241")]
        [TestRailCaseId(29448241)]
        public void CheckAgencyHavePromotionRules()
        {
            var receipientPromotionRulesRequest = ConvertJsonToObject<ReceipientPromotionRulesRequest>("ReceipientPromotionRulesRequest.json");
            receipientPromotionRulesRequest.AgentCode = "PRODTEST717";
            var response = LogStep("1 - Run the `GetReceipientPromotionRules()` request on the soap API", () => client.GetReceipientPromotionRules(receipientPromotionRulesRequest));

            var expectedPromotionRule = ConvertJsonToObject<PromotionRule>("PromotionRuleExpectedResponse.json");
            var actualPromotionRule = response.PromotionRules[0];

            LogStep($"2 - Check that response contains correct PromotionRules", () =>
            {
                EntityAssert.SoftAssertNotNullPropertyValues(expectedPromotionRule, actualPromotionRule, "Incorrect expected PromotionRules.");
            });
        }
    }
}
