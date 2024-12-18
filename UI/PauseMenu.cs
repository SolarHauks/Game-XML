using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.UI;

// Menu de pause
public class PauseMenu : Menu
{
    public PauseMenu() : base("pauseMenu")
    {
        // Bouton Start
        Texture2D startButtonTexture = Globals.Content.Load<Texture2D>("Assets/GUI/startButton");
        // Bouton Exit
        Texture2D exitButtonTexture = Globals.Content.Load<Texture2D>("Assets/GUI/exitButton");

        // Position des boutons => position du menu + décalage
        Buttons.Add(new Button(startButtonTexture, new Vector2(ScreenSize.X / 2 - 15, DestRectangle.Y + 25 + 7)));
        Buttons.Add(new Button(exitButtonTexture, new Vector2(ScreenSize.X / 2 - 15, DestRectangle.Y + 25 * 3 + 7)));
    }

    public void Update(Game1 game)
    {
        base.Update();
        
        // Si le bouton Start est cliqué, on désactive le menu
        if (Buttons[0].IsClicked) { IsActive = false; }

        // Si le bouton Exit est cliqué, on quitte le jeu
        if (Buttons[1].IsClicked) { game.Exit(); }
    }
}