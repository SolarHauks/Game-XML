using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Animation;

public class AnimationManager
{
    private int _numColumns;    // Nombre de frame par ligne
    private Vector2 _size;  // Taille d'une frame d'animation
    private string _currentAnimation;   // Animation actuelle
    
    private readonly Dictionary<string, Animation> _animations; // Dictoinnaire des animations
    
    private const string XmlFilePathPrefix = "../../../Content/Assets/Animation/";
    
    public AnimationManager(Texture2D spriteSheet)
    {
        _animations = new Dictionary<string, Animation>();
        LoadData(spriteSheet);
    }

    private void LoadData(Texture2D spriteSheet)
    {
        string dataFileName = GetFileName(spriteSheet) + ".xml";
        string xmlFilePath = XmlFilePathPrefix + dataFileName;
        
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlFilePath);
        
        // Validation du fichier XML
        // Cela permet d'etre sur que les noeuds qu'on va utiliser après ne sont pas nul
        // sans avoir besoin de faire des vérifications
        string schemaNamespace = "https://www.univ-grenoble-alpes.fr/jeu/animations";
        string xsdFilePath = "../../../Content/Assets/Animation/animSchema.xsd";
        XmlUtils.ValidateXmlFile(schemaNamespace, xsdFilePath, xmlFilePath);
        
        XmlNode paramNode = doc.DocumentElement; // Sélectionne la racine du document XML

        _numColumns = int.Parse(paramNode.Attributes["nbCol"].Value);
        _size = new Vector2(int.Parse(paramNode.Attributes["largeur"].Value), int.Parse(paramNode.Attributes["hauteur"].Value));
        
        XmlNodeList animationNodes = doc.SelectNodes("//animation"); // Sélectionne la racine du document XML
        
        foreach (XmlNode animationNode in animationNodes)
        {
            string nom = animationNode.Attributes["name"].Value;
            int numFrames = animationNode.SelectNodes("frame").Count;
            string type = animationNode.Attributes["type"].Value;

            // Vitesse de l'animation, 15 par défaut
            int speed = animationNode.Attributes["speed"] != null ? int.Parse(animationNode.Attributes["speed"].Value) : 15;
            
            int[] frames = GetFramesArray(numFrames, animationNode);
            
            if (type == "continu")
            {
                _animations.Add(nom, new Animation(frames, AnimationType.Continuous, speed));
            }
            else if (type == "ponctuel")
            {
                _animations.Add(nom, new Animation(frames, AnimationType.OneTime, speed));
            }
            else
            {
                Console.WriteLine("Erreur : le type d'animation " + type + " n'existe pas");
            }
        }
            
    }

    private string GetFileName(Texture2D spriteSheet)
    {
        // spriteSheet.ToString() renvoit un truc du style "Assets/Character/character"
        // On utilise l'indexation à partir de la fin pour obtenir le dernier segment
        return spriteSheet.ToString().Split('/')[^1]; 
    }
    
    private int[] GetFramesArray(int numFrames, XmlNode animationNode)
    {
        int[] frames = new int[numFrames];
        for (int i = 0; i < numFrames; i++)
        {
            frames[i] = int.Parse(animationNode.SelectNodes("frame")[i].Attributes["numFrame"].Value);
        }
        return frames;
    }

    public Vector2 GetSize()
    {
        return _size;
    }
    
    public string GetCurrentAnimation()
    {
        return _currentAnimation;
    }
    
    public bool IsPlaying()
    {
        return _animations[_currentAnimation].IsPlaying;
    }

    public Rectangle GetSourceRectangle()
    {
        int currentFrame = _animations[_currentAnimation].GetNextFrame();
        
        int x = currentFrame % _numColumns * (int)_size.X;
        int y = currentFrame / _numColumns * (int)_size.Y;
        
        return new Rectangle(x, y, (int)_size.X, (int)_size.Y);
    }
    
    public void Update()
    {
        _animations[_currentAnimation].Update();
    }
    
    public void SetAnimation(string anim)
    {
        if (_animations.TryGetValue(anim, out Animation value))
        {
            _currentAnimation = anim;
            if (value.Type == AnimationType.OneTime)
            {
                value.IsPlaying = true;
            }
        }
        else
        {
            Console.WriteLine("Erreur : l'animation " + anim + " n'existe pas");   
        }
    }
    
}