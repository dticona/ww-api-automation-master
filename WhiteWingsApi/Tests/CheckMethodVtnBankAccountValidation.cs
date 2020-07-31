// <copyright file="CheckMethodVtnBankAccountValidation.cs" company="IDT">
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
    public class CheckMethodVtnBankAccountValidation : BaseTest
    {
        private string accountOwnerNotFountMessage = "The account owner was not returned on the response";
        private string accountNumberNotMatch = "The accout number returned on the response is not the same than the request";
        private string bankCodeNotMatch = "The bank code returned on the response is not the same than the request";
        private string validateAccountStepOne = "1. Make a request to `VTNBankAccountValidation()` providing a valid bank account for {0}";
        private string validateAccontStepTwo = "2. Check response for method `VTNBankAccountValidation()` returns the account infromation  {0}";

        [Ignore("Ignore a fixture")]
        [Test(Description = "Verify if VTNBankAccountValidation() method response returns the name of the account owner for FIRST CITY MONUMENT BANK PLC Nigeria")]
        [AllureTag("MTSQ-4449", "C65070311", "@skipped")]
        [TestRailCaseId(65070311)]
        public void CheckValidateVtnAccountValidationFirtsCityMonumentBank()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "First City Monument Bank Plc"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.FirtsCityMonument;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidFirtsCityMonument;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "ORJI LORETTA IFEOMA"), () =>
            {
                Assert.AreEqual("ORJI LORETTA IFEOMA", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidFirtsCityMonument, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.FirtsCityMonument, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response returns the name of the account owner for United Bank For Africa Plc")]
        [AllureTag("MTSQ-4497", "C65581115")]
        [TestRailCaseId(65581115)]
        public void CheckValidateVtnAccountValidationUnitedBankForAfricaPlc()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "United Bank For Africa Plc"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.UnitedBankForAfrica;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidUnitedBankForAfricaAccount;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "GBADAMASI RASIDAT AYONI"), () =>
            {
                Assert.AreEqual("GBADAMASI RASIDAT AYONI", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidUnitedBankForAfricaAccount, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.UnitedBankForAfrica, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response returns the name of the account owner for ZENITH INTERNATIONAL BANK PLC")]
        [AllureTag("MTSQ-4498", "C65581116")]
        [TestRailCaseId(65581116)]
        public void CheckValidateVtnAccountValidationZENITHINTERNATIONALBANKPLC()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "Zenith International Bank Plc"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.ZenithInternationalBank;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidZenithInternationalAccount;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "PHILIS ELUE OGBUEBUNU"), () =>
            {
                Assert.AreEqual("PHILIS ELUE OGBUEBUNU", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidZenithInternationalAccount, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.ZenithInternationalBank, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response returns the name of the account owner for Access Bank Plc")]
        [AllureTag("MTSQ-4499", "C65581117")]
        [TestRailCaseId(65581117)]
        public void CheckValidateVtnAccountValidationAccessBankPlc()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "Access Bank Plc"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.AccessBank;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidAccessBank;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "VERONICA ASHAKA"), () =>
            {
                Assert.AreEqual("VERONICA ASHAKA", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidAccessBank, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.AccessBank, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response return the name of the account owner for Diamond Bank Plc")]
        [AllureTag("MTSQ-4500", "C65581118")]
        [TestRailCaseId(65581118)]
        public void CheckValidateVtnAccountValidationDiamondBankPlc()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "Diamond Bank Plc"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.DiamondBankPlc;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidDiamondBankPlcAccount;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "IHEAKA CHRISTIANA CHIKEZIE"), () =>
            {
                Assert.AreEqual("IHEAKA CHRISTIANA CHIKEZIE", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidDiamondBankPlcAccount, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.DiamondBankPlc, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response return the name of the account owner for Ecobank Nigeria Plc")]
        [AllureTag("MTSQ-4457", "C65081170")]
        [TestRailCaseId(65081170)]
        public void CheckValidateVtnAccountValidationEcobankNigeriaPlc()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "Ecobank Nigeria Plc"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.Ecobank;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidEcobankAccount;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "MOEMEKE LUCKY VICTOR"), () =>
            {
                Assert.AreEqual("MOEMEKE LUCKY VICTOR", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidEcobankAccount, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.Ecobank, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response return the name of the account owner for STANBIC IBTC BANK PLC")]
        [AllureTag("MTSQ-4456", "C65081169")]
        [TestRailCaseId(65081169)]
        public void CheckValidateVtnAccountValidationSTANBICIBTCBANKPLC()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "Stanbic Ibtc Bank Plc"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.StanbicIbtcBank;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidStanbicIbtcBank;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "KAFAYAT ORIADE MUSTAPHA"), () =>
            {
                Assert.AreEqual("KAFAYAT ORIADE MUSTAPHA", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidStanbicIbtcBank, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.StanbicIbtcBank, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response return the name of the account owner for Union Bank Of Nigeria Plc")]
        [AllureTag("MTSQ-4455", "C65081163")]
        [TestRailCaseId(65081163)]
        public void CheckValidateVtnAccountValidationUnionBankOfNigeriaPlc()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "Union Bank Of Nigeria Plc"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.UnionBankOfNigeria;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidUnionBankOfNigeriaAccount;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "FADIPE SUNDAY JAMES"), () =>
            {
                Assert.AreEqual("FADIPE SUNDAY JAMES", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidUnionBankOfNigeriaAccount, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.UnionBankOfNigeria, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Ignore("Ignore a fixture")]
        [Test(Description = "Verify if VTNBankAccountValidation() method response return the name of the account owner for Skye Bank Plc")]
        [AllureTag("MTSQ-4454", "C65070318", "@skipped")]
        [TestRailCaseId(65070318)]
        public void CheckValidateVtnAccountValidationSkyeBankPlc()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "Skye Bank Plc"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.SkyeBankPlc;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.SkyeBankPlc;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "DAMMY AJIBADE ADEBAYO"), () =>
            {
                Assert.AreEqual("DAMMY AJIBADE ADEBAYO", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.SkyeBankPlc, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.SkyeBankPlc, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response return the name of the account owner for KEYSTONE BANK PLC")]
        [AllureTag("MTSQ-4453", "C65070317")]
        [TestRailCaseId(65070317)]
        public void CheckValidateVtnAccountValidationKEYSTONEBANKPLC()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "Keystone Bank Plc"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.KeystoneBankPlc;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidKeystoneBankPlc;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "EVANS OGECHI AUGUSTA"), () =>
            {
                Assert.AreEqual("EVANS OGECHI AUGUSTA", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidKeystoneBankPlc, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.KeystoneBankPlc, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response returns the name of the account owner for Unity Bank Plc Nigeria")]
        [AllureTag("MTSQ-4452", "C65070316")]
        [TestRailCaseId(65070316)]
        public void CheckValidateVtnAccountValidationUnityBankPlcNigeria()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "Unity Bank Plc Nigeria"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.UnityBank;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidUnityBank;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "EMMANUEL OGBU"), () =>
            {
                Assert.AreEqual("EMMANUEL OGBU", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidUnityBank, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.UnityBank, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response returns the name of the account owner for Sterling Bank Plc Nigeria")]
        [AllureTag("MTSQ-4451", "C65070315")]
        [TestRailCaseId(65070315)]
        public void CheckValidateVtnAccountValidationSterlingBankPlcNigeria()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "SterlingBankPlcNigeria"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.SterlingBankPlc;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.SterlingBankPlc;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "EFELUDU OMOVUDU ANTHONY"), () =>
            {
                Assert.AreEqual("EFELUDU OMOVUDU ANTHONY", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.SterlingBankPlc, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.SterlingBankPlc, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response returns the name of the account owner for FIDELITY BANK PLC Nigeria")]
        [AllureTag("MTSQ-4450", "C65070312")]
        [TestRailCaseId(65070312)]
        public void CheckValidateVtnAccountValidationFIDELITYBANKPLCNigeria()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "Fidelity Bank Plc Nigeria"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.FidelityBank;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidFidelityBank;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "EKUE ANDREW IFALUYI"), () =>
            {
                Assert.AreEqual("EKUE ANDREW IFALUYI", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidFidelityBank, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.FidelityBank, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if VTNBankAccountValidation() method response returns the name of the account owner for First Bank Of Nigeria")]
        [AllureTag("MTSQ-4505", "C66008326")]
        [TestRailCaseId(66008326)]
        public void CheckValidateVtnAccountValidationFirstBankOfNigeria()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var vtnResponse = LogStep(string.Format(validateAccountStepOne, "First Bank Of Nigeria"), () =>
            {
                paramVtnAccountValidation.BankCode = NigeriaBankCodes.FirtsBankOfNigeria;
                paramVtnAccountValidation.BankAccountNumber = BankAccountsConstants.ValidFirtsBankOfNigeriaAccount;
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response;
            });

            LogStep(string.Format(validateAccontStepTwo, "AGUONIGHO RHODA"), () =>
            {
                Assert.AreEqual("AGUONIGHO RHODA", vtnResponse.BankAccountName, accountOwnerNotFountMessage);
                Assert.AreEqual(BankAccountsConstants.ValidFirtsBankOfNigeriaAccount, vtnResponse.BankAccountNumber, accountNumberNotMatch);
                Assert.AreEqual(NigeriaBankCodes.FirtsBankOfNigeria, vtnResponse.BankName, bankCodeNotMatch);
            });
        }

        [Test(Description = "Verify if an error message is displayed when an invalid Bank Code is used in VTNBankAccountValidation()")]
        [AllureTag("MTSQ-4506", "C66146742")]
        [TestRailCaseId(66146742)]
        public void CheckValidateVtnAccountValidationWithInvalidBankCode()
        {
            var paramVtnAccountValidation = ConvertJsonToObject<ParamVTNBankAccountValidation>("ParamVtnBankAccountValidation.json");

            var actualErrorMessages = LogStep("1 - Make a request to `VtnAccountValidation()` providing a invalid bank code", () =>
            {
                paramVtnAccountValidation.BankCode = "test";
                var response = client.VTNBankAccountValidation(paramVtnAccountValidation);
                return response.ErrorMessage;
            });

            LogStep($"2 - Check response for method `VtnAccountValidation()` have error message 'Invalid Bank Code'", () =>
            {
                Assert.Contains("Invalid Bank Code", actualErrorMessages, "The expected error message is not returned on the response");
            });
        }
    }
}
