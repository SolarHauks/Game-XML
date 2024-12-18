using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using JeuVideo.Character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JeuVideo.Enemies;

// Boss final
[Serializable]
[XmlRoot("boss", Namespace = "https://www.univ-grenoble-alpes.fr/jeu/ennemi")]
public class Boss : Enemy
{
    [XmlIgnore] private int _counter; // Compteur pour les animations
    [XmlIgnore] private double _lastAttackTime;     // Temps de la dernière attaque
    [XmlIgnore] private bool _isBloqued;    // Indique si le boss est bloqué
    [XmlIgnore] private Player _player;     // Joueur
    [XmlIgnore] private Texture2D _summonTexture;    // Texture des invocations
    [XmlIgnore] private int _interval;    // Texture des invocations
    
    [XmlElement("specialCooldown")] public int Interval;    // Interval pour le comportement spécial
    [XmlElement("attackCooldown")] public int AttackCooldown;    // Interval pour le comportement spécial
    [XmlElement("moveDistance")] public int Distance;    // Distance minimal du joueur pour lancer l'attaque
    [XmlElement("attackRange")] public int AttackRange;    // Distance d'attaque
    [XmlElement("speed")] public int Speed;  // Vitesse de déplacement
    
    // Attributs des invocations
    [XmlElement("summonSpeed")] public int SummonSpeed;  // Vitesse de déplacement
    [XmlElement("summonTime")] public int SummonTime;  // Vitesse de déplacement
    [XmlElement("summonDamage")] public int SummonDamage;  // Vitesse de déplacement
    
    [XmlIgnore] private readonly List<BossSummon> _summons = new(); // Liste des invocations du boss

    // Enumération des états du boss, pour éviter les conflits dans les animations
    public enum BossState
    {
        Normal,
        Special,
        Dying,
        Attacking
    }

    // On passe par une enum pour éviter que plusieurs états ne puissent etre actifs en meme temps, et par propreté
    [XmlIgnore] public BossState CurrentState { get; private set; }
    
    public void Load(Vector2 position, Player player)
    {
        _player = player;
        
        // Texture des invocations pour le spécial
        _summonTexture = Globals.Content.Load<Texture2D>("Assets/Enemies/summon");
        
        Texture2D texture = Globals.Content.Load<Texture2D>("Assets/Enemies/boss"); // Texture du boss
        base.Load(texture, position);
    }

    // Deplacement horizontal du boss, fait des aller-retours
    protected override void DeplacementHorizontal(double dt)
    {
        // Si le boss est en train de mourir, attaque ou fait son spécial => on ne fait rien
        if (_isBloqued) return;
        
        Velocity.X = Direction * Speed * (float)dt;
        Position.X += Velocity.X;
        // Si on dépasse de 50 pixels de la position de départ, on change de direction
        if (Math.Abs(Position.X - StartPosition.X) > Distance) { Direction *= -1; }
    }
    
    // Deplacement vertical du boss, est soumis à la gravité
    protected override void DeplacementVertical(double dt)
    {
        // Si le boss est en train de mourir, attaque ou fait son spécial => on ne fait rien
        if (_isBloqued) return;
        
        Velocity.Y = 25.0f * (float)dt;   // Gravité
        Position.Y += Velocity.Y;
    }
    
    // Animations du boss
    protected override void Animate(Vector2 velocity)
    {
        if (_isBloqued)
        {
            Bloqued();
        }
        else if (Health <= 0)    // Comportement de mort
        {
            DeathAnim();
        }
        else if (_interval==Interval*60) // Comportement spécial, lancer toutes les Interval secondes
        {
            SpecialAnim();
        }
        // Comportement d'attaque. Ne se fait que si le joueur est à portée
        else if (Vector2.Distance(_player.Position, Position) < AttackRange)
        {
            AttackAnim();
        }
        else  // Reste des comportements
        {
            AnimationManager.SetAnimation("fly");
            _interval++;
        }
    }

    // Boss bloqué, .i.e. en train de faire une action spéciale ou d'attaque
    // Dans ce cas on attend la fin de l'animation en cours avant d'en lancer une autres
    // Permet aux animations ponctuelles de se finir avant de lancer une autre
    private void Bloqued()
    {
        _counter++;
        if (CurrentState == BossState.Dying && _counter > 268 || 
            CurrentState == BossState.Attacking && _counter > 15 * 13 || 
            CurrentState == BossState.Special && _counter > 15 * 12)
        {
            CurrentState = BossState.Normal;
            _isBloqued = false;
            _counter = _interval = 0;
        }
        else if (CurrentState == BossState.Normal)
        {
            Console.WriteLine("Erreur: boss bloqué mais aucun état actif");
        }
    }
    
    private void DeathAnim()
    {
        // Lancement de l'animation de mort, stop des autres comportements
        AnimationManager.SetAnimation("death");
        CurrentState = BossState.Dying;
        _isBloqued = true;
    }

    private void SpecialAnim()
    {
        // Lancement de l'animation spéciale, stop des autres comportements
        AnimationManager.SetAnimation("summon");
        CurrentState = BossState.Special;
        _isBloqued = true;
        Vector2 position = new Vector2(Position.X + 50, Position.Y);
        _summons.Add(new BossSummon(_summonTexture, position, _player.Position, SummonSpeed, SummonTime, SummonDamage));
    }

    private void AttackAnim()
    {
        // Cooldown de l'attaque
        double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
        if (currentTime - _lastAttackTime > AttackCooldown)
        {
            // Lancement de l'animation d'attaque, stop des autres comportements
            AnimationManager.SetAnimation("attack");
            CurrentState = BossState.Attacking;
            _isBloqued = true;
            _lastAttackTime = currentTime;
        }
    }
    
    // Update du boss et des summons invoquées
    public override void Update(Dictionary<Vector2, int> collision)
    {
        base.Update(collision);
        
        // On update les invocations
        foreach (BossSummon summon in _summons.ToList())
        {
            summon.Update(collision);
            summon.CheckCollisionWithPlayer(_player);
            if (!summon.IsAlive) { _summons.Remove(summon); }
        }
    }
    
    // Dessin du boss et des summons invoquées
    public override void Draw(Vector2 offset)
    {
        base.Draw(offset);
        
        // On dessine les invocations
        foreach (BossSummon summon in _summons)
        {
            summon.Draw(offset);
        }
    }
    
}