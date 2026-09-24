using System.Xml.Linq;
using Fody;

public class ModuleWeaverOperatorTests
{
    static dynamic targetClass;

    static ModuleWeaverOperatorTests()
    {
        var weaver = new ModuleWeaver
        {
            Config = XElement.Parse("""<Caseless StringComparison="operator"/>"""),
        };

        var testResult = weaver.ExecuteTestRun(
            assemblyPath: "AssemblyToProcess.dll",
            assemblyName: $"{nameof(ModuleWeaverOperatorTests)}AssemblyToProcess");
        targetClass = testResult.GetInstance("TargetClass");
    }

    [Test]
    public async Task OpEquals()
    {
        await Assert.That((bool)targetClass.OpEquals()).IsFalse();
    }

    [Test]
    public async Task OpEqualsWithNull()
    {
        await Assert.That((bool)targetClass.OpEqualsWithNull()).IsFalse();
    }

    [Test]
    public async Task OpNotEquals()
    {
        await Assert.That((bool)targetClass.OpNotEquals()).IsTrue();
    }

    [Test]
    public async Task OpNotEqualsWithNull()
    {
        await Assert.That((bool)targetClass.OpNotEqualsWithNull()).IsTrue();
    }

    [Test]
    public async Task Equal()
    {
        await Assert.That((bool)targetClass.Equals()).IsFalse();
    }

    [Test]
    public async Task EqualsCallOnNull()
    {
        await Assert.That((bool)targetClass.EqualsCallOnNull()).IsFalse();
    }

    [Test]
    public async Task EqualsStatic()
    {
        await Assert.That((bool)targetClass.EqualsStatic()).IsFalse();
    }

    [Test]
    public async Task EqualsStaticWithNull()
    {
        await Assert.That((bool)targetClass.EqualsStaticWithNull()).IsFalse();
    }
}