using System;

namespace JeuVideo.Animation;

public enum AnimationType
{
    Continuous,
    OneTime
}

// Classe représentant une animation continue.
public class Animation
{
    private readonly int[] _frames;
    private float _counter;
    private int _activeFrame;
    private readonly float _interval; // Replace with a game constant if necessary
    private readonly int _nbFrames;

    public bool IsPlaying { get; set; }

    public AnimationType Type { get; }

    // Constructeur de la classe ContinuousAnimation.
    // param : frames - Tableau des indices des frames de l'animation dans le tileset.
    public Animation(int[] frames, AnimationType type, int speed)
    {
        _frames = frames;
        Type = type;
        _interval = speed * (1.0f / 60.0f);
        _nbFrames = frames.Length;
        _activeFrame = 0;
        _counter = 0;
        IsPlaying = true;
    }

    // Met à jour l'état de l'animation.
    public void Update()
    {
        if (!IsPlaying) return;

        float deltaTime = (float)Globals.GameTime.ElapsedGameTime.TotalSeconds;
        
        Console.WriteLine(deltaTime);
        Console.WriteLine(_interval);
        
        _counter += deltaTime;
        if (_counter > _interval)
        {
            _counter -= _interval;
            _activeFrame++;
            if (_activeFrame >= _nbFrames)
            {
                _activeFrame = 0;
                if (Type == AnimationType.OneTime)
                {
                    IsPlaying = false;
                }
            }
        }
    }

    // Obtient la prochaine frame de l'animation.
    // retour : L'indice de la prochaine frame dans le tileset.
    public int GetNextFrame()
    {
        return _frames[_activeFrame] - 1;
    }
}