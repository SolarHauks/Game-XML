using System;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Animation;

public class AnimationManager
{
    private int _numColumns;
    private Vector2 _size;
    private string _currentAnimation;
    
    private readonly Dictionary<string, Animation> _animations;
    
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
        
        // TODO : On pourrait inclure ici une vérification de l'instance XML vis à vis d'un schéma
        
        XmlNode paramNode = doc.DocumentElement; // Sélectionne la racine du document XML
        
        if (paramNode?.Attributes?["nbCol"]?.Value == null ||
            paramNode.Attributes?["largeur"]?.Value == null ||
            paramNode.Attributes?["hauteur"]?.Value == null)
        {
            Console.WriteLine("Erreur : le tilesetNode n'a pas les attributs requis");
            return;
        }

        _numColumns = int.Parse(paramNode.Attributes["nbCol"].Value);
        _size = new Vector2(int.Parse(paramNode.Attributes["largeur"].Value), int.Parse(paramNode.Attributes["hauteur"].Value));
        
        XmlNodeList animationNodes = doc.SelectNodes("//animation"); // Sélectionne la racine du document XML

        if (animationNodes == null)
        {
            Console.WriteLine("Erreur : le fichier XML ne contient pas de noeud animation");   
        }
        else
        {
            foreach (XmlNode animationNode in animationNodes)
            {
                if (animationNode?.Attributes?["name"]?.Value == null) {
                    Console.WriteLine("Erreur : l'animationNode n'a pas les attributs requis");
                    return;
                }

                string nom = animationNode.Attributes["name"].Value;
                int numFrames = animationNode.SelectNodes("frame").Count;
                string type = animationNode.Attributes["type"].Value;

                int speed = 15; // Valeur par défaut
                
                if (animationNode.Attributes["speed"] != null)
                {
                    speed = int.Parse(animationNode.Attributes["speed"].Value);
                }
                
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