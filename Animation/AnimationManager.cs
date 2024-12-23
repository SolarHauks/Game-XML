using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Animation;

public class AnimationManager
{
    private int _numColumns;    // Nombre de frame par ligne
    private Vector2 _size;  // Taille d'une frame d'animation
    private string _currentAnimation;   // Animation actuelle
    
    private readonly Dictionary<string, Animation> _animations; // Dictionnaire des animations
    
    public AnimationManager(Texture2D spriteSheet)
    {
        _animations = new Dictionary<string, Animation>();
        LoadData(spriteSheet);
    }
    
    // Charge les données d'animation à partir d'un fichier XML
    private void LoadData(Texture2D spriteSheet)
    {
        // Le fichier XML doit être nommé avec le meme nom que la spriteSheet avec l'extension .xml
        // Par exemple, si la spriteSheet s'appelle "character", le fichier XML doit s'appeler "character.xml"
        string dataFileName = GetFileName(spriteSheet) + ".xml";
        string xmlFilePath = "../../../Content/Data/Animation/" + dataFileName;
        
        XmlDocument doc = new XmlDocument();
        doc.Load(xmlFilePath);
        
        // La validation du fichier XML se fait dans Game1 pour que ce ne soit fait qu'une fois
        // et pas à chaque instanciation d'une entité
        
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
            
            var animationType = type == "continu" ? AnimationType.Continuous : AnimationType.OneTime;
            if (Enum.IsDefined(typeof(AnimationType), animationType))
            {
                _animations.Add(nom, new Animation(frames, animationType, speed));
            }
            else
            {
                Console.WriteLine("Erreur : le type d'animation " + type + " n'existe pas");
            }
        }
            
    }

    // Renvoie le nom du fichier de la spriteSheet sans le chemin ni l'extension
    // spriteSheet.ToString() renvoit un truc du style "Assets/Character/character"
    // On utilise l'indexation à partir de la fin pour obtenir le dernier segment
    private string GetFileName(Texture2D spriteSheet) => spriteSheet.ToString().Split('/')[^1]; 
        
    // Renvoie un tableau d'entiers contenant les index des frames de l'animation
    private int[] GetFramesArray(int numFrames, XmlNode animationNode) => animationNode.SelectNodes("frame")
                        .Cast<XmlNode>()
                        .Select(node => int.Parse(node.Attributes["numFrame"].Value))
                        .ToArray();

    // Renvoie la taille d'une frame d'animation
    public Vector2 GetSize() => _size;
    
    // Renvoit la frame actuelle de l'animation
    public string GetCurrentAnimation() => _currentAnimation;
    
    // Renvoie true si l'animation est en cours
    public bool IsPlaying() => _animations[_currentAnimation].IsPlaying;
        
    // Renvoie le rectangle source de la frame actuelle
    // Permet surtout d'afficher
    public Rectangle GetSourceRectangle()
    {
        int currentFrame = _animations[_currentAnimation].GetNextFrame();
        
        int x = currentFrame % _numColumns * (int)_size.X;
        int y = currentFrame / _numColumns * (int)_size.Y;
        
        return new Rectangle(x, y, (int)_size.X, (int)_size.Y);
    }
    
    // Met à jour l'animation
    public void Update() => _animations[_currentAnimation].Update();
    
    // Démarre une animation
    public void SetAnimation(string anim)
    {
        if (_animations.TryGetValue(anim, out Animation value))
        {
            _currentAnimation = anim;
            value.IsPlaying = value.Type == AnimationType.OneTime;  // Set du type d'animation
        }
        else
        {
            Console.WriteLine("Erreur : l'animation " + anim + " n'existe pas");   
        }
    }
    
}