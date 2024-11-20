using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JeuVideo.Effects;

public class EffectsManager
{
    private readonly Dictionary<string, Effect> _effects = new(); 

    // Ajout d'un effet à la liste
    public void AddEffect(string effectName)
    {
        Effect newEffect = new Effect(effectName);
        _effects.Add(effectName, newEffect);
    }
    
    // Jouer un effet
    public void PlayEffect(string effectName, Vector2 position, int direction)
    {
        _effects[effectName].Play(position, direction);
    }
    
    // Mettre à jour les effets
    public void Update()
    {
        foreach (Effect effect in _effects.Values)
        {
            effect.Update();
        }
    }
    
    // Dessiner les effets
    public void Draw(Vector2 offset)
    {
        foreach (Effect effect in _effects.Values)
        {
            effect.Draw(offset);
        }
    }
}