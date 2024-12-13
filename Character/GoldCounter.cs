using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Character;

public class GoldCounter
{
    private Texture2D _coinTexture;
    private Vector2 _position;
    private int _money;
    private SpriteFont _font;

    public int Money
    {
        get => _money;
        private set => _money = value >= 0 ? value : _money;
    }
    
    public GoldCounter(Vector2 position)
    {
        _coinTexture = Globals.Content.Load<Texture2D>("Assets/GUI/coin");
        _font = Globals.Content.Load<SpriteFont>("Assets/Fonts/font");
        _position = position;
    }
    
    public void AddMoney(int value) => Money += value;

    public void RemoveMoney(int value) => Money -= value;
    
    public void Reset() => Money = 0;
    
    public void Draw()
    {
        string text = Money.ToString();
        Vector2 textPosition = _position + new Vector2(20, 0);
        
        Globals.SpriteBatch.Draw(_coinTexture, _position, Color.White);
        Globals.SpriteBatch.DrawString(_font, text, textPosition, Color.White);
    }
}