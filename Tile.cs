using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

public class Tile
{
    private Dictionary<Vector2, int> _mg;
    private Dictionary<Vector2, int> _bg;
    private Dictionary<Vector2, int> _collisions;

    private Texture2D _textureAtlas;
    private Texture2D _hitboxTexture;

    private int _displayTilesize;
    private int _numTilesPerRow;
    private int _pixelTilesize;
    
    private readonly Texture2D _rectangleTexture;

    public Dictionary<Vector2, int> Collisions
    {
        init => _collisions = value; 
        get => _collisions;
    }

    public Tile(Texture2D textureAtlas, Texture2D hitboxTexture, Texture2D rectangleTexture)
    {
        _bg = LoadMap("../../../Content/Assets/Level/Level1/output/bg.csv");
        _mg = LoadMap("../../../Content/Assets/Level/Level1/output/mg.csv");
        _collisions = LoadMap("../../../Content/Assets/Level/Level1/output/collision.csv");

        this._textureAtlas = textureAtlas;
        this._hitboxTexture = hitboxTexture;
        
        this._rectangleTexture = rectangleTexture;
        
        _displayTilesize = 16;
        _numTilesPerRow = 17;
        _pixelTilesize = 16;
    }
    
    private Dictionary<Vector2, int> LoadMap(string filepath)
    {
        Dictionary<Vector2, int> result = new();
        StreamReader reader = new(filepath);
        
        string line;
        int y = 0;
        while ((line = reader.ReadLine()) != null) {
            
            string[] items = line.Split(',');
            
            for(int x = 0; x < items.Length; x++) {
                if (int.TryParse(items[x], out int value)) {
                    if (value > -1) {
                        result[new Vector2(x, y)] = value;
                    }
                }
            }
            y++;
        }
        return result;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawMap(_mg, spriteBatch, _textureAtlas);
        DrawMap(_bg, spriteBatch, _textureAtlas);
        DrawMap(_collisions, spriteBatch, _hitboxTexture);
        
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

    private void DrawMap(Dictionary<Vector2, int> data, SpriteBatch spriteBatch, Texture2D texture)
    {
        foreach (var item in data)
        {
            Rectangle drect = new(
                (int)item.Key.X * _displayTilesize,
                (int)item.Key.Y * _displayTilesize,
                _displayTilesize,
                _displayTilesize
            );
            
            int x = item.Value % _numTilesPerRow;
            int y = item.Value / _numTilesPerRow;
            
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