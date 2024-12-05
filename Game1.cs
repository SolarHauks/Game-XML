using System.Collections.Generic;
using System.Linq;
using JeuVideo.Effects;
using JeuVideo.Enemies;
using JeuVideo.Shop;
using JeuVideo.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo;

// core of the MonoGame project, with several critical sections necessary for the game to run:

// heart of the MonoGame project
public class Game1 : Game
{
    // attributs : provide easy access to the various components of MonoGame
    private readonly GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _textureAtlas;    // texture pour le tileset
    private Texture2D _hitboxTexture;   // texture de debug servant à afficher les collisions
    private Tile _tile; // classe Tile pour gérer les tiles
    
    private Player _player; // classe Player pour gérer le joueur
    private readonly List<Enemy> _enemies;
    private ShopKeeper _shopKeeper;
    
    private EffectsManager _effectsManager; // classe EffectsManager pour gérer les effets
    
    private readonly Camera _camera; // classe Camera pour gérer la caméra

    // private PauseMenu _pauseMenu;
    private PauseMenu _pauseMenu;
    private Canvas _canvas;
    
    private KeyboardState _previousKeyState, _currentKeyState; // variables pour la pause du jeu
    
    private Bubble _bubble; // classe Bubble pour gérer les bulles de dialogue
    

    // tell the project how to start, and add key variables
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _camera = new Camera(Vector2.Zero);
        _enemies = new List<Enemy>();
        _previousKeyState = Keyboard.GetState();
    }

    // initialize the game upon its startup
    // The Initialize method is called after the constructor but before the main game loop (Update/Draw).
    // This is where you can query any required services and load any non-graphic related content.
    protected override void Initialize()
    {
        // Paramétrage de la fenètre de jeu
        // Taille actuelle : 480 x 320
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 640;
        _graphics.PreferredBackBufferHeight = 320;
        _graphics.ApplyChanges();
        
        _canvas = new Canvas(GraphicsDevice, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        _canvas.SetDestinationRectangle();

        base.Initialize();
    }

    // used to add and remove assets from the running game from the Content project
    // It is called only once per game, within the Initialize method, before the main game loop starts.
    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        Globals.SpriteBatch = _spriteBatch;
        Globals.Content = Content;
        Globals.ScreenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        Globals.GraphicsDevice = GraphicsDevice;
        
        // Texture du menu
        // Texture2D menuTexture = Content.Load<Texture2D>("Assets/GUI/pauseMenu");
        _pauseMenu = new PauseMenu();
        
        // Texture de debug pour les collisions
        Texture2D debugTexture = new Texture2D(GraphicsDevice, 1, 1);
        debugTexture.SetData(new Color[] { new(255, 0, 0, 255) });
        Globals.DebugTexture = debugTexture;
        
        // Effets
        _effectsManager = new EffectsManager();
        _effectsManager.AddEffect("slash");
        
        // Joueur
        Texture2D playerTexture = Content.Load<Texture2D>("Assets/Character/character");
        _player = new Player(playerTexture, new Vector2(160, 80), _effectsManager);
        
        // Shop
        Texture2D shopTexture = Content.Load<Texture2D>("Assets/NPC/shop");
        _shopKeeper = new ShopKeeper(shopTexture, new Vector2(160, 261), _player);
        
        // Boss
        Texture2D bossTexture = Content.Load<Texture2D>("Assets/Enemies/boss");
        Texture2D summonTexture = Content.Load<Texture2D>("Assets/Enemies/summon");
        Boss boss = new(bossTexture, new Vector2(400, 200), 200, _player, summonTexture);
        _enemies.Add(boss);

        // Ennemis
        // Texture2D snakeTexture = Content.Load<Texture2D>("Assets/Enemies/snake");
        // Snake snake = new(snakeTexture, new Vector2(192, 270), 100);
        // _enemies.Add(snake);
        
        // Texture2D ghostTexture = Content.Load<Texture2D>("Assets/Enemies/ghost");
        // Ghost ghost = new(ghostTexture, new Vector2(680, 30), 20, _player);
        // _enemies.Add(ghost);
        
        // Spike
        // Texture2D spikeTexture = Content.Load<Texture2D>("Assets/Enemies/spike");
        // Spike spike = new(spikeTexture, new Vector2(192, 272));
        // _enemies.Add(spike);

        // Texture des tiles
        _textureAtlas = Content.Load<Texture2D>("Assets/Tileset/tileset");
        _hitboxTexture = Content.Load<Texture2D>("Assets/Tileset/collisions");
        
        // Bulle de dialogue
        Texture2D bubbleTexture = Content.Load<Texture2D>("Assets/GUI/bubble");
        SpriteFont font = Content.Load<SpriteFont>("Assets/Fonts/font");
        _bubble = new Bubble(bubbleTexture, font);
        _bubble.SetText("Hello Developers!");
        _bubble.TextColor = Color.Yellow;

        // Tile
        _tile = new(_textureAtlas, _hitboxTexture);
    }

    // called on a regular interval to update the game state, e.g. take player inputs, move ships, or animate entities
    // The Update method is called multiple times per second,
    // and it is used to update your game state (checking for collisions, gathering input, playing audio, etc.).
    protected override void Update(GameTime gameTime)
    {
        
        Globals.GameTime = gameTime;

        _previousKeyState = _currentKeyState;
        _currentKeyState = Keyboard.GetState();

        // Commande de fermeture de la fenêtre
        if (_currentKeyState.IsKeyDown(Keys.Escape))
            Exit();

        // --------------------------------- Freeze de la pause ---------------------------------
        // Commande de pause du jeu
        if (_currentKeyState.IsKeyDown(Keys.P) && !_previousKeyState.IsKeyDown(Keys.P))
            _pauseMenu.IsActive = !_pauseMenu.IsActive;

        if (_pauseMenu.IsActive)
        {
            // Logique du menu
            _pauseMenu.Update(this);
            // On ne fait rien d'autre car le jeu est en pause
            return;
        }
        
        // --------------------------------- Resize de l'écran ---------------------------------

        // Commande de taille d'écran
        if (_currentKeyState.IsKeyDown(Keys.R) && !_previousKeyState.IsKeyDown(Keys.R))
            SetResolution(320, 640);

        if (_currentKeyState.IsKeyDown(Keys.T) && !_previousKeyState.IsKeyDown(Keys.T))
            SetResolution(640, 1280);

        if (_currentKeyState.IsKeyDown(Keys.Y) && !_previousKeyState.IsKeyDown(Keys.Y))
            SetFullScreen();
        
        if (_player.Dead() == false)
        {


            // --------------------------------- Freeze du shop ---------------------------------

            // Commande du shop
            if (_currentKeyState.IsKeyDown(Keys.E) && !_previousKeyState.IsKeyDown(Keys.E))
                _shopKeeper.Interact();

            // Logique du shop
            _shopKeeper.Update();

            // On mets aussi en pause si on interagit avec le shop. Mais ici pas de menu de pause
            if (_shopKeeper.IsPaused)
            {
                return;
            }

            // --------------------------------- Logique du jeu ---------------------------------

            // Logique de la caméra
            _camera.Follow(_player.Rect, new Vector2(_canvas.Target.Width, _canvas.Target.Height));

            // Logique des ennemis
            foreach (Enemy enemy in _enemies.ToList())
            {
                enemy.Update(_tile.GetCollisions());
                if (enemy.Health <= 0 && !(enemy is Boss boss && boss.CurrentState == Boss.BossState.Dying))
                {
                    _enemies.Remove(enemy);
                    _player.AddMoney(enemy,5);
                }
            }

            // Logique des effets
            _effectsManager.Update();

            base.Update(gameTime);
        }
        
        // Logique du joueur
        _player.Update(_tile.GetCollisions(), _enemies, _shopKeeper);
    }
    private void SetResolution(int height, int width)
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferHeight = height;
        _graphics.PreferredBackBufferWidth = width;
        _graphics.ApplyChanges();
        _canvas.SetDestinationRectangle();
    }

    private void SetFullScreen()
    {
        _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
        _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
        _graphics.IsFullScreen = true;
        _graphics.ApplyChanges();
        _canvas.SetDestinationRectangle();
    }

    // called on a regular interval to take the current game state and draw the game entities to the screen
    protected override void Draw(GameTime gameTime)
    {
        //GraphicsDevice.Clear(Color.CornflowerBlue);
        _canvas.Activate();

        Vector2 offset = _camera.Position;  // Offset lié à la caméra
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        
        if(_player.Dead())
        {
            
            _spriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Assets/Fonts/font"), "Game Over", new Vector2(300, 90), Color.Red);
            _spriteBatch.DrawString(Globals.Content.Load<SpriteFont>("Assets/Fonts/font"), "Appuyer sur A pour reapparaitre", new Vector2(220, 100), Color.Red);

        }
        else
        {

            _tile.Draw(_spriteBatch, offset); // dessin des tiles

            foreach (Enemy enemy in _enemies)
            {
                enemy.Draw(offset); // dessin des ennemis
            }

            _player.Draw(offset); // dessin du joueur

            _shopKeeper.Draw(offset); // dessin du shop

            _effectsManager.Draw(offset); // dessin des effets

            if (_bubble.Visible)
                _bubble.Draw(); // dessin de la bulle de dialogue

           
        }
        _pauseMenu.Draw(); // dessin du menu

        _spriteBatch.End();

        _canvas.Draw(_spriteBatch);
    }
    
}