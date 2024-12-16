using System;
using System.Collections.Generic;
using System.Linq;
using JeuVideo.Effects;
using JeuVideo.Enemies;
using JeuVideo.Shop;
using JeuVideo.Character;
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

    private Texture2D _textureAtlas; // texture pour le tileset
    private Texture2D _hitboxTexture; // texture de debug servant à afficher les collisions
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
    private Timer _timer; // classe Timer pour gérer le timer


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
        _pauseMenu = new PauseMenu();
        _timer = new Timer();

        // Texture de debug pour afficher les hitbox
        Texture2D debugTexture = new Texture2D(GraphicsDevice, 1, 1);
        debugTexture.SetData(new Color[] { new(255, 0, 0, 255) });
        Globals.DebugTexture = debugTexture;

        // Effets
        _effectsManager = new EffectsManager();
        _effectsManager.AddEffect("slash");

        // Bulle de dialogue
        Texture2D bubbleTexture = Content.Load<Texture2D>("Assets/GUI/bubble");
        SpriteFont font = Content.Load<SpriteFont>("Assets/Fonts/font");
        _bubble = new Bubble(bubbleTexture, font);
        _bubble.SetText("Hello Developers!");
        _bubble.TextColor = Color.Yellow;

        // Joueur
        // Il est chargé ici et pas dans EntitiesProcessed car on a besoin qu'il soit initialisé avant les ennemis
        string folderPath = "../../../Content/Data/Stats/Player/";    // Chemin des fichiers de stats des ennemis
        string schemaNamespace = "https://www.univ-grenoble-alpes.fr/jeu/character"; // Namespace du schéma XSD
        string xsdFilePath = "../../../Content/Data/Stats/Player/character.xsd"; // Chemin du fichier XSD
        XmlUtils.ValidateXmlFiles(folderPath, schemaNamespace, xsdFilePath); // Validation des fichiers XML
        
        _player = new XmlManager<Player>().Load("../../../Content/Data/Stats/Player/character.xml");
        _player.Load(new Vector2(100, 100), _effectsManager);

        // Tile
        _textureAtlas = Content.Load<Texture2D>("Assets/Tileset/tileset"); // Texture du terrain
        _hitboxTexture =
            Content.Load<Texture2D>("Assets/Tileset/collisions"); // Texture de debug pour les collisions et les entités

        _tile = new(_textureAtlas, _hitboxTexture);
        EntitiesProcessed(_tile.Entities); // Chargement des entités
    }

    private void EntitiesProcessed(Dictionary<Vector2, int> entities)
    {
        string folderPath = "../../../Content/Data/Stats/Ennemies/";    // Chemin des fichiers de stats des ennemis
        string schemaNamespace = "https://www.univ-grenoble-alpes.fr/jeu/ennemi"; // Namespace du schéma XSD
        string xsdFilePath = "../../../Content/Data/Stats/Ennemies/ennemi.xsd"; // Chemin du fichier XSD
        XmlUtils.ValidateXmlFiles(folderPath, schemaNamespace, xsdFilePath); // Validation des fichiers XML
        
        // Loaders des entités pour la désérialisation
        XmlManager<Ghost> ghostLoader = new XmlManager<Ghost>();
        XmlManager<Snake> snakeLoader = new XmlManager<Snake>();
        XmlManager<Spike> spikeLoader = new XmlManager<Spike>();
        XmlManager<Boss> bossLoader = new XmlManager<Boss>();

        int collisionTilesetThreshold = _tile.CollisionTilesetThreshold; // Décalage des valeurs des tiles d'entités

        foreach (KeyValuePair<Vector2, int> entity in entities)
        {
            Vector2 position = entity.Key * 16; // Position de l'entité
            switch (entity.Value - collisionTilesetThreshold)
            {
                case 2:
                    position.Y += 5; // Décalage vertical pour le shopkeeper
                    _shopKeeper = new ShopKeeper(position, _player);
                    break;
                case 3:
                    Spike spike = spikeLoader.Load(folderPath + "spike.xml");
                    spike.Load(position);
                    _enemies.Add(spike);
                    break;
                case 4:
                    Snake snake = snakeLoader.Load(folderPath + "snake.xml");
                    snake.Load(position);
                    _enemies.Add(snake);
                    break;
                case 5:
                    Ghost ghost = ghostLoader.Load(folderPath + "ghost.xml");
                    ghost.Load(position, _player);
                    _enemies.Add(ghost);
                    break;
                case 6:
                    Boss boss = bossLoader.Load(folderPath + "boss.xml");
                    boss.Load(position, _player);
                    _enemies.Add(boss);
                    break;
                default:
                    Console.Error.WriteLine("Entity not recognized : " + (entity.Value - collisionTilesetThreshold));
                    break;
            }
        }
    }

    // called on a regular interval to update the game state, e.g. take player inputs, move ships, or animate entities
    // The Update method is called multiple times per second,
    // and it is used to update your game state (checking for collisions, gathering input, playing audio, etc.).
    protected override void Update(GameTime gameTime)
    {
        //que le joueur soit mort ou non, on update le temps et tout le reste 
        Globals.GameTime = gameTime;
        Globals.Scale = _canvas.MenuScale;

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

        // --------------------------------- Logique du jeu ---------------------------------

        _timer.Update(); // Update du timer

        if (!_player.IsDead) // Si le joueur n'est pas mort
        {
            // --------------------------------- Freeze du shop ---------------------------------

            // Commande du shop
            if (_currentKeyState.IsKeyDown(Keys.E) && !_previousKeyState.IsKeyDown(Keys.E))
                _shopKeeper.Interact();

            // Logique du shop
            _shopKeeper.Update(_tile.Collisions);

            // On mets aussi en pause si on interagit avec le shop. Mais ici pas de menu de pause
            if (_shopKeeper.IsPaused) return;

            // Logique de la caméra
            _camera.Follow(_player.Rect, new Vector2(_canvas.Target.Width, _canvas.Target.Height));

            // Logique des ennemis
            foreach (Enemy enemy in _enemies.ToList())
            {
                enemy.Update(_tile.Collisions);
                if (enemy.Health <= 0 && !(enemy is Boss boss && boss.CurrentState == Boss.BossState.Dying))
                {
                    _enemies.Remove(enemy);
                    _player.ResourceManager.GoldCounter.AddMoney(5);

                    if (enemy is Boss) _timer.Stop(); // On stop le timer à la mort du boss
                }
            }

            // Logique des effets
            _effectsManager.Update();

            base.Update(gameTime);
        }

        // Logique du joueur
        _player.Update(_tile.Collisions, _enemies);
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
        _canvas.Activate();

        Vector2 offset = _camera.Position; // Offset lié à la caméra

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        if (_player.IsDead) // Si le joueur est mort
        {
            GraphicsDevice.Clear(Color.Black); // Met la couleur du fond en noir pour le game over

            SpriteFont font = Globals.Content.Load<SpriteFont>("Assets/Fonts/font");
            Color color = Color.Red;

            _spriteBatch.DrawString(font, "Game Over", new Vector2(300, 90), color); // Affiche "Game Over" en rouge
            _spriteBatch.DrawString(font, "Appuyer sur A pour reapparaitre", new Vector2(220, 100),
                color); // Affiche "Appuyer sur A pour reapparaitre" en rouge
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

        _timer.Draw(); // dessin du timer

        _spriteBatch.End();

        _canvas.Draw(_spriteBatch);
    }
}