using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace JeuVideo;

// Classe utilitaire pour la validation et la transformation de fichiers XML
// Tous est en static pour éviter d'avoir à instancier un objet pour les utiliser
public static class XmlUtils
{
    // Méthode permettant de valider un fichier XML par rapport à un schéma XSD
    public static void ValidateXmlFile (string schemaNamespace, string xsdFilePath, string xmlFilePath) {
        var settings = new XmlReaderSettings();
        settings.Schemas.Add (schemaNamespace, xsdFilePath);
        settings.ValidationType = ValidationType.Schema;
        settings.ValidationEventHandler += ValidationCallBack;
        var readItems = XmlReader.Create(xmlFilePath, settings);
        while (readItems.Read()) { }
    }

    // Méthode annexe affichant les erreurs et warning rencontrées lors de la validation ci dessus
    private static void ValidationCallBack(object sender, ValidationEventArgs e) {
        if (e.Severity.Equals(XmlSeverityType.Warning)) {
            Console.Write("WARNING: ");
            Console.WriteLine(e.Message);
        }
        else if (e.Severity.Equals(XmlSeverityType.Error)) {
            Console.Write("ERROR: ");
            Console.WriteLine(e.Message);
        }
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