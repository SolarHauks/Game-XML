using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Effects;

public class Effect : Sprite
{
    private string GetName { get; } // Nom de l'effet
    
    public Effect(string textureName) : base(Globals.Content.Load<Texture2D>("Assets/Effects/" + textureName), new Vector2(0, -50))
    {
        AnimationManager.SetAnimation(textureName);
        GetName = textureName;
    }

    public void Play(Vector2 position, int direction)
    {
        if (AnimationManager.IsPlaying()) return;   // Si l'effet joue déjà, on ne fait rien
        
        Position = position;    // On met l'effet là où il doit etre joué
        Direction = direction;  // On définit la direction de l'effet
        // On décale l'effet de 16 pixels à droite ou à gauche en fonction de la direction (attaque du joueur)
        Position.X += (Direction == 1 ? 16 : -16);  // Pour l'instant fixé à 16, à adapter par la suite
        AnimationManager.SetAnimation(GetName); // On lance l'animation
    }
    
    public void Update() => AnimationManager.Update();
    
    public new void Draw(Vector2 offset)
    {
        if (AnimationManager.IsPlaying())
        {
            base.Draw(offset);
        }
    }
}