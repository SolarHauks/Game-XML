using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace JeuVideo.Weapons;

public class WeaponsManager
{
    private readonly Dictionary<string, Weapon> _weapons = new(); 

    // Ajout d'un effet à la liste
    public void AddWeapon(string weaponName)
    {
        Weapon newWeapon = new Weapon();
        _weapons.Add(weaponName, newWeapon);
    }
    
    // Jouer un effet
    public void Fire(string weaponName, Vector2 position, int direction)
    {
        _weapons[weaponName].Fire(position, direction);
    }
    
    // Mettre à jour les effets
    public void Update(Dictionary<Vector2, int> collision, List<Enemy> enemies, Vector2 playerPosition)
    {
        foreach (Weapon weapon in _weapons.Values)
        {
            weapon.Update(collision, enemies, playerPosition);
        }
    }
    
    // Dessiner les effets
    public void Draw(Vector2 offset)
    {
        foreach (Weapon weapon in _weapons.Values)
        {
            weapon.Draw(offset);
        }
    }
}