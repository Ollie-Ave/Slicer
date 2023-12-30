using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Slicer.App.Interfaces;
using Slicer.App.Models;

namespace Slicer.App.Entities;

public class Player : IEntity
{
	private const float BaseMovementSpeed = 7.5f;

	private static Vector2 DefaultFrameSize = new(120, 80);

	private static readonly List<Animation> animations =
	[
		new Animation()
		{
			Texture = "Player/_Idle",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 10,
				NumberOfFrames = 10,
				TimeBetweenFrames = 100,
			},
		},
		new Animation()
		{
			Texture = "Player/_Run",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 10,
				NumberOfFrames = 10,
				TimeBetweenFrames = 70,
			},
		},
	];

    private readonly IAnimationHandlerService animationHandlerService;

	private Vector2 velocity = new(0, 0);

	private Vector2 position = new(0, 300);

	private SpriteEffects spriteEffects = SpriteEffects.None;

    public Player(IAnimationHandlerService animationHandlerService)
	{
        this.animationHandlerService = animationHandlerService;

		this.animationHandlerService.RegisterAnimations(animations);
    }

	public ContentManager? ContentManager { get; set; }

	public void Draw(SpriteBatch spriteBatch)
	{
		ArgumentNullException.ThrowIfNull(ContentManager);

		var currentAnimationData = animationHandlerService.GetCurrentAnimationData();
		var texture = ContentManager.Load<Texture2D>(currentAnimationData.CurrentAnimation.Texture);
		var frame = LoadAnimationFrames(currentAnimationData.CurrentAnimation.MetaData)[currentAnimationData.AnimationState.CurrentFrame];

		spriteBatch.Draw(texture, position, frame, Color.White, 0, Vector2.Zero, 3, spriteEffects, 0);
	}

	public void UpdateHandler(GameTime gameTime)
    {
        animationHandlerService.HandleAnimationState(gameTime);

        velocity.X = HandleVelocity();

        HandleSpriteDisplay();

		position.X += velocity.X * BaseMovementSpeed;
    }

    private void HandleSpriteDisplay()
    {
        if (velocity.X > 0)
        {
            animationHandlerService.SetCurrentAnimation("Player/_Run");
            spriteEffects = SpriteEffects.None;
        }
        else if (velocity.X < 0)
        {
            animationHandlerService.SetCurrentAnimation("Player/_Run");
            spriteEffects = SpriteEffects.FlipHorizontally;
        }
        else
        {
            animationHandlerService.SetCurrentAnimation("Player/_Idle");
        }
    }

    private float HandleVelocity()
    {
        var keyboard = Keyboard.GetState();

		Vector2 velocity = new(0, 0);

        if (keyboard.IsKeyDown(Keys.D))
        {
            velocity.X += 1;
        }
        if (keyboard.IsKeyDown(Keys.A))
        {
            velocity.X -= 1;
        }

		return velocity.X;
    }

    private List<Rectangle> LoadAnimationFrames(AnimationMetaData animationMetaData)
	{
		List<Rectangle> frames = [];

		int currentYIndex = 0;
		int currentXIndex = 0;

		for (int i = 0; i < animationMetaData.NumberOfFrames; i++)
		{
			Rectangle frameToAdd = new()
			{
				X = (int)animationMetaData.FrameSize.X * currentXIndex,
				Y = (int)animationMetaData.FrameSize.Y * currentYIndex,
				Width = (int)animationMetaData.FrameSize.X,
				Height = (int)animationMetaData.FrameSize.Y,
			};

			frames.Add(frameToAdd);

			if (currentXIndex == animationMetaData.FramesPerRow)
			{
				currentXIndex = 0;
				currentYIndex++;
			}
			else
			{
				currentXIndex++;
			}
		}

		return frames;
	}
}
