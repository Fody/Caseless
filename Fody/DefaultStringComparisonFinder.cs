using System;
using System.Linq;
using System.Xml.Linq;

public class DefaultStringComparisonFinder
{
    public MsCoreReferenceFinder MsCoreReferenceFinder;
    public ModuleWeaver ModuleWeaver;
    public int StringComparisonConstant;
    public bool? UseOperatorForOrdinal;

    public void Execute()
    {
        var name = GetStringComparisonFromXml(ModuleWeaver.Config);

        switch (name.ToUpperInvariant())
        {
            case "OPERATOR":
            case "OPERATORS":
                UseOperatorForOrdinal = true;   // force a.Equals(b) being converted to a == b
                name = "Ordinal";               // everything else will use StringComparison.Ordinal
                break;
            case "ORDINAL":
                UseOperatorForOrdinal = false;  // leave force a.Equals(b) alone, it becomes a.Equals(b, StringComparison.Ordinal)
                break;
        }

        var fieldDefinitions = MsCoreReferenceFinder
            .StringComparisonDefinition
            .Fields
            .FirstOrDefault(x => name.Equals(x.Name, StringComparison.OrdinalIgnoreCase));

        if (fieldDefinitions == null)
        {
            throw new WeavingException(string.Format("Could not find value '{0}' in type 'System.StringComparison'. Please check your configuration.", name));
        }
        StringComparisonConstant = (int)fieldDefinitions.Constant;
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
        return value.Trim();
    }
}