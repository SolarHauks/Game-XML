using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo.UI;

// Bouton de menu
public class Button : Sprite
{
    private MouseState _previousMouseState; // Etat de la souris à la frame précédente
    private readonly Texture2D _texture;    // Texture du bouton
    private Rectangle _destRect;    // Rectangle de destination

    public bool IsClicked { get; private set; } // Indique si le bouton est cliqué

    public Button(Texture2D texture, Vector2 position) : base(texture, position, false)
    {
        _texture = texture;
        
        // La position donné en paramètre est le centre du bouton
        int width = _texture.Width;
        int height = _texture.Height;
        _destRect = new Rectangle((int)position.X - width / 2, (int)position.Y - height / 2, width*2, height*2);
    }
    
    // Met à jour l'état du bouton
    public void Update(Vector2 scale)
    {
        MouseState currentMouseState = Mouse.GetState();
        Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);  // Position de la souris
        
        // Ajuster la position de la souris en fonction de la caméra et de l'échelle
        Vector2 adjustedMousePosition = new Vector2(mousePosition.X, mousePosition.Y) / scale;
        
        IsClicked = _destRect.Contains(adjustedMousePosition) && currentMouseState.LeftButton == ButtonState.Pressed && _previousMouseState.LeftButton == ButtonState.Released;

        _previousMouseState = currentMouseState;
    }
    
    // Dessine le bouton
    public void Draw()
    {
       Globals.SpriteBatch.Draw(_texture, _destRect, Color.White);
    }
    
}