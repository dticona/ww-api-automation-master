// <copyright file="CheckMethodValidateBitPesaAccount.cs" company="IDT">
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
    public class CheckMethodValidateBitPesaAccount : BaseTest
    {
        private string accountOwnerNotFountMessage = "The account owner was not returned on the response";
        private string validateAccountStepOne = "1. Make a request to `ValidateBitPesaAccount()` providing a valid bank account for {0}";
        private string validateAccontStepTwo = "2. Check response for method `ValidateBitPesaAccount()` returns the account owner {0}";

        [Test(Description = "Verify if process an ValidateBitPesaAccount() method with invalid Country Code, an error message is displayed.")]
        [AllureTag("QGIN-3939", "C5450912")]
        [TestRailCaseId(5450912)]
        public void CheckValidateBitPesaAccountWithInvalidCountryCode()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var actualErrorMessages = LogStep("1 - Run the `ValidateBitPesaAccount()` request on the soap API", () =>
            {
                var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                return response.ErrorMessage;
            });

            var expectedErrorMessage = "Country Code must be two character only";

            LogStep($"2 - Check response for method `ValidateBitPesaAccount()` have error message '{expectedErrorMessage}'", () =>
            {
                SoftAssert.IsTrue(actualErrorMessages.ToList().Contains(expectedErrorMessage), $"Error message incorrect for method ValidateBitPesaAccount()");
            });
        }

        [Test(Description = "Verify if an error message is displayed when an invalid Account Number is used")]
        [AllureTag("QGIN-3952", "C18083685")]
        [TestRailCaseId(18083685)]
        public void CheckValidateBitPesaAccountWithInvalidBankAccountNumber()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var actualErrorMessages = LogStep("1 - Make a request to `ValidateBitPesaAccount()` providing a invalid bank account", () =>
            {
                paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                return response.ErrorMessage;
            });

            LogStep($"2 - Check response for method `ValidateBitPesaAccount()` have error message 'Invalid Account'", () =>
            {
                Assert.Contains("Invalid Account", actualErrorMessages.ToList(), "The expected error message is not returned on the response");
            });
        }

        [Test(Description = "Verify if an error message is displayed when an invalid Bank Code is used ")]
        [AllureTag("QGIN-3951", "C18083684")]
        [TestRailCaseId(18083684)]
        public void CheckValidateBitPesaAccountWithInvalidBankCode()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var actualErrorMessages = LogStep("1 - Make a request to `ValidateBitPesaAccount()` providing a invalid bank code", () =>
            {
                paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                paramValidateBitPesaAccount.BankCode = "000";
                var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                return response.ErrorMessage;
            });

            LogStep($"2 - Check response for method `ValidateBitPesaAccount()` have error message 'Invalid Bank Code'", () =>
            {
                Assert.Contains("Invalid Bank Code", actualErrorMessages.ToList(), "The expected error message is not returned on the response");
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for First Bank of Nigeria ")]
        [AllureTag("QGIN-3953", "C18083687")]
        [TestRailCaseId(18083687)]
        public void CheckValidateBitPesaAccountFirtsBankOfNigeria()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Firts Bank of Nigeria"), () =>
             {

                 paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                 paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.FirtsBankOfNigeria;
                 paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidFirtsBankOfNigeriaAccount;
                 var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                 return response.ErrorMessage;
             });

            LogStep(string.Format(validateAccontStepTwo, "AGUONIGHO RHODA"), () =>
            {
                Assert.Contains("AGUONIGHO RHODA", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for Diamond Bank Plc - Nigeria ")]
        [AllureTag("MTSQ-4431", "C64889588")]
        [TestRailCaseId(64889588)]
        public void CheckValidateBitPesaAccountDiamontBankPlc()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "DiamontBank Plc"), () =>
             {
                 paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                 paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.DiamondBankPlc;
                 paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidDiamondBankPlcAccount;
                 var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                 return response.ErrorMessage;
             });

            LogStep(string.Format(validateAccountStepOne, "IHEAKA CHRISTIANA CHIKEZIE"), () =>
            {
                Assert.Contains("IHEAKA CHRISTIANA CHIKEZIE", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }


        [Test(Description = "Verify if ValidateBitPesaAccount() method response returns the name of the account owner for ZENITH INTERNATIONAL BANK PLC Nigeria")]
        [AllureTag("MTSQ-4439", "C64889586")]
        [TestRailCaseId(64889586)]
        public void CheckValidateBitPesaAccountZenithBank()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Zenith International Bank Plc"), () =>
             {
                 paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                 paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.ZenithInternationalBank;
                 paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidZenithInternationalAccount;
                 var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                 return response.ErrorMessage;
             });

            LogStep(string.Format(validateAccontStepTwo, "PHILIS ELUE OGBUEBUNU"), () =>
            {
                Assert.Contains("PHILIS ELUE OGBUEBUNU", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for Union Bank Of Nigeria Plc")]
        [AllureTag("MTSQ-4436", "C64889591")]
        [TestRailCaseId(64889591)]
        public void CheckValidateBitPesaAccountUnionBank()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Union Bank Of Nigeria Plc"), () =>
             {
                 paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                 paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.UnionBankOfNigeria;
                 paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidUnionBankOfNigeriaAccount;
                 var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                 return response.ErrorMessage;
             });

            LogStep(string.Format(validateAccontStepTwo, "FADIPE SUNDAY JAMES"), () =>
            {
                Assert.Contains("FADIPE SUNDAY JAMES", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for United Bank For Africa Plc")]
        [AllureTag("MTSQ-4437", "C64889585")]
        [TestRailCaseId(64889585)]
        public void CheckValidateBitPesaAccountUnitedBankForAfrica()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "United Bank For Africa Plc"), () =>
             {
                 paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                 paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.UnitedBankForAfrica;
                 paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidUnitedBankForAfricaAccount;
                 var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                 return response.ErrorMessage;
             });

            LogStep(string.Format(validateAccontStepTwo, "GBADAMASI RASIDAT AYONI"), () =>
            {
                Assert.Contains("GBADAMASI RASIDAT AYONI", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for Guaranty Trust Bank Plc")]
        [AllureTag("MTSQ-4440", "C64889584")]
        [TestRailCaseId(64889584)]
        public void CheckValidateBitPesaAccountGuarantyTrust()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Guaranty Trust Bank Plc"), () =>
             {
                 paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                 paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.GuarantyTrust;
                 paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidGuarantyTrustAccount;
                 var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                 return response.ErrorMessage;
             });

            LogStep(string.Format(validateAccontStepTwo, "ADIGUN OMOTAYO MUFTAU"), () =>
            {
                Assert.Contains("ADIGUN OMOTAYO MUFTAU", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for Ecobank Nigeria Plc")]
        [AllureTag("MTSQ-4442", "C64889589")]
        [TestRailCaseId(64889589)]
        public void CheckValidateBitPesaAccountEcoBankNigeria()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Ecobank Nigeria Plc"), () =>
             {
                 paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                 paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.Ecobank;
                 paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidEcobankAccount;
                 var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                 return response.ErrorMessage;
             });

            LogStep(string.Format(validateAccontStepTwo, "MOEMEKE LUCKY VICTOR"), () =>
            {
                Assert.Contains("MOEMEKE LUCKY VICTOR", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for Access Bank Plc")]
        [AllureTag("MTSQ-4443", "C64889587")]
        [TestRailCaseId(64889587)]
        public void CheckValidateBitPesaAccountAccessBankNigeria()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Access Bank Plc"), () =>
             {
                 paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                 paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.AccessBank;
                 paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidAccessBank;
                 var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                 return response.ErrorMessage;
             });

            LogStep(string.Format(validateAccontStepTwo, "VERONICA ASHAKA"), () =>
            {
                Assert.Contains("VERONICA ASHAKA", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response returns the name of the account owner for FIDELITY BANK PLC Nigeria")]
        [AllureTag("MTSQ-4432", "C65024064")]
        [TestRailCaseId(65024064)]
        public void CheckValidateBitPesaAccountFidelityBank()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Fidelity Bank"), () =>
            {
                paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.FidelityBank;
                paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidFidelityBank;
                var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                return response.ErrorMessage;
            });

            LogStep(string.Format(validateAccontStepTwo, "EKUE ANDREW IFALUYI"), () =>
            {
                Assert.Contains("EKUE ANDREW IFALUYI", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response returns the name of the account owner for FIRST CITY MONUMENT BANK PLC Nigeria")]
        [AllureTag("MTSQ-4433", "C65024063")]
        [TestRailCaseId(65024063)]
        public void CheckValidateBitPesaAccountFirtsCityMonumentBank()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Fidelity Bank"), () =>
            {
                paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.FirtsCityMonument;
                paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidFirtsCityMonument;
                var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                return response.ErrorMessage;
            });

            LogStep(string.Format(validateAccontStepTwo, "ORJI LORETTA IFEOMA"), () =>
            {
                Assert.Contains("ORJI LORETTA IFEOMA", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for STANBIC IBTC BANK PLC")]
        [AllureTag("MTSQ-4434", "C64889590")]
        [TestRailCaseId(64889590)]
        public void CheckValidateBitPesaAccountStanbicIbtcBank()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Fidelity Bank"), () =>
            {
                paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.StanbicIbtcBank;
                paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidStanbicIbtcBank;
                var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                return response.ErrorMessage;
            });

            LogStep(string.Format(validateAccontStepTwo, "KAFAYAT ORIADE MUSTAPHA"), () =>
            {
                Assert.Contains("KAFAYAT ORIADE MUSTAPHA", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for Unity Bank Plc")]
        [AllureTag("MTSQ-4438", "C64889595")]
        [TestRailCaseId(64889595)]
        public void CheckValidateBitPesaAccountUnityBank()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Unity Bank Plc"), () =>
            {
                paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.UnityBank;
                paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidUnityBank;
                var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                return response.ErrorMessage;
            });

            LogStep(string.Format(validateAccontStepTwo, "EMMANUEL OGBU"), () =>
            {
                Assert.Contains("EMMANUEL OGBU", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for Sterling Bank Plc")]
        [AllureTag("MTSQ-4435", "C64889597")]
        [TestRailCaseId(64889597)]
        public void CheckValidateBitPesaAccountSterlingBank()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, " Sterling Bank Plc"), () =>
            {
                paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.SterlingBankPlc;
                paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.SterlingBankPlc;
                var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                return response.ErrorMessage;
            });

            LogStep(string.Format(validateAccontStepTwo, "EFELUDU OMOVUDU ANTHONY"), () =>
            {
                Assert.Contains("EFELUDU OMOVUDU ANTHONY", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Ignore("Ignore a fixture")]
        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for Skye Bank Plc")]
        [AllureTag("MTSQ-4444", "C64889592","@skipped")]
        [TestRailCaseId(64889592)]
        public void CheckValidateBitPesaAccountSkyeBank()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Sterling Bank Plc"), () =>
            {
                paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.SkyeBankPlc;
                paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.SkyeBankPlc;
                var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                return response.ErrorMessage;
            });

            LogStep(string.Format(validateAccontStepTwo, "DAMMY AJIBADE ADEBAYO"), () =>
            {
                Assert.Contains("DAMMY AJIBADE ADEBAYO", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }

        [Test(Description = "Verify if ValidateBitPesaAccount() method response return the name of the account owner for KEYSTONE BANK PLC")]
        [AllureTag("MTSQ-4446", "C64889594")]
        [TestRailCaseId(64889594)]
        public void CheckValidateBitPesaAccountKeyStone()
        {
            var paramValidateBitPesaAccount = ConvertJsonToObject<ParamValidateBitPesaAccount>("ParamValidateBitPesaAccount.json");

            var accountHolder = LogStep(string.Format(validateAccountStepOne, "Keystone Bank Plc"), () =>
            {
                paramValidateBitPesaAccount.Country = CountriesIsoCodesTwo.Nigeria;
                paramValidateBitPesaAccount.BankCode = NigeriaBankCodes.KeystoneBankPlc;
                paramValidateBitPesaAccount.AccountNo = BankAccountsConstants.ValidKeystoneBankPlc;
                var response = client.ValidateBitPesaAccount(paramValidateBitPesaAccount);
                return response.ErrorMessage;
            });

            LogStep(string.Format(validateAccontStepTwo, "EVANS OGECHI AUGUSTA"), () =>
            {
                Assert.Contains("EVANS OGECHI AUGUSTA", accountHolder.ToList(), accountOwnerNotFountMessage);
            });
        }
    }
}
