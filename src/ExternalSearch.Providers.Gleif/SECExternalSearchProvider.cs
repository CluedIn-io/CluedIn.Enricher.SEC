// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GleifExternalSearchProvider.cs" company="Clued In">
//   Copyright (c) 2018 Clued In. All rights reserved.
// </copyright>
// <summary>
//   Implements the gleif external search provider class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using CluedIn.Core;
using CluedIn.Core.Data;
using CluedIn.Core.Data.Parts;
using CluedIn.Core.Data.Relational;
using CluedIn.Core.ExternalSearch;
using CluedIn.Core.Providers;
using RestSharp;
using Newtonsoft.Json;
using EntityType = CluedIn.Core.Data.EntityType;
using CluedIn.ExternalSearch.Providers.SEC.Models;
using CluedIn.ExternalSearch.Providers.SEC.Vocabularies;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace CluedIn.ExternalSearch.Providers.SEC
{
    /// <summary>The gleif graph external search provider.</summary>
    /// <seealso cref="CluedIn.ExternalSearch.ExternalSearchProviderBase" />
    public class SECExternalSearchProvider : ExternalSearchProviderBase, IExtendedEnricherMetadata, IConfigurableExternalSearchProvider
    {
        private static readonly EntityType[] AcceptedEntityTypes = { "/Vendor" };

        /**********************************************************************************************************
         * CONSTRUCTORS
         **********************************************************************************************************/

        public SECExternalSearchProvider()
            : base(Constants.ProviderId, AcceptedEntityTypes)
        {
        }

        /**********************************************************************************************************
         * METHODS
         **********************************************************************************************************/

        /// <inheritdoc/>
        public override IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request)
        {
            if (!this.Accepts(request.EntityMetaData.EntityType))
                yield break;

            var entityType       = request.EntityMetaData.EntityType;
            var ticker         = request.QueryParameters.GetValue(CluedIn.Core.Data.Vocabularies.Vocabularies.CluedInOrganization.CodesCIK, new HashSet<string>());

            if (ticker != null && ticker.Any())
            {
                foreach (var value in ticker)
                    yield return new ExternalSearchQuery(this, entityType, ExternalSearchQueryParameter.Identifier, value);
            }
        }

        /// <inheritdoc/>
        public override IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query)
        {
            var ticker = query.QueryParameters[ExternalSearchQueryParameter.Identifier].FirstOrDefault();

            if (string.IsNullOrEmpty(ticker))
                yield break;
           
            var client = new RestClient("https://data.sec.gov");

            var request = new RestRequest(String.Format("submissions/CIK{0}.json", ticker), Method.GET);
            request.AddHeader("User-Agent", "CluedIn contact@cluedin.com");
            request.AddHeader("Host", "www.cluedin.com");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("Accept-Encoding", "gzip, deflate");

            ServicePointManager.ServerCertificateValidationCallback =
      delegate (
          object s,
          X509Certificate certificate,
          X509Chain chain,
          SslPolicyErrors sslPolicyErrors
      ) {
          return true;
      };

            var response = client.ExecuteAsync(request).Result;

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var responseData = response.Content; //.Substring(1, response.Content.Length - 2);

                var data = JsonConvert.DeserializeObject<SECResponse>(responseData);

                if (data != null)
                    yield return new ExternalSearchQueryResult<SECResponse>(query, data);
            }
            else if (response.StatusCode == HttpStatusCode.NoContent || response.StatusCode == HttpStatusCode.NotFound)
                yield break;
            else if (response.ErrorException != null)
                throw new AggregateException(response.ErrorException.Message, response.ErrorException);
            else
                throw new ApplicationException("Could not execute external search query - StatusCode:" + response.StatusCode + "; Content: " + response.Content);
        }

        /// <inheritdoc/>
        public override IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<SECResponse>();

            var code = this.GetOriginEntityCode(resultItem.Data.cik);

            var clue = new Clue(code, context.Organization);

            this.PopulateMetadata(clue.Data.EntityData, resultItem, request);

            return new[] { clue };
        }

        /// <inheritdoc/>
        public override IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            var resultItem = result.As<SECResponse>();
            return this.CreateMetadata(resultItem, request);
        }

        /// <inheritdoc/>
        public override IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request)
        {
            return null;
        }

        /// <summary>Creates the metadata.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <returns>The metadata.</returns>
        private IEntityMetadata CreateMetadata(IExternalSearchQueryResult<SECResponse> resultItem, IExternalSearchRequest request)
        {
            var metadata = new EntityMetadataPart();

            this.PopulateMetadata(metadata, resultItem, request);

            return metadata;
        }

        /// <summary>Gets the origin entity code.</summary>
        /// <param name="resultItem">The result item.</param>
        /// <param name="lei"></param>
        /// <returns>The origin entity code.</returns>
        private EntityCode GetOriginEntityCode(string lei)
        {
            return new EntityCode("/Vendor", this.GetCodeOrigin(), lei);
        }

        /// <summary>Gets the code origin.</summary>
        /// <returns>The code origin</returns>
        private CodeOrigin GetCodeOrigin()
        {
            return CodeOrigin.CluedIn.CreateSpecific("SEC");
        }

        /// <summary>Populates the metadata.</summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="resultItem">The result item.</param>
        private void PopulateMetadata(IEntityMetadata metadata, IExternalSearchQueryResult<SECResponse> resultItem, IExternalSearchRequest request)
        {
            var data = resultItem.Data;

            var code = this.GetOriginEntityCode(data.cik);

            metadata.EntityType       = "/Vendor";
            metadata.Name = request.EntityMetaData.Name; //data.Attributes.Entity.LegalName?.Name;
            metadata.OriginEntityCode = code;

            metadata.Codes.Add(code);

            metadata.Properties[SECVocabularies.Organization.Category] = data.category;
            metadata.Properties[SECVocabularies.Organization.Description] = data.description;
            metadata.Properties[SECVocabularies.Organization.CIK] = data.cik;
            metadata.Properties[SECVocabularies.Organization.EIN] = data.ein;
            metadata.Properties[SECVocabularies.Organization.EntityType] = data.entityType;
            metadata.Properties[SECVocabularies.Organization.FiscalYearEnd] = data.fiscalYearEnd;
            metadata.Properties[SECVocabularies.Organization.Flags] = data.flags;
            metadata.Properties[SECVocabularies.Organization.FormerNames] = JsonUtility.Serialize(data.formerNames); //Alias
            metadata.Properties[SECVocabularies.Organization.InsiderTransactionForIssuerExists] = data.insiderTransactionForIssuerExists.ToString();
            metadata.Properties[SECVocabularies.Organization.InsiderTransactionForOwnerExists] = data.insiderTransactionForOwnerExists.ToString();
            metadata.Properties[SECVocabularies.Organization.InvestorWebsite] = data.investorWebsite;
            metadata.Properties[SECVocabularies.Organization.Name] = data.name;
            metadata.Properties[SECVocabularies.Organization.Phone] = data.phone;
            metadata.Properties[SECVocabularies.Organization.SIC] = data.sic;
            metadata.Properties[SECVocabularies.Organization.SicDescription] = data.sicDescription;
            metadata.Properties[SECVocabularies.Organization.StateOfIncorporation] = data.stateOfIncorporation;
            metadata.Properties[SECVocabularies.Organization.StateOfIncorporationDescription] = data.stateOfIncorporationDescription;
            metadata.Properties[SECVocabularies.Organization.Tickers] = JsonUtility.Serialize(data.tickers); //?
            metadata.Properties[SECVocabularies.Organization.Website] = data.website;
        }

        public IEnumerable<EntityType> Accepts(IDictionary<string, object> config, IProvider provider)
        {
            return AcceptedEntityTypes;
        }

        public IEnumerable<IExternalSearchQuery> BuildQueries(ExecutionContext context, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            return BuildQueries(context, request);
        }

        public IEnumerable<IExternalSearchQueryResult> ExecuteSearch(ExecutionContext context, IExternalSearchQuery query, IDictionary<string, object> config, IProvider provider)
        {
            return ExecuteSearch(context, query);
        }

        public IEnumerable<Clue> BuildClues(ExecutionContext context, IExternalSearchQuery query, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            return BuildClues(context, query, result, request);
        }

        public IEntityMetadata GetPrimaryEntityMetadata(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            return GetPrimaryEntityMetadata(context, result, request);
        }

        public IPreviewImage GetPrimaryEntityPreviewImage(ExecutionContext context, IExternalSearchQueryResult result, IExternalSearchRequest request, IDictionary<string, object> config, IProvider provider)
        {
            return GetPrimaryEntityPreviewImage(context, result, request);
        }

        public string Icon { get; } = Constants.Icon;
        public string Domain { get; } = Constants.Domain;
        public string About { get; } = Constants.About;
        public AuthMethods AuthMethods { get; } = Constants.AuthMethods;
        public IEnumerable<Control> Properties { get; } = Constants.Properties;
        public Guide Guide { get; } = Constants.Guide;
        public IntegrationType Type { get; } = Constants.IntegrationType;
    }
}
