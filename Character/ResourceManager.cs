using System;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using JeuVideo.UI;

namespace JeuVideo.Character;

// Classe permettant de gérer les ressources du joueur
[Serializable]
[XmlRoot("ressource", Namespace = "https://www.univ-grenoble-alpes.fr/jeu/character")]
public class ResourceManager
{
    [XmlElement("health")] public int MaxHealth;    // Vie maximale du joueur
    [XmlIgnore] private int _currentHealth;       // Vie actuelle du joueur
    [XmlIgnore] private QuantityBar _healthBar;   // Barre de vie
    
    [XmlElement("mana")] public int MaxMana;    // Mana maximal du joueur
    [XmlIgnore] private int _currentMana;    // Mana actuel du joueur
    [XmlIgnore] private QuantityBar _manaBar;   // Barre de mana
    
    [XmlIgnore] public GoldCounter GoldCounter;  // Compteur d'or
    [XmlIgnore] private double _lastRegenTime;  // Dernier temps de régénération
    
    [XmlElement("regenTime")] public float RegenTime;   // Temps entre chaque tic de régénération
    [XmlElement("regenAmount")] public int RegenAmount; // Quantité de régénération par tic

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

    public bool IsDead => _currentHealth <= 0;  // Le joueur est mort si sa vie est inférieure ou égale à 0

    // On ne note pas cette méthode [OnDeserialized] par soucis de cohérence
    public void Load()
    {
        _currentHealth = MaxHealth;
        _currentMana = MaxMana;
        _healthBar = new QuantityBar(MaxHealth, Color.Red, new Vector2(10, 10));
        _manaBar = new QuantityBar(MaxMana, Color.Blue, new Vector2(10, 30));
        GoldCounter = new GoldCounter(new Vector2(10, 50));
    }
    
    // Ajoute de la vie max au joueur avec un cap à 200, utile dans le shop
    public void AddMaxHealth(int value)
    {
        MaxHealth = Math.Min(MaxHealth + value, 200);   // Cap de la vie à 200
        Health += Math.Min(Health + value, MaxHealth);
    }

    // Ajoute du mana max au joueur avec un cap à 200, utile dans le shop
    public void AddMaxMana(int value)
    {
        MaxMana = Math.Min(MaxMana + value, 200);   // Cap du mana à 200
        Mana += Math.Min(Mana + value, MaxMana);
    }
    
    // Régénération du joueur
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

    // Reset des ressources à la mort
    public void ResetRessource()
    {
        MaxHealth = 100;
        _currentHealth = MaxHealth;
        
        MaxMana = 100;
        _currentMana = MaxMana;
        
        GoldCounter.Reset();
    }

    // Dessine les barres de ressources et le compteur d'or
    public void Draw()
    {
        _healthBar.Draw();
        _manaBar.Draw();
        GoldCounter.Draw();
    }
}