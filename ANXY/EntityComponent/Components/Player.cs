﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ANXY.EntityComponent.Components;

public class Player : Component
{
    public enum PlayerState
    {
        Idle,
        Running,
        Jumping,
        DoubleJumping,
        Ducking,
        Falling
    }

    private const int GroundLevel = 400;
    private const int JumpHeight = 150;


    private readonly Vector2 _movementSpeed = new(200, 1000);
    private float _jumpedHeight;
    public BoxCollider BoxCollider;

    private bool isAlive = true;
    private PlayerInputController playerInputController;
    private int windowHeight;

    private readonly int windowWidth;


    /// <summary>
    ///     TODO
    /// </summary>
    public Player(int windowWidth, int windowHeight)
    {
        this.windowWidth = windowWidth;
        this.windowHeight = windowHeight;
    }

    public float WalkingInXVelocity { get; private set; }

    public Vector2 CurrentVelocity { get; private set; }
    public PlayerState MovementState { get; private set; }

    /* TODO implement later
    public bool Crouch()
    {
        return true;
    }
    public bool Dash()
    {
        return true;
    }
    */


    public override void Initialize()
    {
        MovementState = PlayerState.Falling;
    }

    public override void Destroy()
    {
        throw new NotImplementedException();
    }

    // Methods

    public override void Update(GameTime gameTime)
    {
        //input
        var state = Keyboard.GetState();
        var inputDirection = Vector2.Zero;
        var jumpingDirection = Vector2.Zero;

        if (state.IsKeyDown(Keys.D) || state.IsKeyDown(Keys.Right)) inputDirection += new Vector2(1, 0);
        if (state.IsKeyDown(Keys.A) || state.IsKeyDown(Keys.Left)) inputDirection += new Vector2(-1, 0);
        if (state.IsKeyDown(Keys.Space))
        {
            if (MovementState == PlayerState.Idle) MovementState = PlayerState.Jumping;
            inputDirection += Jump(gameTime);
        }


        /*TODO remove
        if (state.IsKeyDown(Keys.W) || state.IsKeyDown(Keys.Up))
        {
            inputDirection += new Vector2(0, -1);
        }
        if (state.IsKeyDown(Keys.S) || state.IsKeyDown(Keys.Down))
        {
            inputDirection += new Vector2(0, 1);
        }*/

        //velocity update
        CurrentVelocity = inputDirection * _movementSpeed;
        CurrentVelocity += ApplyGravity();

        //Direction update
        WalkingInXVelocity = CurrentVelocity.X;

        //ScreenConstraintUpdate
        if ((WalkingInXVelocity > 0 && Entity.Position.X >= windowWidth * 3.0 / 4.0)
            || (WalkingInXVelocity < 0 && Entity.Position.X <= windowWidth * 1.0 / 4.0))
            CurrentVelocity *= new Vector2(0, 1);

        //position update
        Entity.Position += CurrentVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    /// <summary>
    ///     TODO
    /// </summary>
    public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
    }

    public Vector2 Jump(GameTime gameTime)
    {
        if (MovementState != PlayerState.Falling)
        {
            _jumpedHeight += _movementSpeed.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_jumpedHeight >= JumpHeight)
            {
                _jumpedHeight = 0;
                MovementState = PlayerState.Falling;
            }
            else
            {
                return new Vector2(0, -1);
            }
        }

        return new Vector2(0, 0);
    }

    public bool DoubleJump()
    {
        return true;
    }

    public bool Drop()
    {
        return true;
    }

    public bool Die()
    {
        return true;
    }

    private Vector2 ApplyGravity()
    {
        if (Entity.Position.Y < GroundLevel && MovementState != PlayerState.Jumping) return new Vector2(0, 250);
        MovementState = PlayerState.Idle;
        return new Vector2(0, 0);
    }
}