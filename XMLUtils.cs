using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace JeuVideo;

// Classe utilitaire pour la validation et la transformation de fichiers XML
// Tous est en static pour éviter d'avoir à instancier un objet pour les utiliser
public static class XmlUtils
{
    public static void ValidateXmlFiles(string folderPath, string schemaNamespace, string xsdFilePath)
    {
        XmlSchemaSet schemaSet = new XmlSchemaSet();
        schemaSet.Add(schemaNamespace, xsdFilePath);

        foreach (string xmlFilePath in Directory.GetFiles(folderPath, "*.xml"))
        {
            ValidateXmlFile(xmlFilePath, schemaSet);
        }
    }

    public static void ValidateXmlFile(string schemaNamespace, string xsdFilePath, string xmlFilePath)
    {
        XmlSchemaSet schemaSet = new XmlSchemaSet();
        schemaSet.Add(schemaNamespace, xsdFilePath);

        ValidateXmlFile(xmlFilePath, schemaSet);
    }

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

        Console.WriteLine($"{xmlFilePath} is valid.");
    }
    
    // Méthode pemettant d'appliquer une transformation XSLT sur un fichier XML,
    // et d'enregistrer le résultat dans un fichier de sortie
    public static void XslTransform(string xmlFilePath, string xsltFilePath, string outputFilePath)
    {
        XPathDocument xpathDoc = new XPathDocument(xmlFilePath);
        XslCompiledTransform xslt = new XslCompiledTransform();
        
        // Permet de forcer la résolution des ressources externes (fonction document() et les URI externes)
        XsltSettings settings = new XsltSettings(true, true);
        XmlResolver resolver = new XmlUrlResolver();

        // Crée une liste d'arguments pour passer des paramètres à la transformation XSLT
        XsltArgumentList argList = new XsltArgumentList();
        // Selon le fichier XSLT, on ajoute des paramètres correspondants
        if (xsltFilePath.EndsWith("infirmiere.xslt"))
        {
            // Passe à la transformation le numéro de l'infirmiere en paramètre
            argList.AddParam("destinedId", "", "001");
        } else if (xsltFilePath.EndsWith("extractionPatient.xslt"))
        {
            // Passe à la transformation le nom du patient en paramètre
            argList.AddParam("destinedName", "", "Pourferlavésel");
        }

        xslt.Load(xsltFilePath, settings, resolver);
        using XmlTextWriter outputWriter = new XmlTextWriter(outputFilePath, null);
        xslt.Transform(xpathDoc, argList, outputWriter);
    }
}