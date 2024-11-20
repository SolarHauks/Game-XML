using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

public class Tile
{
    private readonly List<Dictionary<Vector2, int>> _layerList; // liste des layers du niveau

    private readonly Texture2D _textureAtlas;    // texture pour le tileset
    private Texture2D _hitboxTexture;   // ne sert que pour le debug à afficher les collisions

    private int _displayTilesize;   // taille d'un tile à l'écran
    private int _pixelTilesize;    // taille d'un tile en pixel
    private int _numTilesPerRow;    // nombre de tiles par ligne dans le tileset
    private readonly double _onScreenMultiplier = 1.0; // WIP

    // liste des tiles de collision, qu'on sépare par soucis de simplicité
    public Dictionary<Vector2, int> Collisions { private set; get; }
    

    public Tile(Texture2D textureAtlas, Texture2D hitboxTexture)
    {
        // initialisation des variables
        _layerList = new List<Dictionary<Vector2, int>>();
        Collisions = new Dictionary<Vector2, int>();
        
        // initialisation des textures
        _textureAtlas = textureAtlas;
        _hitboxTexture = hitboxTexture;
        
        Load();
    }

    // Charge le niveau à partir des données XML
    private void Load()
    {
        // chargement du fichier XML du niveau
        string xmlFilePath = "../../../Content/Assets/Level/level1.tmx"; // Chemin du fichier XML
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlFilePath);
        
        // Validation du fichier XML
        // Cela permet d'etre sur que les noeuds qu'on va utiliser après ne sont pas nul
        // sans avoir besoin de faire des vérifications
        string schemaNamespace = "https://www.univ-grenoble-alpes.fr/jeu/level";
        string xsdFilePath = "../../../Content/Assets/Level/levelSchema.xsd";
        XmlUtils.ValidateXmlFile(schemaNamespace, xsdFilePath, xmlFilePath);
        
        // récupère les valeurs de _displayTilesize, _pixelTilesize et numTilesPerRow
        GetNumbers(doc);
        
        // récupère les layers
        GetLayers(doc);
    }
    
    // Récupère les layers du document XML et les sépare en layers d'affichage et layer de collision.
    // param : doc - Le document XML contenant les données des layers.
    private void GetLayers(XmlDocument doc)
    {
        int threshold = GetTilesetThreshold(doc);   // Récupère la valeur de seuil pour détecter le layer de collision

        // traitement des layers
        XmlNodeList layerNodes = doc.SelectNodes("//layer/data"); // Selectionne toutes les données des noeuds layer
        
        foreach (XmlNode layerNode in layerNodes)
        {
            string nodeContent = layerNode.InnerText;   // Données du layer
            Dictionary<Vector2, int> layer = LoadMap(nodeContent);  // Charge les données du layer dans un dictionnaire
            
            // On regarde si le layer qu'on vient de charger est le layer de collision
            // Si oui on le sépare des autres car on ne veut pas l'afficher, on s'en sert just pour la logique
            bool containsGreaterValue = false;

            foreach (int value in layer.Values)
            {
                if (value >= threshold)
                {
                    containsGreaterValue = true;
                    break;
                }
            }
            
            if (containsGreaterValue) {
                Collisions = layer;
            } else {
                _layerList.Add(layer);
            }
        }
    }
    
    // Récupère la valeur de seuil pour détecter le layer de collision
    private int GetTilesetThreshold(XmlDocument doc)
    {
        int threshold = 0;
        XmlNodeList tilesetNodes = doc.SelectNodes("//tileset"); // Selectionne tous les noeuds tileset

        foreach (XmlNode tilesetNode in tilesetNodes)
        {
            if (tilesetNode.Attributes["source"].Value.EndsWith("collision.tsx"))
            {
                threshold = int.Parse(tilesetNode.Attributes["firstgid"].Value);
            }
        }

        return threshold;
    }
    
    // Récupère les valeurs de _displayTilesize, _pixelTilesize et numTilesPerRow
    private void GetNumbers(XmlDocument doc)
    {
        GetNumTilesPerRow();
        GetPixelTileSize(doc);
        _displayTilesize = (int)(_pixelTilesize * _onScreenMultiplier); // Ne marche pas pour le moment, WIP.
    }

    private void GetPixelTileSize(XmlDocument doc)
    {
        // traitement des paramètres des tiles
        XmlNode paramNode = doc.DocumentElement; // Sélectionne la racine du document
        _pixelTilesize = int.Parse(paramNode.Attributes["tilewidth"].Value);
    }
    
    private void GetNumTilesPerRow()
    {
        string xmlFilePath = "../../../Content/Assets/Level/Data/Level1/tilesets/tileset.tsx"; // Path to the XML file

        using (XmlReader reader = XmlReader.Create(xmlFilePath))
        {
            while (reader.Read())
            {
                if (reader.IsStartElement() && reader.Name == "tileset")
                {
                    string columns = reader.GetAttribute("columns");
                    _numTilesPerRow = int.Parse(columns);
                    break;
                }
            }
        }
    }
    
    // Charge les données d'un layer dans un dictionnaire
    private Dictionary<Vector2, int> LoadMap(string data)
    {
        Dictionary<Vector2, int> result = new();
        
        string[] lines = data.Split('\n');
        int y = 0;
        foreach (string line in lines) {
            string[] items = line.Split(',');

            for (int x = 0; x < items.Length; x++) {
                if (int.TryParse(items[x], out int value)) {
                    if (value > 0) {
                        result[new Vector2(x, y)] = value;
                    }
                }
            }
            y++;
        }
        return result;
    }

    // Affiche le niveau
    public void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        foreach (var layer in _layerList)
        {
            DrawMap(layer, spriteBatch, _textureAtlas, offset);
        }
    }

    // Affiche un layer
    private void DrawMap(Dictionary<Vector2, int> data, SpriteBatch spriteBatch, Texture2D texture, Vector2 offset)
    {
        foreach (var item in data)
        {
            // Rectangle de destination, on le place en fonction de la position du tile et du décalage lié à la caméra
            Rectangle drect = new(
                (int)(item.Key.X * _displayTilesize + offset.X),
                (int)(item.Key.Y * _displayTilesize + offset.Y),
                _displayTilesize,
                _displayTilesize
            );
            
            // De part comment Tiled génère ses fichiers, on doit faire -1 pour avoir le bon index
            int x = (item.Value - 1) % _numTilesPerRow;
            int y = (item.Value - 1) / _numTilesPerRow;
            
            Rectangle src = new(
                x * _pixelTilesize,
                y * _pixelTilesize,
                _pixelTilesize,
                _pixelTilesize
            );
            
            spriteBatch.Draw(texture, drect, src, Color.White);
        }
    }
    
}