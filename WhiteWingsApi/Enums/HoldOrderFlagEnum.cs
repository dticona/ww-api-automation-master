// <copyright file="HoldOrderFlagEnum.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Enums
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public enum HoldOrderFlagEnum
    {
        ProcessingHold = 4,
        OperationHold = 1,
        UnPaid = 2,
        ComplianseHold = 3,
        ZeroHoldOrderFlag = 0
    }
}