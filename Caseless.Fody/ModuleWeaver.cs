using System.Collections.Generic;
using System.Linq;
using Fody;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Mono.Cecil.Rocks;

public partial class ModuleWeaver : BaseModuleWeaver
{
    ConverterCache converterCache;

    public override void Execute()
    {
        FindCoreReferences();
        var comparisonFinder = new DefaultStringComparisonFinder
        {
            ModuleWeaver = this
        };
        comparisonFinder.Execute();
        converterCache = new ConverterCache
        {
            ModuleWeaver = this,
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

    public override IEnumerable<string> GetAssembliesForScanning()
    {
        yield return "netstandard";
        yield return "mscorlib";
        yield return "System";
        yield return "System.Runtime";
        yield return "System.Core";
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
            if (instruction.OpCode != OpCodes.Callvirt &&
                instruction.OpCode != OpCodes.Call)
            {
                continue;
            }

            if (instruction.Operand is not MethodReference methodReference)
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
                if (replaceWith.Count <= 0)
                {
                    continue;
                }
                foreach (var inst in instructions.Where(_ => _.Operand == instruction))
                {
                    inst.Operand = replaceWith[0];
                }

                instructions.RemoveAt(index);
                foreach (var innerInstruction in replaceWith)
                {
                    instructions.Insert(index, innerInstruction);
                    index++;
                }

                break;
            }
        }

        method.Body.OptimizeMacros();
    }

    public override bool ShouldCleanReference => true;
}