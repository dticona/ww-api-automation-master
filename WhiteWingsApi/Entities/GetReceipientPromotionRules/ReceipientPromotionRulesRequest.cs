// <copyright file="ReceipientPromotionRulesRequest.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Entities.GetReceipientPromotionRules
{
    public class ReceipientPromotionRulesRequest
    {
        public string SourceCountryISOCode { get; set; }

        public string DestinationCountryISOCode { get; set; }

        public string AgentCode { get; set; }
    }
}
