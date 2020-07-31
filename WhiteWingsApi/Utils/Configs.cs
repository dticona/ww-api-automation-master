// <copyright file="Configs.cs" company="IDT">
// Copyright (c) IDT. All rights reserved.
// </copyright>

namespace WhiteWingsApi.Utils
{
    using System.Configuration;

    public static class Configs
    {
        public static readonly string WhiteWingsClientUrl = ConfigurationSettings.AppSettings["white_wings_client_url"];

        public static readonly string TestRailRunId = ConfigurationSettings.AppSettings["testrail_run_id"];

        public static readonly string TestrailUsername = ConfigurationSettings.AppSettings["testrail_username"];

        public static readonly string TestrailPassword = ConfigurationSettings.AppSettings["testrail_password"];

        public static readonly string TestrailUrl = ConfigurationSettings.AppSettings["testrail_url"];
    }
}
