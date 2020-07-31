// <copyright file="UpdateHoldStatusResults.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>
namespace WhiteWingsApi.TestData.Results
{
    public static class UpdateHoldStatusResults
    {
        public static(string message, int code) StatusChangedToOpsHoldResult()
        {
            var result =(message: "Transactions status changed from Processing Hold to Ops Hold", code: 0);
            return result;
        }

        public static (string message, int code) StatusChangedToComplianceHold()
        {
            var result = (message: "Transactions status changed from Processing Hold to Compliance Hold", code: 0);
			return result;
		}
		
        public static (string message, int code) StatusChangedFromComplianceHoldToOpsHoldResult()
        {
            var result = (message: "Transactions status changed from Compliance Hold to Ops Hold", code: 0);
            return result;
        }

        public static(string message, int code) StatusIsInvalid()
        {
            var result =(message: "Invalid New Status", code: 1);
            return result;
        }

        public static(string message, int code) StatusTransactionIsNotOnCompliance()
        {
            var result =(message: "Transaction is not on Compliance/Processing Hold", code: 3);
            return result;
        }

        public static(string message, int code) StatusIsRequired()
        {
            var result =(message: "New Status is required.", code: 4);
            return result;
        }

        public static(string message, int code) StatusTransactionIsAlreadyOnCompliance()
        {
            var result =(message: "Transaction is already on Compliance Hold", code: 5);
            return result;
        }

        public static(string message, int code) CpasIsInvalid()
        {
            var result =(message: "Invalid CPAS ID", code: 6);
            return result;
        }

        public static (string message, int code) CpasIsRequired()
        {
            var result = (message: "CPAS ID is required.", code: 4);
            return result;
        }
    }
}
