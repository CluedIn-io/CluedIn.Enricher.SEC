// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GleifOrganizationVocabulary.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the gleif organization vocabulary class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using CluedIn.Core.Data;
using CluedIn.Core.Data.Vocabularies;

namespace CluedIn.ExternalSearch.Providers.SEC.Vocabularies
{
    public class SECOrganizationVocabulary : SimpleVocabulary
    {        
        public SECOrganizationVocabulary()
        {
            this.VocabularyName = "SEC Organization";
            this.KeyPrefix      = "sec.organization";
            this.KeySeparator   = ".";
            this.Grouping       = CluedIn.Core.Data.EntityType.Organization;

            this.AddGroup("SEC Organization Details", group => 
            {
                this.FiscalYearEnd = group.Add(new VocabularyKey("fiscalYearEnd"));
                this.EntityType = group.Add(new VocabularyKey("entityType"));
                this.EIN = group.Add(new VocabularyKey("eIN"));
                this.CIK = group.Add(new VocabularyKey("cIK"));
                this.Description = group.Add(new VocabularyKey("description"));
                this.FormerNames = group.Add(new VocabularyKey("formerNames"));
                this.Flags = group.Add(new VocabularyKey("flags"));
                this.InsiderTransactionForIssuerExists = group.Add(new VocabularyKey("insiderTransactionForIssuerExists"));
                this.InsiderTransactionForOwnerExists = group.Add(new VocabularyKey("insiderTransactionForOwnerExists"));
                this.InvestorWebsite = group.Add(new VocabularyKey("investorWebsite"));
                this.Name = group.Add(new VocabularyKey("name"));
                this.Phone = group.Add(new VocabularyKey("phone"));
                this.SIC = group.Add(new VocabularyKey("sic"));
                this.SicDescription = group.Add(new VocabularyKey("sicDescription"));
                this.StateOfIncorporation = group.Add(new VocabularyKey("stateOfIncorporation"));
                this.StateOfIncorporationDescription = group.Add(new VocabularyKey("stateOfIncorporationDescription"));
                this.Tickers = group.Add(new VocabularyKey("tickers"));
                this.Website = group.Add(new VocabularyKey("website"));
                this.Category = group.Add(new VocabularyKey("category"));
            });

            //this.AddMapping(this.LeiCode,                       CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesLeiCode);
            //this.AddMapping(this.LegalName,                     CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.OrganizationName);
        }

        public VocabularyKey FiscalYearEnd { get; internal set; }
        public VocabularyKey EntityType { get; internal set; }
        public VocabularyKey EIN { get; internal set; }
        public VocabularyKey CIK { get; internal set; }
        public VocabularyKey Description { get; internal set; }
        public VocabularyKey FormerNames { get; internal set; }
        public VocabularyKey Flags { get; internal set; }
        public VocabularyKey InsiderTransactionForIssuerExists { get; internal set; }
        public VocabularyKey InsiderTransactionForOwnerExists { get; internal set; }
        public VocabularyKey InvestorWebsite { get; internal set; }
        public VocabularyKey Name { get; internal set; }
        public VocabularyKey Phone { get; internal set; }
        public VocabularyKey SIC { get; internal set; }
        public VocabularyKey SicDescription { get; internal set; }
        public VocabularyKey StateOfIncorporation { get; internal set; }
        public VocabularyKey StateOfIncorporationDescription { get; internal set; }
        public VocabularyKey Tickers { get; internal set; }
        public VocabularyKey Website { get; internal set; }
        public VocabularyKey Category { get; internal set; }
    }
}
