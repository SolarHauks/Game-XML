using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JeuVideo.Effects;

public class EffectsManager
{
    private readonly Dictionary<string, Effect> _effects = new(); 

    public void AddEffect(string effectName)
    {
        Effect newEffect = new Effect(effectName);
        _effects.Add(effectName, newEffect);
    }
    
    public void PlayEffect(string effectName, Vector2 position, int direction)
    {
        _effects[effectName].Play(position, direction);
    }
    
    public void Update()
    {
        foreach (Effect effect in _effects.Values)
        {
            effect.Update();
        }
    }
    
    public void Draw(Vector2 offset)
    {
        foreach (Effect effect in _effects.Values)
        {
            effect.Draw(offset);
        }
    }
}