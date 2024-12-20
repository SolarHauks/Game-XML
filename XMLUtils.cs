using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace JeuVideo;

// Classe utilitaire pour la validation et la transformation de fichiers XML
// Tous est en static pour éviter d'avoir à instancier un objet pour les utiliser
// La base de la classe est la classe donnée dans le cours
// Les méthodes de validation ont été adaptés et réécrites.
public static class XmlUtils
{
    // Valide tous les fichiers XML contenu dans un dossier avec un schéma XSD
    public static void ValidateXmlFiles(string folderPath, string schemaNamespace, string xsdFilePath)
    {
        XmlSchemaSet schemaSet = new XmlSchemaSet();
        schemaSet.Add(schemaNamespace, xsdFilePath);

        foreach (string xmlFilePath in Directory.GetFiles(folderPath, "*.xml"))
        {
            ValidateXmlFile(xmlFilePath, schemaSet);
        }
    }

    // Valide un seul fichier XML avec un schéma XSD
    public static void ValidateXmlFile(string schemaNamespace, string xsdFilePath, string xmlFilePath)
    {
        XmlSchemaSet schemaSet = new XmlSchemaSet();
        schemaSet.Add(schemaNamespace, xsdFilePath);

        ValidateXmlFile(xmlFilePath, schemaSet);
    }
    
    // Méthode permettant de valider un fichier XML avec un schéma XSD
    private static void ValidateXmlFile(string xmlFilePath, XmlSchemaSet schemaSet)
    {
        XmlDocument doc = new XmlDocument();
        doc.Schemas.Add(schemaSet);
        doc.Load(xmlFilePath);

        doc.Validate((sender, e) =>
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                Console.WriteLine($"Warning: {e.Message}");
            }
            else if (e.Severity == XmlSeverityType.Error)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
        });
    }
    
    // Méthode pemettant d'appliquer une transformation XSLT sur un fichier XML,
    // et d'enregistrer le résultat dans un fichier de sortie
    public static void XslTransform(string xmlFilePath, string xsltFilePath, string outputFilePath)
    {
        XPathDocument xpathDoc = new XPathDocument(xmlFilePath);
        XslCompiledTransform xslt = new XslCompiledTransform();

        xslt.Load(xsltFilePath);
        
        using XmlTextWriter outputWriter = new XmlTextWriter(outputFilePath, null);
        xslt.Transform(xpathDoc, outputWriter);
    }
}