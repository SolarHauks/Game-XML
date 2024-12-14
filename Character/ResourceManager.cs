using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace JeuVideo.Character;

[Serializable]
[XmlRoot("ressource", Namespace = "https://www.univ-grenoble-alpes.fr/jeu/character")]
public class ResourceManager
{
    [XmlElement("health")] public int MaxHealth;
    [XmlIgnore] private int _currentHealth;
    [XmlIgnore] private QuantityBar _healthBar;
    
    [XmlElement("mana")] public int MaxMana;
    [XmlIgnore] private int _currentMana;
    [XmlIgnore] private QuantityBar _manaBar;
    
    [XmlIgnore] public GoldCounter GoldCounter;
    [XmlIgnore] private double _lastRegenTime;
    
    [XmlElement("regenTime")] public float RegenTime;
    [XmlElement("regenAmount")] public int RegenAmount;

    public int Health
    {
        get => _currentHealth;
        set {
            _currentHealth = Math.Clamp(value, 0, MaxHealth);
            _healthBar.Set(_currentHealth);
        }
    }

    public int Mana
    {
        get => _currentMana;
        set {
            _currentMana = Math.Clamp(value, 0, MaxMana);
            _manaBar.Set(_currentMana);
        }
    }

    public bool IsDead => _currentHealth <= 0;

    // On ne note pas cette méthode [OnDeserialized] par soucis de cohérence
    public void Load()
    {
        _currentHealth = MaxHealth;
        _currentMana = MaxMana;
        _healthBar = new QuantityBar(MaxHealth, Color.Red, new Vector2(10, 10));
        _manaBar = new QuantityBar(MaxMana, Color.Blue, new Vector2(10, 30));
        GoldCounter = new GoldCounter(new Vector2(10, 50));
    }

    public void AddMaxHealth(int value)
    {
        MaxHealth = Math.Min(MaxHealth + value, 200);
        Health += Math.Min(Health + value, MaxHealth);
    }

    public void AddMaxMana(int value)
    {
        MaxMana = Math.Min(MaxMana + value, 200);
        Mana += Math.Min(Mana + value, MaxMana);
    }
    
    public void Regen()
    {
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        if (currentTime - _lastRegenTime > RegenTime)
        {
            _lastRegenTime = currentTime;
            Health += RegenAmount;
            Mana += RegenAmount;
        }
    }

    public void ResetRessource()
    {
        MaxHealth = 100;
        _currentHealth = MaxHealth;
        
        MaxMana = 100;
        _currentMana = MaxMana;
        
        GoldCounter.Reset();
    }

    public void Draw()
    {
        _healthBar.Draw();
        _manaBar.Draw();
        GoldCounter.Draw();
    }
}