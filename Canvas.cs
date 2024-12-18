using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

// Classe représentant un Canvas pour le rendu graphique.
// Sert à redimensionner le rendu du jeu pour l'adapter à la taille de l'écran.
public class Canvas
{
    private readonly GraphicsDevice _graphicsDevice;
    private Rectangle _destinationRectangle;
    public readonly RenderTarget2D Target;
    public Vector2 MenuScale { get; private set; }  // Échelle pour le menu.

    public Canvas(GraphicsDevice graphicsDevice, int width, int height)
    {
        _graphicsDevice = graphicsDevice;
        Target = new(_graphicsDevice, width, height);
    }

    // Définit le rectangle de destination pour le rendu du canvas.
    public void SetDestinationRectangle()
    {
        var screenSize = _graphicsDevice.PresentationParameters.Bounds;

        float scaleX = (float)screenSize.Width / Target.Width;
        float scaleY = (float)screenSize.Height / Target.Height;
        MenuScale = new Vector2(scaleX, scaleY);
        float scale = Math.Min(scaleX, scaleY);

        int newWidth = (int)(Target.Width * scale);
        int newHeight = (int)(Target.Height * scale);

        int posX = (screenSize.Width - newWidth) / 2;
        int posY = (screenSize.Height - newHeight) / 2;

        _destinationRectangle = new Rectangle(posX, posY, newWidth, newHeight);
    }

    // Active le canvas pour le rendu.
    public void Activate()
    {
        _graphicsDevice.SetRenderTarget(Target);
        _graphicsDevice.Clear(Color.DarkGray);
    }

    // Dessine le contenu du canvas sur l'écran.
    public void Draw(SpriteBatch spriteBatch)
    {
        _graphicsDevice.SetRenderTarget(null);
        spriteBatch.Begin();
        spriteBatch.Draw(Target, _destinationRectangle, Color.White);
        spriteBatch.End();
    }
}