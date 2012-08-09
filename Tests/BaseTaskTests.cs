using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;

public abstract class BaseTaskTests
{
    string projectPath;
    Assembly assembly;
    dynamic targetClass;

    protected BaseTaskTests(string projectPath)
    {

#if (!DEBUG)

            projectPath = projectPath.Replace("Debug", "Release");
#endif
        this.projectPath = projectPath;
    }

    [TestFixtureSetUp]
    public void Setup()
    {
        Stopwatch startNew = Stopwatch.StartNew();
        var weaverHelper = new WeaverHelper(projectPath);
        startNew .Stop();
        Debug.WriteLine(startNew.ElapsedMilliseconds);
        assembly = weaverHelper.Assembly;

        targetClass = assembly.GetInstance("TargetClass");
    }


    [Test]
    public void CompareTo()
    {
        Assert.AreEqual(0,targetClass.CompareTo());
    }
    [Test]
    public void CompareStatic()
    {
        Assert.AreEqual(0,targetClass.CompareStatic());
    }

    [Test]
    public void Contains()
    {
        Assert.IsTrue(targetClass.Contains());
    }
    [Test]
    public void IndexOf()
    {
        Assert.AreEqual(0,targetClass.IndexOf());
    }
    [Test]
    public void IndexOf_StartIndex()
    {
        Assert.AreEqual(1, targetClass.IndexOf_StartIndex());
    }
    [Test]
    public void IndexOf_StartIndexCount()
    {
        Assert.AreEqual(1, targetClass.IndexOf_StartIndexCount());
    }

    [Test]
    public void LastIndexOf()
    {
        Assert.AreEqual(0,targetClass.LastIndexOf());
    }

    [Test]
    public void OpEquals()
    {
        Assert.IsTrue(targetClass.OpEquals());
    }
    [Test]
    public void OpNotEquals()
    {
        Assert.IsFalse(targetClass.OpNotEquals());
    }
    [Test]
    public void StartsWith()
    {
        Assert.IsTrue(targetClass.StartsWith());
    }
    [Test]
    public void EndsWith()
    {
        Assert.IsTrue(targetClass.EndsWith());
    }
    [Test]
    public void Equals()
    {
        Assert.IsTrue(targetClass.Equals());
    }
    [Test]
    public void EqualsStatic()
    {
        Assert.IsTrue(targetClass.EqualsStatic());
    }


#if(DEBUG)
    [Test]
    public void PeVerify()
    {
        Verifier.Verify(assembly.CodeBase.Remove(0, 8));
    }
#endif

}