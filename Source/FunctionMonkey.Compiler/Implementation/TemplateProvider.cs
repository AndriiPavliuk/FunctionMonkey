﻿using System;
using System.Collections.Generic;
using System.IO;
using FunctionMonkey.Model;

namespace FunctionMonkey.Compiler.Implementation
{
    internal class TemplateProvider : ITemplateProvider
    {
        private static readonly Dictionary<Type, string> TypeToTemplatePrefixMap = new Dictionary<Type, string>
        {
            {typeof(HttpFunctionDefinition), "http"},
            {typeof(ServiceBusQueueFunctionDefinition),"servicebusqueue" },
            {typeof(ServiceBusSubscriptionFunctionDefinition),"servicebussubscription" },
            {typeof(StorageQueueFunctionDefinition),"storagequeue" },
            {typeof(BlobFunctionDefinition),"storageblob" },
            {typeof(BlobStreamFunctionDefinition),"storageblobstream" },
            {typeof(TimerFunctionDefinition),"timer" },
            {typeof(CosmosDbFunctionDefinition), "cosmosdb" }
        };

        public string GetCSharpTemplate(AbstractFunctionDefinition functionDefinition)
        {
            return GetTemplate(functionDefinition, "csharp");
        }

        public string GetTemplate(string name, string type)
        {
            using (Stream stream = GetType().Assembly
                .GetManifestResourceStream($"FunctionMonkey.Compiler.Templates.{name}.{type}.handlebars"))
            {
                if (stream != null)
                {
                    using (StreamReader reader = new StreamReader(stream))
                        return reader.ReadToEnd();
                }
            }

            throw new ConfigurationException($"No templates are configured for function definitions of type {name}");
        }

        public string GetJsonTemplate(AbstractFunctionDefinition functionDefinition)
        {
            return GetTemplate(functionDefinition, "json");
        }

        private string GetTemplate(AbstractFunctionDefinition functionDefinition, string type)
        {
            if (TypeToTemplatePrefixMap.TryGetValue(functionDefinition.GetType(), out string prefix))
            {
                using (Stream stream = GetType().Assembly.GetManifestResourceStream($"FunctionMonkey.Compiler.Templates.{prefix}.{type}.handlebars"))
                using (StreamReader reader = new StreamReader(stream))
                    return reader.ReadToEnd();
            }
            throw new ConfigurationException($"No templates are configured for function definitions of type {functionDefinition.GetType().Name}");
        }

        public string GetCSharpLinkBackTemplate()
        {
            using (Stream stream = GetType().Assembly.GetManifestResourceStream("FunctionMonkey.Compiler.Templates.forcereference.csharp.handlebars"))
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}
