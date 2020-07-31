# WW API Framework

## Summary:

1. Before test creation, we need to add test class and the following attributes:
```csharp
    [TestFixture]
    [Category("regression")] - TestRail category
    [Category("WhiteWings")]
```
 
2. Test class extends BaseTest class, BaseTest has the following methods:
```csharp
    void LogStep(string stepName, Action action)
    T LogStep<T>(string stepName, Func<T> action)
    T ConvertJsonToObject<T>(string fileName)
```

3. One test = one test method, for test method add following attributes:
```csharp
    [Test(Description = "Test name in jira")]
    [AllureTag("task id in jira", "task id in testrail, example: C5450912")]
    [TestRailCaseId(only id in test rail(example:5450912))]
```

4. For every step we need to use one of following methods: 
```csharp
   void LogStep(string stepName, Action action) - if step return nothing
   T LogStep<T>(string stepName, Func<T> action) - if need to return something value for use in different steps
```
5. Test data should be saved in json files in folder `TestData`
e.g. 'ParamValidateBitPesaAccount.json'

## Test Example:
```csharp
    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodValidateBitPesaAccount : BaseTest
    {
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
    }
```
