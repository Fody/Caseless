using System.Xml.Linq;
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
    [ExpectedException(typeof (WeavingException))]
    public void GetStringComparisonFromXmlWhiteSpace()
    {
        var xElement = XElement.Parse("<Caseless StringComparison='  '/>");
        DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement);
    }

    [Test]
    [ExpectedException(typeof (WeavingException))]
    public void GetStringComparisonFromXmlEmpty()
    {
        var xElement = XElement.Parse("<Caseless StringComparison=''/>");
        DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement);
    }
}