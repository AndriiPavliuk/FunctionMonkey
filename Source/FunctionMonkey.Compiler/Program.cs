﻿using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using FunctionMonkey.Compiler.Implementation;

namespace FunctionMonkey.Compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("Must specify the assembly file to build the functions from");
            }

            string inputAssemblyFile = args[0];
            
            FunctionCompiler.TargetEnum target = FunctionCompiler.TargetEnum.NETStandard20;
            if (args.Any(x => x.ToLower() == "--netcore21"))
            {
                target = FunctionCompiler.TargetEnum.NETCore21;
            }
            
            // TODO: convert the input to an absolute path if necessary
            Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(inputAssemblyFile);
            string outputBinaryDirectory = Path.GetDirectoryName(assembly.Location);
            
            // Not sure why the AssemblyLoadContext doesn't deal with the below. I thought it did. Clearly not.
            // TODO: Have a chat with someone who knows a bit more about this.
            AssemblyLoadContext.Default.Resolving += (context, name) =>
            {
                string path = Path.Combine(outputBinaryDirectory, $"{name.Name}.dll");
                //string path = $"{outputBinaryDirectory}\\{name.Name}.dll";
                if (File.Exists(path))
                {
                    Assembly referencedAssembly = context.LoadFromAssemblyPath(path);
                    return referencedAssembly;
                }                
                return null;
            };

            FunctionCompiler compiler = new FunctionCompiler(assembly, outputBinaryDirectory, true, target);
            compiler.Compile();
        }
    }
}
