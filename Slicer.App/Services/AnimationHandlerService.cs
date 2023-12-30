using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Slicer.App.Interfaces;
using Slicer.App.Models;

namespace Slicer.App.Services;

public class AnimationHandlerService : IAnimationHandlerService
{
	private static readonly AnimationState animationState = new();

	private List<Animation> animations = new();

	private Animation? currentAnimation;

    public void RegisterAnimations(List<Animation> animations)
    {
		this.animations = animations;
		this.currentAnimation = animations[0];
	}

	public (Animation CurrentAnimation, AnimationState AnimationState) GetCurrentAnimationData()
	{
		ArgumentNullException.ThrowIfNull(currentAnimation);

		return (currentAnimation, animationState);
	}

	public void HandleAnimationState(GameTime gameTime)
	{
		ArgumentNullException.ThrowIfNull(currentAnimation);

		if (animationState.TimeSinceLastFrame > currentAnimation.MetaData.TimeBetweenFrames)
		{

			if (animationState.CurrentFrame == currentAnimation.MetaData.NumberOfFrames - 1)
			{
				if (currentAnimation.MetaData.Loop)
				{
					animationState.CurrentFrame = 0;
				}
			}
			else
			{
				animationState.CurrentFrame++;
			}

			animationState.TimeSinceLastFrame = 0;
		}
		else
		{
			animationState.TimeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
		}
	}

    public void SetCurrentAnimation(string animationName)
    {
		ArgumentNullException.ThrowIfNull(currentAnimation);

		if (currentAnimation.Texture == animationName)
		{
			return;
		}

		animationState.CurrentFrame = 0;
		animationState.TimeSinceLastFrame = 0;

		currentAnimation = animations.Single(x => x.Texture == animationName);
    }

    public List<Rectangle> LoadAnimationFrames(AnimationMetaData animationMetaData)
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