using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JeuVideo;

public class Player : GameObject
{
    private readonly GraphicsDeviceManager _graphics;
    private bool _grounded;
    
    private List<Rectangle> _intersections;
    private KeyboardState _prevKeystate;
    
    
    public Player(Texture2D texture, Vector2 position, int size, GraphicsDeviceManager graphics) 
        : base(texture, position, size) {
        Velocity = new();
        _graphics = graphics;
        _grounded = false;
        Direction = -1;
    }
    
    public void Update(KeyboardState keystate, Tile tile, GameTime gameTime) {
        
        // Déplacements
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        
        Velocity.X = 0.0f;
        Velocity.Y += 35.0f * dt;
        
        Velocity.Y = Math.Min(25.0f, Velocity.Y);

        float horizontalSpeed = 300 * dt;
        
        if (keystate.IsKeyDown(Keys.Left)) {
            Velocity.X = -horizontalSpeed;
            Direction = -1;
        }
        
        if (keystate.IsKeyDown(Keys.Right)) {
            Velocity.X = horizontalSpeed;
            Direction = 1;
        }
        
        // Jump
        if (_grounded && keystate.IsKeyDown(Keys.Up) && !_prevKeystate.IsKeyDown(Keys.Up)) {
            Velocity.Y = -600 * dt;
        }
        
        _position.X += (int)Velocity.X;
        _intersections = GetIntersectingTilesHorizontal(_Rect);

        foreach (var rect in _intersections)
        {
            if (tile.Collisions.TryGetValue(new Vector2(rect.X, rect.Y), out _))
            {
                Rectangle collision = new Rectangle(
                    rect.X * 16,
                    rect.Y * 16,
                    16,
                    16
                );

                if (_Rect.Intersects(collision))
                {
                    continue;
                }

                if (Velocity.X > 0.0f)
                {
                    _position.X = collision.Left - _Rect.Width;
                }
                else if (Velocity.X < 0.0f)
                {
                    _position.X = collision.Right;
                }
            }
        }

        _position.Y += (int)Velocity.Y;
        _intersections = GetIntersectingTilesVertical(_Rect);

        _grounded = false;
        foreach (var rect in _intersections)
        {
            if (tile.Collisions.TryGetValue(new Vector2(rect.X, rect.Y), out _))
            {
                Rectangle collision = new Rectangle(
                    rect.X * 16,
                    rect.Y * 16,
                    16,
                    16
                );

                if (!_Rect.Intersects(collision))
                {
                    continue;
                }

                // colliding with the top face
                if (Velocity.Y > 0.0f)
                {
                    _position.Y = collision.Top - _Rect.Height;
                    Velocity.Y = 1.0f; // counter snap to ground
                    _grounded = true;
                }
                else if (Velocity.Y < 0.0f)
                {
                    _position.Y = collision.Bottom;
                }
            }
        }
        
        _prevKeystate = keystate;
        
        // Limites
        CheckLimits();
    }

    private void CheckLimits()
    {
        if (_position.X > _graphics.PreferredBackBufferWidth - Texture.Width / 2.0f)
        {
            _position.X = _graphics.PreferredBackBufferWidth - Texture.Width / 2.0f;
        }
        else if (_position.X < Texture.Width / 2.0f)
        {
            _position.X = Texture.Width / 2.0f;
        }

        if (_position.Y > _graphics.PreferredBackBufferHeight - Texture.Height / 2.0f)
        {
            _position.Y = _graphics.PreferredBackBufferHeight - Texture.Height / 2.0f;
        }
        else if (_position.Y < Texture.Height / 2.0f)
        {
            _position.Y = Texture.Height / 2.0f;
        }
    }
    
    private List<Rectangle> GetIntersectingTilesHorizontal(Rectangle target)
    {
        List<Rectangle> intersections = new();

        int widthInTiles = (target.Width - (target.Width % 16)) / 16;
        int heightInTiles = (target.Height - (target.Height % 16)) / 16;

        for (int x = 0; x <= widthInTiles; x++)
        {
            for (int y = 0; y <= heightInTiles; y++)
            {

                intersections.Add(new Rectangle(
                    (target.X + x * 16) / 16,
                    (target.Y + y * (16 - 1)) / 16,
                    16,
                    16
                ));

            }
        }

        return intersections;
    }

    private List<Rectangle> GetIntersectingTilesVertical(Rectangle target)
    {
        List<Rectangle> intersections = new();

        int widthInTiles = (target.Width - (target.Width % 16)) / 16;
        int heightInTiles = (target.Height - (target.Height % 16)) / 16;

        for (int x = 0; x <= widthInTiles; x++)
        {
            for (int y = 0; y <= heightInTiles; y++)
            {

                intersections.Add(new Rectangle(
                    (target.X + x * (16 - 1)) / 16,
                    (target.Y + y * 16) / 16,
                    16,
                    16
                ));

            }
        }

        return intersections;
    }
}