using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Character;

// Sert à gérer l'argent du joueur ainsi que son affichage
public class GoldCounter(Vector2 position)
{
    // Texture de l'icon d'une pièce
    private readonly Texture2D _coinTexture = Globals.Content.Load<Texture2D>("Assets/GUI/coin");
    // Police du texte
    private readonly SpriteFont _font = Globals.Content.Load<SpriteFont>("Assets/Fonts/font");
    private int _money; // L'argent du joueur
    
    public int Money
    {
        get => _money;
        private set => _money = value >= 0 ? value : _money;
    }

    public void AddMoney(int value) => Money += value;  // Ajoute de l'argent
    
    public void Reset() => Money = 0;   // Remet l'argent à 0, sert au moment de la mort
    
    // Affiche l'argent du joueur
    public void Draw()
    {
        string text = Money.ToString();
        Vector2 textPosition = position + new Vector2(20, 0);
        
        Globals.SpriteBatch.Draw(_coinTexture, position, Color.White);
        Globals.SpriteBatch.DrawString(_font, text, textPosition, Color.White);
    }
}