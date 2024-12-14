using System;
using System.Globalization;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo;

public class Timer {
    
    private readonly Texture2D _texture;    // Texture du background du timer
    private readonly SpriteFont _font;  // Police pour le texte
    private readonly Vector2 _textPosition;     // Position du texte
    private string _text;   // Texte à afficher
    private double _time;   // Temps du timer
    private readonly Rectangle _destRect;    // Rectangle de destination pour le background
    private bool _active = true;    // Timer actif ou non

    public Timer()
    {
        // On réutilise la texture du shopMenu pour le timer, par soucis de simplicité
        _texture = Globals.Content.Load<Texture2D>("Assets/GUI/shopMenu");
        
        _font = Globals.Content.Load<SpriteFont>("Assets/Fonts/font");
        _text = "00:00:00";    // Texte de base uniquement là pour centrer le texte dans le menu
        
        // Set des positions
        Vector2 screenSize = Globals.ScreenSize;
        Vector2 position = new Vector2(screenSize.X - _texture.Width * 3, 0);
        _textPosition = new(position.X + (_texture.Width*3 - _font.MeasureString(_text).X) / 2, position.Y + (_texture.Height - _font.MeasureString(_text).Y) / 2);
        _destRect = new Rectangle((int)position.X, (int)position.Y, _texture.Width * 3, _texture.Height);
    }
    
    public void Update()
    {
        if (!_active) return;
        
        _time = Globals.GameTime.TotalGameTime.TotalSeconds;    // On récupère le temps total du jeu
        _text = TimeSpan.FromSeconds(_time).ToString(@"mm\:ss\.ff");    // On le convertit en minutes:secondes:centièmes
    }

    public void Stop()
    {
        _active = false;
        Save();
    }

    private void Save()
    {
        String filePrefix = "../../../Content/Data/Highscore/";
        string xmlFilePath = filePrefix + "score.xml";
        
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlFilePath);
        
        // Création d'un nouvel élément pour le timer
        XmlElement newScore = doc.CreateElement("temps");
        
        // Format du temps en secondes avec 2 décimales
        // CultureInfo.InvariantCulture -> utilisation du point pour les décimales au lieu de virgule,
        // nécessaire pour les transformations XSLT
        newScore.InnerText = _time.ToString("F2", CultureInfo.InvariantCulture);

        // Ajout du nouvel élément au document
        doc.DocumentElement?.AppendChild(newScore);

        // Sauvegarde du document
        doc.Save(xmlFilePath);

        // Première transformation pour trier les scores
        xmlFilePath = filePrefix + "score.xml";
        var result = filePrefix + "sorted_score.xml";
        XmlUtils.XslTransform(xmlFilePath, filePrefix + "sort_score.xslt", result);
        
        // Seconde transformation pour ne garder que le meilleur score
        xmlFilePath = filePrefix + "sorted_score.xml";
        result = filePrefix + "min_score.xml";
        XmlUtils.XslTransform(xmlFilePath, filePrefix + "min_score.xslt", result);
    }
    
    public void Draw()
    {
        Globals.SpriteBatch.Draw(_texture, _destRect, Color.White); // Background
        Globals.SpriteBatch.DrawString(_font, _text, _textPosition, Color.Black);   // Texte
    }
}