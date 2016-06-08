using System.Xml.Linq;
using ApprovalTests;
using NUnit.Framework;

[TestFixture]
public class DefaultStringComparisonFinderTests
{
    [Test]
    public void GetStringComparisonFromXml()
    {
        var xElement = XElement.Parse("<Caseless StringComparison='InvariantCultureIgnoreCase'/>");
        Assert.AreEqual("InvariantCultureIgnoreCase", DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement));
    }

    [Test]
    public void GetStringComparisonFromXmlNull()
    {
        DefaultStringComparisonFinder.GetStringComparisonFromXml(null);
    }

    [Test]
    public void GetStringComparisonFromXmlTrim()
    {
        var xElement = XElement.Parse("<Caseless StringComparison=' InvariantCultureIgnoreCase '/>");
        Assert.AreEqual("InvariantCultureIgnoreCase", DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement));
    }

    [Test]
    public void GetStringComparisonFromXmlWhiteSpace()
    {
        var xElement = XElement.Parse("<Caseless StringComparison='  '/>");
        var exception = Assert.Throws<WeavingException>(() => DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement));
        Approvals.Verify(exception.Message);
    }

    [Test]
    public void GetStringComparisonFromXmlEmpty()
    {
        var xElement = XElement.Parse("<Caseless StringComparison=''/>");
        var exception = Assert.Throws<WeavingException>(() => DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement));
        Approvals.Verify(exception.Message);
    }
}