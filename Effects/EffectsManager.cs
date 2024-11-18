using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace JeuVideo.Effects;

public class EffectsManager
{
    private readonly List<Effect> _effects = new();

    public void AddEffect(string effectName)
    {
        Effect newEffect = new Effect(effectName);
        _effects.Add(newEffect);
    }
    
    public void PlayEffect(string effectName, Vector2 position, int direction)
    {
        foreach (var effect in _effects.Where(effect => effect.GetName == effectName))
        {
            effect.Play(position, direction);
        }
    }
    
    public void Update()
    {
        foreach (Effect effect in _effects)
        {
            effect.Update();
        }
    }
    
    public void Draw(Vector2 offset)
    {
        foreach (Effect effect in _effects)
        {
            effect.Draw(offset);
        }
    }
}