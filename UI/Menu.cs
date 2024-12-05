using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.UI;

public abstract class Menu
{
    private readonly Texture2D _bgTexture;  // textures du menu
    public bool IsActive { get; set; }  // Etat du menu. Active -> affiché et logique qui tourne
    protected readonly Vector2 ScreenSize;   // Taille de l'écran
    protected readonly List<Button> Buttons; // Liste des boutons du menu

    // Rectangle de destination du sprite du menu
    protected Rectangle DestRectangle => new((int)(ScreenSize.X / 2 - 64), (int)(ScreenSize.Y / 2 - 64), 128, 128);

    protected Menu(string bgTextureName)
    {
        string bgTexturePath = "Assets/GUI/" + bgTextureName;
        _bgTexture = Globals.Content.Load<Texture2D>(bgTexturePath); // Fond du menu
        IsActive = false;   // Menu désactivé par défaut
        ScreenSize = Globals.ScreenSize;
        Buttons = new List<Button>();
    }

    protected void Update()
    {
        if (!IsActive) { return; }  // Si le menu n'est pas actif, on ne le met pas à jour
        
        // Update des boutons
        foreach (var button in Buttons)
        {
            button.Update();
        }
    }

    public void Draw()
    {
        if (!IsActive) return;  // Si le menu n'est pas actif, on ne le dessine pas

        // Dessin du menu
        Globals.SpriteBatch.Draw(
            _bgTexture,
            DestRectangle,
            Color.White);
        
        // Dessin des boutons
        foreach (var button in Buttons)
        {
            button.Draw();
        }
    }
}