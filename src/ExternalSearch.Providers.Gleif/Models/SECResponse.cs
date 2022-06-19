// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GleifResponse.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the gleif response class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace CluedIn.ExternalSearch.Providers.SEC.Models
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Addresses
    {
        public Mailing mailing { get; set; }
        public Business business { get; set; }
    }

    public class Business
    {
        public string street1 { get; set; }
        public string street2 { get; set; }
        public string city { get; set; }
        public string stateOrCountry { get; set; }
        public string zipCode { get; set; }
        public string stateOrCountryDescription { get; set; }
    }

    public class File
    {
        public string name { get; set; }
        public int filingCount { get; set; }
        public string filingFrom { get; set; }
        public string filingTo { get; set; }
    }

    public class Filings
    {
        public Recent recent { get; set; }
        public List<File> files { get; set; }
    }

    public class Mailing
    {
        public string street1 { get; set; }
        public object street2 { get; set; }
        public string city { get; set; }
        public string stateOrCountry { get; set; }
        public string zipCode { get; set; }
        public string stateOrCountryDescription { get; set; }
    }

    public class Recent
    {
        public List<string> accessionNumber { get; set; }
        public List<string> filingDate { get; set; }
        public List<string> reportDate { get; set; }
        public List<DateTime> acceptanceDateTime { get; set; }
        public List<string> act { get; set; }
        public List<string> form { get; set; }
        public List<string> fileNumber { get; set; }
        public List<string> filmNumber { get; set; }
        public List<string> items { get; set; }
        public List<int> size { get; set; }
        public List<int> isXBRL { get; set; }
        public List<int> isInlineXBRL { get; set; }
        public List<string> primaryDocument { get; set; }
        public List<string> primaryDocDescription { get; set; }
    }

    public class SECResponse
    {
        public string cik { get; set; }
        public string entityType { get; set; }
        public string sic { get; set; }
        public string sicDescription { get; set; }
        public int insiderTransactionForOwnerExists { get; set; }
        public int insiderTransactionForIssuerExists { get; set; }
        public string name { get; set; }
        public List<string> tickers { get; set; }
        public List<string> exchanges { get; set; }
        public string ein { get; set; }
        public string description { get; set; }
        public string website { get; set; }
        public string investorWebsite { get; set; }
        public string category { get; set; }
        public string fiscalYearEnd { get; set; }
        public string stateOfIncorporation { get; set; }
        public string stateOfIncorporationDescription { get; set; }
        public Addresses addresses { get; set; }
        public string phone { get; set; }
        public string flags { get; set; }
        public List<object> formerNames { get; set; }
        public Filings filings { get; set; }
    }


}
