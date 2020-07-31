// <copyright file="CheckMethodCreateNewCpasId.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System.Text.RegularExpressions;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.Utils.TestRail;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodCreateNewCpasId : BaseTest
    {
        [Test(Description = "Verify that it's possible create a new CpasId by 'CreateNewCpasId()' method")]
        [AllureTag("qgin-4554", "C24681131")]
        [TestRailCaseId(24681131)]
        public void CheckItIsPossibleCreateNewCpasId()
        {
            var response = LogStep("1 - Run the `CreateNewCpasId()` request on the soap API", () => client.CreateNewCpasId());

            LogStep("2 - Check response shows New CpasId", () =>
            {
                Regex regex = new Regex(@"\d{12,}");
                Assert.IsTrue(regex.Match(response.CPASId.ToString()).Success, $"Response doesn't show correct cpasId");
            });

            LogStep("3 - Check response shows result code = 0", () =>
            {
                var expectedResultCode = 0;
                Assert.AreEqual(expectedResultCode, response.ResultCode, $"Actual result code value is {response.ResultCode}, but expected was {expectedResultCode}");
            });
        }

        [Test(Description = "Verify New CpasId on 'CreateNewCpasId()' response")]
        [AllureTag("qgin-4555", "C24681132")]
        [TestRailCaseId(24681132)]
        public void CheckNewCpasIdCreated()
        {
            var response = LogStep("1 - Run the `CreateNewCpasId()` request on the soap API", () => client.CreateNewCpasId());

            LogStep("2 - Check response shows New CpasId has 12 random digits", () =>
            {
                Regex regex = new Regex(@"\d{12,}");
                Assert.IsTrue(regex.Match(response.CPASId.ToString()).Success, $"Response doesn't show correct cpasId");
            });
        }
    }
}
