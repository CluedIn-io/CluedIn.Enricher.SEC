using System;
using System.Collections.Generic;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.Providers;

namespace CluedIn.ExternalSearch.Providers.SEC
{
    public static class Constants
    {
        public const string ComponentName = "SEC";
        public const string ProviderName = "SEC";
        public static readonly Guid ProviderId = Guid.Parse("DDD32113-55C2-41E0-B180-4446AAB58B7F");

        public static string About { get; set; } = "The SEC protects investors, promotes fairness in the securities markets, and shares information about companies and investment professionals to help investors make informed decisions and invest with confidence.";
        public static string Icon { get; set; } = "Resources.sec-logo.png";
        public static string Domain { get; set; } = "https://www.sec.gov/";

        public static AuthMethods AuthMethods { get; set; } = new AuthMethods
        {
            token = new List<Control>()
        };

        public static IEnumerable<Control> Properties { get; set; } = new List<Control>()
        {
            // NOTE: Leaving this commented as an example - BF
            //new()
            //{
            //    displayName = "Some Data",
            //    type = "input",
            //    isRequired = true,
            //    name = "someData"
            //}
        };

        public static Guide Guide { get; set; } = null;
        public static IntegrationType IntegrationType { get; set; } = IntegrationType.Enrichment;
    }
}
