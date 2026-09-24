using System.Xml.Linq;
using Fody;

public class DefaultStringComparisonFinderTests
{
    [Test]
    public async Task GetStringComparisonFromXml()
    {
        var xElement = XElement.Parse("<Caseless StringComparison='InvariantCultureIgnoreCase'/>");
        await Assert.That(DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement)).IsEqualTo("InvariantCultureIgnoreCase");
    }

    [Test]
    public void GetStringComparisonFromXmlNull()
    {
        DefaultStringComparisonFinder.GetStringComparisonFromXml(null);
    }

    [Test]
    public async Task GetStringComparisonFromXmlTrim()
    {
        var xElement = XElement.Parse("<Caseless StringComparison=' InvariantCultureIgnoreCase '/>");
        await Assert.That(DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement)).IsEqualTo("InvariantCultureIgnoreCase");
    }

    [Test]
    public async Task GetStringComparisonFromXmlWhiteSpace()
    {
        var xElement = XElement.Parse("<Caseless StringComparison='  '/>");
        var exception = await Assert.That(() => { DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement); }).Throws<WeavingException>();
        await Assert.That(exception!.Message).IsEqualTo("Expected StringComparison to have a value.");
    }

    [Test]
    public async Task GetStringComparisonFromXmlEmpty()
    {
        var xElement = XElement.Parse("<Caseless StringComparison=''/>");
        var exception = await Assert.That(() => { DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement); }).Throws<WeavingException>();
        await Assert.That(exception!.Message).IsEqualTo("Expected StringComparison to have a value.");
    }
}