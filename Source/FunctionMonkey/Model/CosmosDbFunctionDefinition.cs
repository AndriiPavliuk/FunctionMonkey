﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FunctionMonkey.Model
{
    public class CosmosDbFunctionDefinition : AbstractFunctionDefinition
    {
        public CosmosDbFunctionDefinition(Type commandType) : base("CosmosFn", commandType)
        {
        }

        public string DatabaseName { get; set; }

        public string CollectionName { get; set; }

        public string ConnectionStringName { get; set; }

        public IReadOnlyCollection<CosmosDbCommandProperty> CommandProperties { get; set; }

        public bool IsDocumentCommand { get; set; }

        public bool IsDocumentBatchCommand { get; set; }

        public string LeaseCollectionName { get; set; }

        public string LeaseDatabaseName { get; set; }

        public bool ConvertToPascalCase { get; set; }

        public bool CreateLeaseCollectionIfNotExists { get; set; }

        public bool StartFromBeginning { get; set; }

        public string LeaseConnectionStringName { get; set; }

        public string LeaseCollectionPrefix { get; set; }

        public int? MaxItemsPerInvocation { get; set; }

        public int? FeedPollDelay { get; set; }
        public int? LeaseAcquireInterval { get; set; }
        public int? LeaseExpirationInterval { get; set; }
        public int? LeaseRenewInterval { get; set; }
        public int? CheckpointFrequency { get; set; }
        public int? LeasesCollectionThroughput { get; set; }
        public string ErrorHandlerTypeName { get; set; }
        public Type ErrorHandlerType { get; set; }
    }
}
