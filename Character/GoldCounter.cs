using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Character;

public class GoldCounter(Vector2 position)
{
    private readonly Texture2D _coinTexture = Globals.Content.Load<Texture2D>("Assets/GUI/coin");
    private int _money;
    private readonly SpriteFont _font = Globals.Content.Load<SpriteFont>("Assets/Fonts/font");

    public int Money
    {
        get => _money;
        private set => _money = value >= 0 ? value : _money;
    }

    public void AddMoney(int value) => Money += value;

    public void RemoveMoney(int value) => Money -= value;
    
    public void Reset() => Money = 0;
    
    public void Draw()
    {
        string text = Money.ToString();
        Vector2 textPosition = position + new Vector2(20, 0);
        
        Globals.SpriteBatch.Draw(_coinTexture, position, Color.White);
        Globals.SpriteBatch.DrawString(_font, text, textPosition, Color.White);
    }
}