using System.Xml.Linq;
using Fody;
using Xunit;

public class DefaultStringComparisonFinderTests
{
    [Fact]
    public void GetStringComparisonFromXml()
    {
        var xElement = XElement.Parse("<Caseless StringComparison='InvariantCultureIgnoreCase'/>");
        Assert.Equal("InvariantCultureIgnoreCase", DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement));
    }

    [Fact]
    public void GetStringComparisonFromXmlNull()
    {
        DefaultStringComparisonFinder.GetStringComparisonFromXml(null);
    }

    [Fact]
    public void GetStringComparisonFromXmlTrim()
    {
        var xElement = XElement.Parse("<Caseless StringComparison=' InvariantCultureIgnoreCase '/>");
        Assert.Equal("InvariantCultureIgnoreCase", DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement));
    }

    [Fact]
    public void GetStringComparisonFromXmlWhiteSpace()
    {
        var xElement = XElement.Parse("<Caseless StringComparison='  '/>");
        var exception = Assert.Throws<WeavingException>(() => DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement));
        Assert.Equal("Expected StringComparison to have a value.", exception.Message);
    }

    [Fact]
    public void GetStringComparisonFromXmlEmpty()
    {
        var xElement = XElement.Parse("<Caseless StringComparison=''/>");
        var exception = Assert.Throws<WeavingException>(() => DefaultStringComparisonFinder.GetStringComparisonFromXml(xElement));
        Assert.Equal("Expected StringComparison to have a value.", exception.Message);
    }
}