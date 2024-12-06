using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.UI;

public class PauseMenu : Menu
{
    public PauseMenu() : base("pauseMenu")
    {
        Texture2D startButtonTexture = Globals.Content.Load<Texture2D>("Assets/GUI/startButton");
        Texture2D exitButtonTexture = Globals.Content.Load<Texture2D>("Assets/GUI/exitButton");

        // Position des boutons => position du menu + décalage
        Buttons.Add(new Button(startButtonTexture, new Vector2(ScreenSize.X / 2 - 15, DestRectangle.Y + 25 + 7)));
        Buttons.Add(new Button(exitButtonTexture, new Vector2(ScreenSize.X / 2 - 15, DestRectangle.Y + 25 * 3 + 7)));
    }

    public void Update(Game1 game, Vector2 scale)
    {
        base.Update(scale);
        
        if (Buttons[0].IsClicked) { IsActive = false; }

        if (Buttons[1].IsClicked) { game.Exit(); }
    }
}