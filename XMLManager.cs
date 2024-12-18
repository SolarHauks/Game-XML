using System.IO;
using System.Xml.Serialization;

namespace JeuVideo;

/* An XMLManager is used to Serialize (save) /Deserialize (load) XML documents and Objects.
It's a generic class that must be specialized for a particular object that corresponds to the XML document.

documentation : https://learn.microsoft.com/fr-fr/dotnet/api/system.xml.serialization.xmlserializer.deserialize?view=net-8.0
*/

// Classe contenant les méthodes pour la sérialization et la désérialization d'un fichier XML
// Directement reprise des exemples du cours
public class XmlManager<T> {
    
    // Désérialize un fichier XML en un objet de type T
    public T Load(string path) {
        // Declare a generic T variable of the type to be deserialized to store the deserialized xml file 
        T instance;
        // reads the text (xml) file and store in reader
        using (TextReader reader = new StreamReader(path)) {
            // create an instance of the XmlSerializer
            var xml = new XmlSerializer(typeof(T));
            // deserialize and casts the text into the type T
            instance = (T) xml.Deserialize(reader);
        }

        // returns the document as a serialized object of type T
        return instance;
    }
    
    public void Save(string path, object obj) {
        using (TextWriter writer = new StreamWriter(path)) {
            var xml = new XmlSerializer(typeof(T));
            xml.Serialize(writer, obj);
        }
    }
    
    public void Save(string path, object obj, XmlSerializerNamespaces ns) {
        using (TextWriter writer = new StreamWriter(path)) {
            var xml = new XmlSerializer(typeof(T));
            xml.Serialize(writer, obj, ns);
        }
    }

}