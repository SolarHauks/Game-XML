using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

public class Tile
{
    private List<Dictionary<Vector2, int>> _layerList;

    private Texture2D _textureAtlas;    // texture pour le tileset
    private Texture2D _hitboxTexture;   // ne sert que pour le debug à afficher les collisions
    private readonly Texture2D _rectangleTexture;    // texture de debug pour les collisions. Sert dans Tile.DrawRectHollow

    private int _displayTilesize;
    private int _pixelTilesize;
    private int _numTilesPerRow;
    private double _onScreenMultiplier = 1.0; // WIP

    public Dictionary<Vector2, int> Collisions { private set; get; }
    

    public Tile(Texture2D textureAtlas, Texture2D hitboxTexture, Texture2D rectangleTexture)
    {
        // initialisation des variables
        _layerList = new List<Dictionary<Vector2, int>>();
        Collisions = new Dictionary<Vector2, int>();
        
        // initialisation des textures
        _textureAtlas = textureAtlas;
        _hitboxTexture = hitboxTexture;
        _rectangleTexture = rectangleTexture;
        
        // chargement du fichier XML du niveau
        string xmlFilePath = "../../../Content/Assets/Level/Level1/output/test.tmx"; // Chemin du fichier XML
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlFilePath);
        
        // récupère les valeurs de _displayTilesize, _pixelTilesize et numTilesPerRow
        GetNumbers(doc);
        
        // récupère les layers
        GetLayers(doc);
        
    }
    
    // Récupère les layers du document XML et les sépare en layers d'affichage et layer de collision.
    // param : doc - Le document XML contenant les données des layers.
    private void GetLayers(XmlDocument doc)
    {
        int threshold = GetTilesetThreshold(doc);

        // traitement des layers
        XmlNodeList layerNodes = doc.SelectNodes("//layer/data"); // Selectionne toutes les données des noeuds layer

        if (layerNodes != null) {
            foreach (XmlNode layerNode in layerNodes)
            {
                string nodeContent = layerNode.InnerText;
                Dictionary<Vector2, int> layer = LoadMap(nodeContent);
                
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
        
    }
    
    private int GetTilesetThreshold(XmlDocument doc)
    {
        int threshold = 0;
        XmlNodeList tilesetNodes = doc.SelectNodes("//tileset"); // Selectionne tous les noeuds tileset
        if (tilesetNodes == null)
        {
            Console.WriteLine("Erreur : tilesetNodes est null");
            threshold = -1;
        }
        else
        {
            foreach (XmlNode tilesetNode in tilesetNodes)
            {
                if (tilesetNode?.Attributes?["source"]?.Value == null ||
                    tilesetNode.Attributes?["firstgid"]?.Value == null)
                {
                    Console.WriteLine("Erreur : le tilesetNode n'a pas les attributs requis");
                    return 0;
                }

                if (tilesetNode.Attributes["source"].Value.EndsWith("collision.tsx"))
                {
                    threshold = int.Parse(tilesetNode.Attributes["firstgid"].Value);
                }
            }
        }

        return threshold;
    }

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

        if (paramNode?.Attributes?["tilewidth"]?.Value == null)
        {
            Console.WriteLine("Erreur : l'attribut tilewidth est manquant ou n'a pas de valeur.");
            return;
        }
        
        _pixelTilesize = int.Parse(paramNode.Attributes["tilewidth"].Value);
    }

    private void GetNumTilesPerRow()
    {
        string xmlFilePath = "../../../Content/Assets/Level/Level1/tilesets/tileset.tsx"; // Chemin du fichier XML
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlFilePath);
        
        // traitement des paramètres des tiles
        XmlNode paramNode = doc.DocumentElement; // Sélectionne la racine du document

        if (paramNode?.Attributes?["columns"]?.Value == null)
        {
            Console.WriteLine("Erreur : l'attribut columns est manquant ou n'a pas de valeur.");
            return;
        }
        
        _numTilesPerRow = int.Parse(paramNode.Attributes["columns"].Value);
    }
    
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

    public void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        foreach (var layer in _layerList)
        {
            DrawMap(layer, spriteBatch, _textureAtlas, offset);
        }
        
        // debug : draw collisions
        /*foreach (var rect in intersections)
        {
            DrawRectHollow(_spriteBatch, 
                new Rectangle(
                    rect.X * display_tilesize,
                    rect.Y * display_tilesize,
                    display_tilesize,
                    display_tilesize
                ), 
                1);
        }*/
    }

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
    
    private void DrawRectHollow(SpriteBatch spriteBatch, Rectangle rect, int thickness) {
        spriteBatch.Draw(
            _rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Y,
                rect.Width,
                thickness
            ),
            Color.White
        );
        spriteBatch.Draw(
            _rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Bottom - thickness,
                rect.Width,
                thickness
            ),
            Color.White
        );
        spriteBatch.Draw(
            _rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Y,
                thickness,
                rect.Height
            ),
            Color.White
        );
        spriteBatch.Draw(
            _rectangleTexture,
            new Rectangle(
                rect.Right - thickness,
                rect.Y,
                thickness,
                rect.Height
            ),
            Color.White
        );
    }
}