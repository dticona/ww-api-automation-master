// <copyright file="ValidateTransferToAccountResults.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>
namespace WhiteWingsApi.TestData.Results
{
    public static class ValidateTransferToAccountResults
    {
        public static(string messageForPhone, string messageForSubPayerId, int code) ResponseHasEmptyFieldsResult()
        {
            var result =(messageForPhone: "Phone Number is required.", messageForSubPayerId: "Sub Payer Id is required.", code: 4);
            return result;
        }

        public static (string message, int code) ResponseHasInvalidSubPayerIdResult()
        {
            var result = (message: "Invalid SubPayer Id Number format.", code: 5);
            return result;
        }

        public static (string message, int code) ResponseHasSubPayerIdNotListedForMobileWalletResult()
        {
            var result = (message: "Invalid SubPayer Id", code: 6);
            return result;
        }
    }
}
