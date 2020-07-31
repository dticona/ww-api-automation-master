// <copyright file="GetPointOfContactListBySubPayerResults.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>
namespace WhiteWingsApi.TestData.Results
{
    public static class GetPointOfContactListBySubPayerResults
    {
        public static (string message, int code) ResponseHasInvalidSubPayerIdResult()
        {
            var result = (message: "Invalid SubPayer Id.", code: 3);
			return result;
		}
    }
}
