using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using JeuVideo.Effects;
using JeuVideo.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo.Character;

// Représente un joueur dans le jeu.
// Hérite de la classe GameObject.
[Serializable]
[XmlRoot("character", Namespace = "https://www.univ-grenoble-alpes.fr/jeu/character")]
public class Player : GameObject
{
    private bool _grounded; // Si le joueur est au sol
    private KeyboardState _prevKeystate; // Etat du clavier à la frame d'avant
    private AttackManager _attackManager; // Gestionnaire des attaques
    public ResourceManager ResourceManager; // Gestionnaire des ressources

    private double _lastDamageTime; // Temps du dernier dégât

    [XmlElement("hitboxRatio")] public float HitboxRatio;   // Ratio de la hitbox du joueur, textureSize * ratio = hitbox
    
    public bool IsDead => ResourceManager.IsDead;   // Si le joueur est mort
    
    public void Load(Vector2 position, EffectsManager effets)
    {
        Texture2D texture = Globals.Content.Load<Texture2D>("Assets/Character/character");
        base.Load(texture, position, true, HitboxRatio);
        
        // Désérialization des données d'attaques
        _attackManager = new XmlManager<AttackManager>().Load("../../../Content/Data/Stats/Player/attack.xml");
        _attackManager.Load(AnimationManager, effets);
        
        // Désérialization des données de ressources
        ResourceManager = new XmlManager<ResourceManager>().Load("../../../Content/Data/Stats/Player/ressource.xml");
        ResourceManager.Load();
    }

    /// Met à jour l'état du joueur.
    /// param keystate : L'état actuel du clavier.
    /// param tile : Les informations sur les tiles pour la détection des collisions.
    /// param gameTime : Le temps écoulé depuis la dernière frame.
    public void Update(Dictionary<Vector2, int> collision, List<Enemy> enemies)
    {

        KeyboardState keystate = Keyboard.GetState(); // Récupère l'état du clavier (ie : les touches actuellement pressées)

        if(!IsDead){
            
            base.Update(collision); // Met à jour la position du joueur

            if (Position.Y > 1000) ResourceManager.Health = 0;  // Si le joueur tombe dans le vide, il meurt
            
            // Attaque
            if (keystate.IsKeyDown(Keys.C) && !_prevKeystate.IsKeyDown(Keys.C) && _attackManager.CanAttack())
            {
                _attackManager.Attack(enemies, Position, Direction);
            }

            // Attaque Spé
            if (keystate.IsKeyDown(Keys.V) && !_prevKeystate.IsKeyDown(Keys.V) && _attackManager.CanAttack(ResourceManager.Mana, true))
            {
                ResourceManager.Mana -= 20;
                _attackManager.SpecialAttack(enemies, Position, Direction);
            }

            Animate(Velocity); // Gère l'animation du joueur

            TakeDamage(enemies); // Gère les dégâts du joueur

            ResourceManager.Regen(); // Gère la régénération du joueur

            _prevKeystate = keystate; // Sauvegarde l'état du clavier pour la frame suivante
        }
        
        else if (keystate.IsKeyDown(Keys.A))    // Réapparition du joueur (en cas de mort)
        {
            Respawn();
        }
    }

    // On réimplemente la détection verticales des collisions pour le joueur pour integrer le saut
    protected override void CheckCollisionsVertical(Dictionary<Vector2, int> collision)
    {
        _grounded = false;
        List<Rectangle> intersections = GetIntersectingTilesVertical(Rect); // Récupère les tiles intersectés par le joueur

        // Pour chaque tile que le joueur intersect, on vérifit si il y a collision avec une tile du layer 'collisions'
        // Si c'est la cas, on replace le joueur
        foreach (var rect in intersections)
        {
            if (collision.TryGetValue(new Vector2(rect.X, rect.Y), out _))
            {
                Rectangle collisionTile = new Rectangle(
                    rect.X * 16,
                    rect.Y * 16,
                    16,
                    16
                );

                if (!Rect.Intersects(collisionTile)) { continue; }

                // colliding with the top face
                if (Velocity.Y > 0.0f)
                {
                    Position.Y = collisionTile.Top - Rect.Height;
                    Velocity.Y = 1.0f; // counter snap to ground
                    _grounded = true;
                }
                else if (Velocity.Y < 0.0f)
                {
                    Position.Y = collisionTile.Bottom;
                    Velocity.Y = 0.0f;
                }
            }
        }
    }
    
    protected override void DeplacementHorizontal(double dt)
    {
        Velocity.X = 0.0f;  // Reset la vitesse horizontale, supprime l'inertie
        double horizontalSpeed = 250 * dt;   // Vitesse horizontale
        KeyboardState keystate = Keyboard.GetState();
        
        // Déplacements horizontaux
        if (keystate.IsKeyDown(Keys.Left)) {
            Velocity.X = (float)-horizontalSpeed;  // Vitesse horizontale
            Direction = -1; // Direction
        }
        
        if (keystate.IsKeyDown(Keys.Right)) {
            Velocity.X = (float)horizontalSpeed; // Vitesse horizontale
            Direction = 1; // Direction
        }
        
        Position.X += (int)Velocity.X;  // Déplacement horizontal
    }

    protected override void DeplacementVertical(double dt)
    {
        KeyboardState keystate = Keyboard.GetState();
        
        // Gestion des déplacements horizontaux : saut et gravité
        Velocity.Y += 35.0f * (float)dt;   // Gravité
        Velocity.Y = Math.Min(25.0f, Velocity.Y);   // Limite la vitesse de chute
        
        // Si le joueur est au sol et que la touche espace est pressée -> on saute
        // Evite de sauter quand on est dans les airs
        if (_grounded && keystate.IsKeyDown(Keys.Space) && !_prevKeystate.IsKeyDown(Keys.Space)) {
            Velocity.Y = -600 * (float)dt;
        }
        
        Position.Y += (int)Velocity.Y; // Déplacement vertical
    }

    // Gestion des animations du joueur
    protected override void Animate(Vector2 velocity)
    {
        string currentAnim = AnimationManager.GetCurrentAnimation();    // Animation en cours de lecture
        
        // On ne change pas d'animation si on est en train de jouer l'attaque
        if (currentAnim == "slash" && AnimationManager.IsPlaying()) { return; }

        if (_grounded) {
            if (velocity.X != 0 && currentAnim != "run") {
                AnimationManager.SetAnimation("run");
            } else if (velocity.X == 0 && currentAnim != "idle") {
                AnimationManager.SetAnimation("idle");
            }
        } else {
            string newAnim = velocity.Y > 1 ? "fall" : "jump";
            if (currentAnim != newAnim) {
                AnimationManager.SetAnimation(newAnim);
            }
        }
    }
    
    // Gère les dégâts subis par le joueur lorsqu'il entre en collision avec un ennemi.
    // Si on prend des dégats, on a 1 seconde d'invincibilité
    // enemies - Liste des ennemis présents dans le jeu.
    private void TakeDamage(List<Enemy> enemies)
    {
        foreach (Enemy enemy in enemies)
        {
            double currentTime = Globals.GameTime.TotalGameTime.TotalSeconds;
            if (DamageHitbox.Intersects(enemy.DamageHitbox) && (currentTime - _lastDamageTime > 1))
            {
                if (!(enemy is Boss boss) || boss.CurrentState == Boss.BossState.Attacking)
                {
                    ResourceManager.Health -= enemy.DamageDealt;
                    int attackDirection = Position.X < enemy.Rect.X ? -1 : 1;
                    Position.X += attackDirection * 20;
                    _lastDamageTime = Globals.GameTime.TotalGameTime.TotalSeconds;
                }
            }
        }
    }
    
    // Inflige des dégâts au joueur. Utilse uniquement pour le summons qui n'est pas directement dans la liste d'ennemi
    public void TakeDamage(int damage) => ResourceManager.Health -= damage;
    
    // Dessine le joueur à l'écran.
    public override void Draw(Vector2 offset)
    {
        base.Draw(offset);
        ResourceManager.Draw();
    }

    // Réinitialise la position du joueur et ses ressources à la mort.
    private void Respawn()
    {
        Position = new Vector2(20, 10);
        ResourceManager.ResetRessource();
    }
    
}