// <copyright file="CheckMethodSetTransactionNotes.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Tests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Allure.Attributes;
    using NUnit.Framework;
    using WhiteWingsApi.Enums;
    using WhiteWingsApi.TestData.Constants;
    using WhiteWingsApi.Utils.Asserts;
    using WhiteWingsApi.Utils.TestRail;
    using WhiteWingsApi.WWAPI;

    [TestFixture]
    [Category("regression")]
    [Category("WhiteWings")]
    public class CheckMethodSetTransactionNotes : BaseTest
    {
        private ParamProcessTransaction transaction;
        private string noteToAdd = "This is a note added by the API Automation Framework";

        [SetUp]
        public void CreateNewCpasIdAndPrepareDataToProcessTransaction()
        {
            var cpas = client.CreateNewCpasId();
            transaction = ConvertJsonToObject<ParamProcessTransaction>("ParamProcessTransaction.json");
            transaction.CpasId = cpas.CPASId;
            transaction.HoldOrderFlag = (int)HoldOrderFlagEnum.OperationHold;
            client.ProcessTransaction(transaction);
        }

        [Test(Description = "Verify if a note is created using the SetTransactionNote() method.")]
        [AllureTag("MTSQ-4406", "C19611025")]
        [TestRailCaseId(19611025)]
        public void CheckSetTransactionNotesIfNoteIsAdded()
        {
            var note = LogStep("1. Make a request using SetTransactionNote() with the following values", () =>
            {
                ParamTransactionNote transactionNote = new ParamTransactionNote();
                transactionNote.CPASID = transaction.CpasId;
                transactionNote.AgencyCode = transaction.ComplianceAgentID;
                transactionNote.UserCode = transaction.UserID;
                transactionNote.Note = noteToAdd;
                var setTransactionTnote = client.SetTransactionNote(transactionNote);
                return setTransactionTnote;
            });

            LogStep("2. Verify if the response returns the Transaction Note ID", () =>
            {
                Assert.AreEqual(0, note.ResultCode);
                Assert.NotZero(note.TransactionNoteID);
            });
        }

        [Test(Description = "Verify that GetTransactionNotes() method response shows Details of Notes")]
        [AllureTag("MTSQ-4407", "C19694581")]
        [TestRailCaseId(19694581)]
        public void CheckGetTransactionNotesHasNoteDatails()
        {


            var note = LogStep("1. Make a request using SetTransactionNote() with the following values", () =>
            {
                ParamTransactionNote transactionNote = new ParamTransactionNote();
                transactionNote.CPASID = transaction.CpasId;
                transactionNote.AgencyCode = transaction.ComplianceAgentID;
                transactionNote.UserCode = transaction.UserID;
                transactionNote.Note = noteToAdd;
                return client.SetTransactionNote(transactionNote);
            });

            LogStep("2. Make a request to GetTransaction Notes and verify if the noted is returned on the response", () =>
            {
                var response = client.GetTransactionNotes(transaction.CpasId, string.Empty, string.Empty).ListTransactionNotes;
                List<string> notes = new List<string>();
                foreach (var node in response)
                {
                    notes.Add(node.Note);
                }
                Assert.Contains(noteToAdd, notes,"The expected message was not found on the response");
            });
        }
    }
}
