using System;
using System.IO;
using System.Reflection;
using Mono.Cecil;

public class MockAssemblyResolver : IAssemblyResolver
{
    public AssemblyDefinition Resolve(AssemblyNameReference name)
    {
#if (NETCOREAPP2_0)
        if (name.Name == "netstandard")
        {
            var netstandard = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
                @"dotnet\sdk\NuGetFallbackFolder\netstandard.library\2.0.0\build\netstandard2.0\ref\netstandard.dll");
            return AssemblyDefinition.ReadAssembly(
                fileName: netstandard,
                parameters: new ReaderParameters(ReadingMode.Deferred)
                {
                    AssemblyResolver = this
                });
        }
#endif
        Assembly assembly;
        try
        {
            assembly = Assembly.Load(name.FullName);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
        var codeBase = assembly.CodeBase.Replace("file:///", "");
        return AssemblyDefinition.ReadAssembly(
            fileName: codeBase,
            parameters: new ReaderParameters(ReadingMode.Deferred)
            {
                AssemblyResolver = this
            });
    }

    public AssemblyDefinition Resolve(AssemblyNameReference name, ReaderParameters parameters)
    {
        throw new NotImplementedException();
    }

    public AssemblyDefinition Resolve(string fullName)
    {
        try
        {
            var codeBase = Assembly.Load(fullName).CodeBase.Replace("file:///", "");

            return AssemblyDefinition.ReadAssembly(codeBase);
        }
        catch (FileNotFoundException)
        {
            return null;
        }
    }

    public AssemblyDefinition Resolve(string fullName, ReaderParameters parameters)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
    }
}