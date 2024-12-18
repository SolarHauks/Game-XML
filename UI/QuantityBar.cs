using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.UI;

// Barre de ressource, utilisée pour afficher la vie et le mana du joueur
public class QuantityBar
{
    private int _currentValue;  // Valeur actuelle de la ressource
    private readonly Vector2 _position; // Position de la barre
    
    private Texture2D Bar { get; }  // Texture de la barre

    public QuantityBar(int value, Color color, Vector2 position)
    {
        _currentValue = value;
        _position = position;
        
        // Création de la texture de la barre
        Bar = new Texture2D(Globals.GraphicsDevice, 1, 1);
        Bar.SetData([color]);
    }

    // Met à jour la valeur de la ressource
    public void Set(int value) => _currentValue = value;

    // Dessine la barre
    public void Draw()
    {
        Globals.SpriteBatch.Draw(Bar, new Rectangle((int)_position.X, (int)_position.Y, _currentValue, 10), Color.White);
    }
}