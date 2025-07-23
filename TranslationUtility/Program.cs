using System.Text.Json;
using System.Xml;

Console.WriteLine("🌍 Multilingual Journal - AI Translation Utility");
Console.WriteLine("================================================");
Console.WriteLine();

// Mock translation service - in real implementation, this would call Azure Translator or OpenAI
var mockTranslations = new Dictionary<string, Dictionary<string, string>>
{
    ["Home"] = new()
    {
        ["es"] = "Inicio",
        ["fr"] = "Accueil", 
        ["de"] = "Startseite",
        ["it"] = "Casa"
    },
    ["MyJournal"] = new()
    {
        ["es"] = "Mi Diario",
        ["fr"] = "Mon Journal",
        ["de"] = "Mein Tagebuch", 
        ["it"] = "Il Mio Diario"
    },
    ["NewEntry"] = new()
    {
        ["es"] = "Nueva Entrada",
        ["fr"] = "Nouvelle Entrée",
        ["de"] = "Neuer Eintrag",
        ["it"] = "Nuova Voce"
    }
};

var supportedLanguages = new[] { "es", "fr", "de", "it" };
var basePath = "../MultilingualJournalApp/Resources/Pages/Shared";

Console.WriteLine($"📁 Base path: {basePath}");
Console.WriteLine($"🔤 Supported languages: {string.Join(", ", supportedLanguages)}");
Console.WriteLine();

try
{
    // Read the English resource file
    var englishFile = Path.Combine(basePath, "SharedResource.en.resx");
    
    if (!File.Exists(englishFile))
    {
        Console.WriteLine($"❌ English resource file not found: {englishFile}");
        return;
    }

    Console.WriteLine("📖 Reading English resource file...");
    var englishResources = ReadResxFile(englishFile);
    
    Console.WriteLine($"✅ Found {englishResources.Count} resource keys");
    Console.WriteLine();

    // Generate translations for each language
    foreach (var language in supportedLanguages)
    {
        Console.WriteLine($"🔄 Generating {language.ToUpper()} translations...");
        
        var translatedResources = new Dictionary<string, string>();
        
        foreach (var resource in englishResources)
        {
            // Use mock translation or fallback to English
            var translatedValue = GetMockTranslation(resource.Key, language, resource.Value);
            translatedResources[resource.Key] = translatedValue;
            
            Console.WriteLine($"  {resource.Key}: {resource.Value} → {translatedValue}");
        }

        // Write translated resource file
        var outputFile = Path.Combine(basePath, $"SharedResource.{language}.resx");
        WriteResxFile(outputFile, translatedResources);
        
        Console.WriteLine($"✅ Created: {outputFile}");
        Console.WriteLine();
    }

    Console.WriteLine("🎉 Translation completed successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error: {ex.Message}");
}

string GetMockTranslation(string key, string language, string englishValue)
{
    // Check if we have a mock translation
    if (mockTranslations.ContainsKey(key) && mockTranslations[key].ContainsKey(language))
    {
        return mockTranslations[key][language];
    }
    
    // In a real implementation, this would call Azure Translator API or OpenAI API
    // For now, return a prefixed version to show it's "translated"
    return $"[{language.ToUpper()}] {englishValue}";
}

Dictionary<string, string> ReadResxFile(string filePath)
{
    var resources = new Dictionary<string, string>();
    var doc = new XmlDocument();
    doc.Load(filePath);
    
    var dataNodes = doc.SelectNodes("//data");
    if (dataNodes != null)
    {
        foreach (XmlNode node in dataNodes)
        {
            var nameAttr = node.Attributes?["name"];
            var valueNode = node.SelectSingleNode("value");
            
            if (nameAttr != null && valueNode != null)
            {
                resources[nameAttr.Value] = valueNode.InnerText;
            }
        }
    }
    
    return resources;
}

void WriteResxFile(string filePath, Dictionary<string, string> resources)
{
    var doc = new XmlDocument();
    
    // Create the root element
    var root = doc.CreateElement("root");
    doc.AppendChild(root);
    
    // Add resource headers
    AddResxHeader(doc, root, "resmimetype", "text/microsoft-resx");
    AddResxHeader(doc, root, "version", "2.0");
    AddResxHeader(doc, root, "reader", "System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
    AddResxHeader(doc, root, "writer", "System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
    
    // Add empty line comment
    var comment = doc.CreateComment(" Translated resources ");
    root.AppendChild(comment);
    
    // Add resource data
    foreach (var resource in resources)
    {
        var dataElement = doc.CreateElement("data");
        dataElement.SetAttribute("name", resource.Key);
        dataElement.SetAttribute("xml:space", "preserve");
        
        var valueElement = doc.CreateElement("value");
        valueElement.InnerText = resource.Value;
        dataElement.AppendChild(valueElement);
        
        root.AppendChild(dataElement);
    }
    
    // Save the file
    var settings = new XmlWriterSettings
    {
        Indent = true,
        IndentChars = "  "
    };
    
    using var writer = XmlWriter.Create(filePath, settings);
    doc.Save(writer);
}

void AddResxHeader(XmlDocument doc, XmlElement root, string name, string value)
{
    var header = doc.CreateElement("resheader");
    header.SetAttribute("name", name);
    
    var valueElement = doc.CreateElement("value");
    valueElement.InnerText = value;
    header.AppendChild(valueElement);
    
    root.AppendChild(header);
}