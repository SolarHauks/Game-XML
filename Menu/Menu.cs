using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Menu;

public class Menu
{
    // boutons du menu
    private readonly Button _playButton;
    private readonly Button _optionButton;
    private readonly Button _quitButton;
    
    // textures du menu
    private readonly Texture2D _texture;
    private readonly Texture2D _bgTexture;
    
    // Etat du jeu
    public bool IsPaused { get; set; }
    
    // Taille de l'écran
    public Vector2 ScreenSize { get; set; }
    
    // Rectangle source du sprite du menu
    private readonly Rectangle _srcRectangle;

    // Rectangle de destination du sprite du menu
    private Rectangle DestRectangle => new((int)(ScreenSize.X / 2 - 64), (int)(ScreenSize.Y / 2 - 64), 128, 128);

    public Menu(Texture2D texture)
    {
        _texture = texture; // Tileset gui contenant le menu
        
        // Texture du fond gris semi transparent
        _bgTexture = new Texture2D(Globals.GraphicsDevice, 1, 1);
        _bgTexture.SetData(new Color[] { new(0, 0, 0, 128) });

        _srcRectangle = new Rectangle(0, 192, 64, 64);  // Position du menu dans le tileset gui

        IsPaused = false;
        
        ScreenSize = Globals.ScreenSize;
        
        // Position des boutons => position du menu + décalage
        _playButton = new Button(new Vector2(DestRectangle.X + 17*2, DestRectangle.Y + 9*2));
        _quitButton = new Button(new Vector2(DestRectangle.X + 17*2, DestRectangle.Y + 41*2));
    }
    
    public void Update(Game1 game)
    {
        if (!IsPaused) { return; }
        
        _playButton.Update();
        _quitButton.Update();

        if (_playButton.IsClicked)
        {
            // Restart the game
            IsPaused = false;
        }

        if (_quitButton.IsClicked)
        {
            // Exit the game
            game.Exit();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (!IsPaused) return;
        
        // Dessin du fond gris semi transparent
        spriteBatch.Draw(_bgTexture, new Rectangle(0, 0, (int)ScreenSize.X, (int)ScreenSize.Y), Color.White);
            
        // Dessin du menu
        spriteBatch.Draw(
            _texture,
            DestRectangle,
            _srcRectangle,
            Color.White);
    }
}