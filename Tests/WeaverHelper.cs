﻿using System.IO;
using Mono.Cecil;
using Mono.Cecil.Pdb;

public static class WeaverHelper
{

    public static string Weave(string assemblyPath)
    {
        var newAssembly = assemblyPath.Replace(".dll", "2.dll");
        var oldpdb = assemblyPath.Replace(".dll", ".pdb");
        var newpdb = assemblyPath.Replace(".dll", "2.pdb");
        File.Copy(assemblyPath, newAssembly, true);
        File.Copy(oldpdb, newpdb, true);

        using (var symbolStream = File.OpenRead(newpdb))
        {
            var readerParameters = new ReaderParameters
            {
                ReadSymbols = true,
                SymbolStream = symbolStream,
                SymbolReaderProvider = new PdbReaderProvider()
            };
            var moduleDefinition = ModuleDefinition.ReadModule(newAssembly, readerParameters);

            var weavingTask = new ModuleWeaver
            {
                ModuleDefinition = moduleDefinition,
            };

            weavingTask.Execute();
            moduleDefinition.Write(newAssembly);

            return newAssembly;
        }
    }
}