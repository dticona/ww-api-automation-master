// <copyright file="CheckMethodGetStateListByCountry.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System.Linq;
    using System.Text.RegularExpressions;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.Utils.Asserts;
    using WhiteWingsApi.Utils.TestRail;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodGetStateListByCountry : BaseTest
    {
        [Ignore("Ignore a fixture")]
        [Test(Description = "Verify that GetStateListByCountry() response shows CountryID  StateID, StateISOCode  StateName.")]
        [AllureTag("QGIN-4599", "C24714758","@skipped")]
        [TestRailCaseId(24714758)]
        public void CheckResponseShowsCountryIDStateIDStateISOCodeStateName()
        {
            var countryId = "COL";

            var response = LogStep("1 - Run the `GetStateListByCountry()` request on the soap API", () =>
            {
                return client.GetStateListByCountry(countryId);
            });

            LogStep($"2 - Check response should have CountryID, StateID, StateISOCode, StateName for each State", () =>
            {
                var errors = response.ErrorMessage;
                Assert.IsTrue(errors == null, $"Response have error messages");

                var states = response.States;
                var stateIdRegex = new Regex(@"^[0-9]{5}$");

                states.ToList().ForEach(state =>
                {
                    SoftAssert.AreEquals(countryId, state.CountryID, $"Incorrect CountryId for state with name: '{state.StateName}'");
                    SoftAssert.IsTrue(stateIdRegex.IsMatch($"{state.StateID}"), $"Incorrect StateID for state with name: '{state.StateName}'");
                    SoftAssert.IsFalse(string.IsNullOrEmpty(state.StateISOCode), $"StateISOCode empty or null for state with name: '{state.StateName}'");
                    SoftAssert.IsFalse(string.IsNullOrEmpty(state.StateName), $"StateName empty or null for state with state id: '{state.StateID}'");
                });
            });
        }
    }
}
