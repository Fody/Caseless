using System.Linq;
using System.Xml.Linq;

public class DefaultStringComparisonFinder
{
    public MsCoreReferenceFinder MsCoreReferenceFinder;
    public ModuleWeaver ModuleWeaver;
    public int StringComparisonConstant;

    public void Execute()
    {
        var stringComparisonKey = GetStringComparisonFromXml(ModuleWeaver.Config);
        var fieldDefinitions = MsCoreReferenceFinder
            .StringComparisonDefinition
            .Fields
            .FirstOrDefault(x => x.Name == stringComparisonKey);
        if (fieldDefinitions == null)
        {
            throw new WeavingException(string.Format("Could not find value '{0}' in type 'System.StringComparison'. Please check your configuration.", stringComparisonKey));
        }
        StringComparisonConstant = (int) fieldDefinitions.Constant;
    }

    public static string GetStringComparisonFromXml(XElement xElement)
    {
        if (xElement == null)
        {
            return "OrdinalIgnoreCase";
        }
        var xAttribute = xElement.Attribute("StringComparison");
        if (xAttribute == null)
        {
            return "OrdinalIgnoreCase";
        }
        var value = xAttribute.Value;
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new WeavingException("Expected StringComparison to have a value.");
        }
        return value;
    }
}