using System;
using System.Linq;
using System.Xml.Linq;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

public class ModuleWeaver
{
    public Action<string> LogInfo { get; set; }
    public Action<string> LogWarning { get; set; }
    public ModuleDefinition ModuleDefinition { get; set; }
    public XElement Config { get; set; }
    MsCoreReferenceFinder msCoreReferenceFinder;
    ConverterCache converterCache;

    public ModuleWeaver()
    {
        LogInfo = s => { };
        LogWarning = s => { };
    }

    public void Execute()
    {
        msCoreReferenceFinder = new MsCoreReferenceFinder
                                        {
                                            AssemblyResolver = ModuleDefinition.AssemblyResolver,
                                        };
        msCoreReferenceFinder.Execute();
        var comparisonFinder = new DefaultStringComparisonFinder
                                   {
                                       ModuleWeaver = this,
                                       MsCoreReferenceFinder = msCoreReferenceFinder,
                                   };
        comparisonFinder.Execute();
        converterCache = new ConverterCache
                             {
                                 MsCoreReferenceFinder = msCoreReferenceFinder,
                                 ModuleDefinition = ModuleDefinition,
                                 DefaultStringComparisonFinder = comparisonFinder
                             };
        converterCache.Execute();

        foreach (var type in ModuleDefinition.GetTypes())
        {
            if (type.IsInterface)
            {
                continue;
            }
            if (type.IsEnum)
            {
                continue;
            }
            ProcessType(type);
        }
    }

    void ProcessType(TypeDefinition typeDefinition)
    {
        foreach (var method in typeDefinition.Methods)
        {
            if (!method.HasBody)
            {
                continue;
            }
            ProcessMethod(method);
        }
    }

    void ProcessMethod(MethodDefinition method)
    {
        method.Body.SimplifyMacros();
        var instructions = method.Body.Instructions;
        for (var index = 0; index < instructions.Count; index++)
        {
            var instruction = instructions[index];
            if (instruction.OpCode != OpCodes.Callvirt && instruction.OpCode != OpCodes.Call)
            {
                continue;
            }
            if (instruction.Operand == null)
            {
                continue;
            }
            var methodReference = instruction.Operand as MethodReference;
            if (methodReference == null)
            {
                continue;
            }

            if (methodReference.DeclaringType.FullName != "System.String")
            {
                continue;
            }
            foreach (var converter in converterCache.Converters)
            {
                var replaceWith = converter.Convert(methodReference).ToList();
                if (replaceWith.Count > 0)
                {
                    instructions.RemoveAt(index);
                    foreach (var innerInstruction in replaceWith)
                    {
                        instructions.Insert(index, innerInstruction);
                        index++;
                    }
                    break;
                }
            }
        }
        method.Body.OptimizeMacros();
    }
}