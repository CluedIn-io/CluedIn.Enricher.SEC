using System.Collections.Generic;
using CluedIn.Core.Crawling;

namespace CluedIn.ExternalSearch.Providers.SEC
{
    public class SECExternalSearchJobData : CrawlJobData
    {
        public SECExternalSearchJobData(IDictionary<string, object> configuration)
        {
           
        }

        public IDictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object> {
                
            };
        }
    }
}
