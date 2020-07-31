// <copyright file="WhiteWingsClient.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Framework.Api
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using System.Web.Services.Protocols;
    using System.Xml;
    using Entities.GetReceipientPromotionRules;
    using NUnit.Framework;
    using Utils;
    using WhiteWingsApi.Utils.Api;
    using WWAPI;

    /// <summary>
    /// This class handles the WhiteWings API Client.
    /// </summary>
    public class WhiteWingsClient : WWCDTC
    {
        private StreamWrapper requestStream;
        private static WhiteWingsClient instanceClient;

        /// <summary>
        /// Gets WhiteWingsClient.
        /// </summary>
        /// <returns>WhiteWingsClient instance.</returns>
        public static WhiteWingsClient InstanceClient()
        {
            if (instanceClient == null)
            {
                instanceClient = new WhiteWingsClient
                {
                    Url = Configs.WhiteWingsClientUrl
                };
            }

            return instanceClient;
        }

        /// <summary>
        /// Closes the conection to WhiteWings client.
        /// </summary>
        public static void CloseInstanceClient()
        {
            try
            {
                TestContext.WriteLine($"Closing current WWCDTC client...");
                instanceClient.Dispose();
            }
            catch (Exception error)
            {
                TestContext.WriteLine("Error when try close the WWCDTC Client " + error.Message);
            }
        }

        public PromotionRuleList GetReceipientPromotionRules(ReceipientPromotionRulesRequest receipientPromotionRulesRequest)
        {
            return GetReceipientPromotionRules(receipientPromotionRulesRequest.SourceCountryISOCode, receipientPromotionRulesRequest.DestinationCountryISOCode, receipientPromotionRulesRequest.AgentCode);
        }

        protected override XmlReader GetReaderForMessage(SoapClientMessage message, int bufferSize)
        {
            var responseXml = ReadStringFromStream(message.Stream);
            var requestXml = requestStream.StringValueForStream;
            AllureHelper.AddAtachmentXml($"Request for soap method: '{message.Action}'", requestXml);
            AllureHelper.AddAtachmentXml($"Response for soap method: '{message.Action}'", responseXml);
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(responseXml));
            UpdateStreamForSoapMessage(message, stream);
            return base.GetReaderForMessage(message, bufferSize);
        }

        protected override XmlWriter GetWriterForMessage(SoapClientMessage message, int bufferSize)
        {
            requestStream = new StreamWrapper(message.Stream);
            UpdateStreamForSoapMessage(message, requestStream);
            return base.GetWriterForMessage(message, bufferSize);
        }

        private void UpdateStreamForSoapMessage(SoapClientMessage message, Stream stream)
        {
            var field = typeof(SoapMessage).GetField("stream", BindingFlags.NonPublic | BindingFlags.Instance);
            field.SetValue(message, stream);
        }

        private string ReadStringFromStream(Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
