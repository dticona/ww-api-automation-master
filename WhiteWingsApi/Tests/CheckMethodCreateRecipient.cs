// <copyright file="CheckMethodCreateRecipient.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System.Text.RegularExpressions;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.Utils;
    using WhiteWingsApi.Utils.Asserts;
    using WhiteWingsApi.Utils.TestRail;
    using WhiteWingsApi.WWAPI;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodCreateRecipient : BaseTest
    {
        private ParamCreateRecipient paramCreateRecipient;

        [OneTimeSetUp]
        public void PrepareDataToRunCreateRecipient()
        {
            paramCreateRecipient = ConvertJsonToObject<ParamCreateRecipient>("ParamCreateRecipient.json");
            paramCreateRecipient.RecipientFirstName = StringHelper.GenerateRandomString(5);
            paramCreateRecipient.RecipientLastName = StringHelper.GenerateRandomString(5);
        }

        [Test(Description = "Verify if is possible to create a Recipient throughout API")]
        [AllureTag("qgin-4556", "C10843007")]
        [TestRailCaseId(10843007)]
        public void CheckItIsPossibleCreateNewRecipient()
        {
            var receiverUid = LogStep("1 - Run the ` CreateRecipient()` request on the soap API", () =>
            {
                var response = client.CreateRecipient(paramCreateRecipient);
                var isResponseHaveErrors = response.ErrorMessage != null;
                Assert.That(!isResponseHaveErrors, $"Api method GetCountryList have error: '{response.ErrorMessage}'");
                return response.MTS_Receiver_UID;
            });

            LogStep("2 - Check response should return the MTS_Receiver_UID", () =>
            {
                SoftAssert.IsTrue(receiverUid > 0, $"Response have  MTS_Receiver_UID filed with {receiverUid} value");
            });
        }
    }
}
