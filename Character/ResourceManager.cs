using System;
using Microsoft.Xna.Framework;

namespace JeuVideo.Character;

public class ResourceManager
{
    private int _maxHealth;
    private int _currentHealth;
    private int _maxMana;
    private int _currentMana;
    private readonly QuantityBar _healthBar;
    private readonly QuantityBar _manaBar;

    public int Health
    {
        get => _currentHealth;
        set {
            _currentHealth = Math.Clamp(value, 0, _maxHealth);
            _healthBar.Set(_currentHealth);
        }
    }

    public int Mana
    {
        get => _currentMana;
        set {
            _currentMana = Math.Clamp(value, 0, _maxMana);
            _manaBar.Set(_currentMana);
        }
    }

    public bool IsDead => _currentHealth <= 0;

    public ResourceManager(int maxHealth, int maxMana)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
        _maxMana = maxMana;
        _currentMana = maxMana;
        _healthBar = new QuantityBar(_maxHealth, Color.Red, new Vector2(10, 10));
        _manaBar = new QuantityBar(_maxMana, Color.Blue, new Vector2(10, 30));
    }

    public void AddMaxHealth(int value)
    {
        _maxHealth = Math.Min(_maxHealth + value, 200);
        Health += Math.Min(Health + value, _maxHealth);
    }

    public void AddMaxMana(int value)
    {
        _maxMana = Math.Min(_maxMana + value, 200);
        Mana += Math.Min(Mana + value, _maxMana);
    }

    public void ResetRessource()
    {
        _maxHealth = 100;
        _currentHealth = _maxHealth;
        
        _maxMana = 100;
        _currentMana = _maxMana;
    }

    public void Draw()
    {
        _healthBar.Draw();
        _manaBar.Draw();
    }
}