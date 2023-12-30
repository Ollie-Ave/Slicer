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

	private const float BaseGravityForce = 0.5f;

	private const float BaseJumpForce = 12f;

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
		new Animation()
		{
			Texture = "Player/_Jump",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 3,
				NumberOfFrames = 3,
				TimeBetweenFrames = 70,
				Loop = false,
			},
		},
		new Animation()
		{
			Texture = "Player/_JumpFallInbetween",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 2,
				NumberOfFrames = 2,
				TimeBetweenFrames = 70,
				Loop = false,
			},
		},
		new Animation()
		{
			Texture = "Player/_Fall",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 3,
				NumberOfFrames = 3,
				TimeBetweenFrames = 70,
				Loop = false,
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
		var frame = animationHandlerService.LoadAnimationFrames(currentAnimationData.CurrentAnimation.MetaData)[currentAnimationData.AnimationState.CurrentFrame];

		spriteBatch.Draw(texture, position, frame, Color.White, 0, Vector2.Zero, 3, spriteEffects, 0);
	}

	public void UpdateHandler(GameTime gameTime)
	{
		animationHandlerService.HandleAnimationState(gameTime);

		HandleVelocity();
		HandleSpriteDisplayDirection();
		HandleSpriteAnimation();

		position += new Vector2(velocity.X * BaseMovementSpeed, velocity.Y);
	}

	private void HandleSpriteDisplayDirection()
	{
		if (velocity.X > 0)
		{
			spriteEffects = SpriteEffects.None;
		}
		else if (velocity.X < 0)
		{
			spriteEffects = SpriteEffects.FlipHorizontally;
		}
	}

	private void HandleSpriteAnimation()
	{

		if (velocity.Y != 0 && velocity.Y < 0.1f)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Jump");
		}
		else if (velocity.Y > -4f && velocity.Y < 4f && position.Y < 300)
		{
			animationHandlerService.SetCurrentAnimation("Player/_JumpFallInbetween");
		}
		else if (velocity.Y != 0 && velocity.Y > 0.1f)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Fall");
		}
		else if (velocity.X > 0)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Run");
		}
		else if (velocity.X < 0)
		{
			animationHandlerService.SetCurrentAnimation("Player/_Run");
		}
		else
		{
			animationHandlerService.SetCurrentAnimation("Player/_Idle");
		}
	}

	private void HandleVelocity()
	{
		var keyboard = Keyboard.GetState();

		velocity.X = 0;

		if (keyboard.IsKeyDown(Keys.D))
		{
			velocity.X += 1;
		}

		if (keyboard.IsKeyDown(Keys.A))
		{
			velocity.X -= 1;
		}

		if (keyboard.IsKeyDown(Keys.Space) && position.Y > 300)
		{
			velocity.Y -= BaseJumpForce;
		}
		else if (position.Y > 300)
		{
			velocity.Y = 0;
		}
		else
		{
			velocity.Y += BaseGravityForce;
		}
	}
}
