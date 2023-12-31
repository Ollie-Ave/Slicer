using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slicer.App.Accessors;
using Slicer.App.Constants;
using Slicer.App.Interfaces;
using Slicer.App.Models;

namespace Slicer.App.Entities;

internal class Goblin :IEntity, ITexturedEntity
{
	private const int SpriteScaling = 2;

	private static Vector2 DefaultFrameSize = new(150, 150);

	private static readonly List<Animation> animations =
	[
		new Animation()
		{
			Texture = "Goblin/_Idle",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 4,
				NumberOfFrames = 4,
				TimeBetweenFrames = 100,
			},
		},
		new Animation()
		{
			Texture = "Goblin/_Run",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 8,
				NumberOfFrames = 8,
				TimeBetweenFrames = 70,
			},
		},
		new Animation()
		{
			Texture = "Goblin/_Attack",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 8,
				NumberOfFrames = 8,
				TimeBetweenFrames = 100,
			},
		},
		new Animation()
		{
			Texture = "Goblin/_Death",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 4,
				NumberOfFrames = 4,
				TimeBetweenFrames = 100,
			},
		},
		new Animation()
		{
			Texture = "Goblin/_TakeHit",
			MetaData = new()
			{
				FrameSize = DefaultFrameSize,
				FramesPerRow = 4,
				NumberOfFrames = 4,
				TimeBetweenFrames = 100,
			},
		},
	];

	private readonly IAnimationHandlerService animationHandlerService;
    private readonly IEntityManagerService entityManagerService;
    private SpriteEffects spriteEffects = SpriteEffects.None;

	private Vector2 position = new(0, WorldConstants.FloorHeight);

	public Goblin(IAnimationHandlerServiceBuilder animationHandlerServiceBuilder, IEntityManagerService entityManagerService)
    {
		animationHandlerService = animationHandlerServiceBuilder.Build(animations);
        this.entityManagerService = entityManagerService;
    }

    public string? EntityName { get; set; }

	public Dictionary<string, Texture2D>? Textures { get; set; }


    public void UpdateHandler(GameTime gameTime)
    {
		animationHandlerService.HandleAnimationState(gameTime);

		if (Environment.GetEnvironmentVariable("DEBUG") == "true")
		{
			DrawHitBox();
		}
    }

    public void Draw(SpriteBatch spriteBatch)
    {
		ArgumentNullException.ThrowIfNull(Textures);

		var currentAnimationData = animationHandlerService.GetCurrentAnimationData();
		var texture = Textures[currentAnimationData.CurrentAnimation.Texture];
		var frame = GetCurrentAnimationFrame();

		spriteBatch.Draw(texture, position, frame, Color.White, 0, Vector2.Zero, SpriteScaling, spriteEffects, 0);
    }

    public void LoadTextures()
    {
		var content = ContentManagerAccessor.GetContentManager();

		foreach (var animation in animations)
		{
			Textures ??= [];

			Textures.Add(animation.Texture, content.Load<Texture2D>(animation.Texture));
		}
    }

	private Rectangle GetCurrentAnimationFrame()
	{
		var currentAnimationData = animationHandlerService.GetCurrentAnimationData();

		return animationHandlerService.LoadAnimationFrames(currentAnimationData.CurrentAnimation.MetaData)[currentAnimationData.AnimationState.CurrentFrame];
	}

	private void DrawHitBox()
	{
		var entitySize = GetCurrentAnimationFrame();
		var debugBox2 = entityManagerService.CreateEntity<DebugBox>("Goblin_Hitbox");

		debugBox2.Bounds = new Rectangle((int)position.X, (int)position.Y, entitySize.Width * SpriteScaling, entitySize.Height * SpriteScaling);
	}
}
