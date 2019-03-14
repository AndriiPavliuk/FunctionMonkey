﻿using System.IO;
using AzureFromTheTrenches.Commanding.Abstractions;
using FunctionMonkey.Commanding.Abstractions;

namespace FunctionMonkey.Tests.Integration.Functions.Commands
{
    public class StreamBlobCommand : ICommand, IStreamCommand
    {
        public Stream Stream { get; set; }
        public string Name { get; set; }
    }
}
