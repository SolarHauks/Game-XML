using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo;

// core of the MonoGame project, with several critical sections necessary for the game to run:

// heart of the MonoGame project
public class Game1 : Game
{
    // attributs : provide easy access to the various components of MonoGame
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _textureAtlas;    // texture pour le tileset
    private Texture2D _hitboxTexture;   // texture de debug servant à afficher les collisions
    private Texture2D _rectangleTexture;    // texture de debug pour les collisions. Sert dans Tile.DrawRectHollow
    private Tile _tile; // classe Tile pour gérer les tiles
    
    private Player _player; // classe Player pour gérer le joueur

    private List<Enemy> _enemies;
    
    private Camera _camera; // classe Camera pour gérer la caméra


    // tell the project how to start, and add key variables
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _camera = new Camera(Vector2.Zero);
        _enemies = new List<Enemy>();
    }

    // initialize the game upon its startup
    // The Initialize method is called after the constructor but before the main game loop (Update/Draw).
    // This is where you can query any required services and load any non-graphic related content.
    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        // Paramétrage de la fenètre de jeu
        // Taille actuelle : 480 x 320
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 854;
        _graphics.PreferredBackBufferHeight = 480;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    // used to add and remove assets from the running game from the Content project
    // It is called only once per game, within the Initialize method, before the main game loop starts.
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        // Joueur
        Texture2D playerTexture = Content.Load<Texture2D>("Assets/Character/character");
        Vector2 screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        _player = new Player(playerTexture, new Vector2(160, 80), screenSize);
        
        // Ennemis
        Texture2D snakeTexture = Content.Load<Texture2D>("Assets/Enemies/snake");
        Enemy snake = new(snakeTexture, new Vector2(192, 80), 100);
        _enemies.Add(snake);

        // Texture des tiles
        _textureAtlas = Content.Load<Texture2D>("Assets/Tileset/tileset");
        _hitboxTexture = Content.Load<Texture2D>("Assets/Tileset/collisions");

        // Texture de debug pour les collisions
        _rectangleTexture = new Texture2D(GraphicsDevice, 1, 1);
        _rectangleTexture.SetData(new Color[] { new(255, 0, 0, 255) });

        // Tile
        _tile = new(_textureAtlas, _hitboxTexture, _rectangleTexture);
    }

    // called on a regular interval to update the game state, e.g. take player inputs, move ships, or animate entities
    // The Update method is called multiple times per second,
    // and it is used to update your game state (checking for collisions, gathering input, playing audio, etc.).
    protected override void Update(GameTime gameTime)
    {
        // Commande de fermeture de la fenêtre
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        // Logique du joueur
        _player.Update(Keyboard.GetState(), _tile, gameTime, _enemies);
        
        // Logique des ennemis
        foreach (Enemy enemy in _enemies)
        {
            enemy.Update(_tile, gameTime);
        }
        
        // Vérifie et supprime les ennemis avec 0 point de vie
        RemoveDeadEnemies();
        
        // Logique de la caméra
        // A décommenter si on veut utiliser la caméra
        // _camera.Follow(_player.Rect, new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight));

        base.Update(gameTime);
    }

    private void RemoveDeadEnemies()
    {
        foreach (Enemy enemy in _enemies.ToList())
        {
            if (enemy.Health <= 0)
            {
                _enemies.Remove(enemy);
            }
        }
    }

    // called on a regular interval to take the current game state and draw the game entities to the screen
    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        Vector2 offset = _camera.Position;  // Offset lié à la caméra

        // TODO: Add your drawing code here
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _tile.Draw(_spriteBatch, offset);   // dessin des tiles
            
            foreach (Enemy enemy in _enemies)
            {
                enemy.Draw(_spriteBatch, offset);   // dessin des ennemis
            }
            
            _player.Draw(_spriteBatch, offset); // dessin du joueur

        _spriteBatch.End();

        base.Draw(gameTime);
    }
    
}