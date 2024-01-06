using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Slicer.App.Models;

namespace Slicer.App.Entities;

public partial class Goblin
{
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
				TimeBetweenFrames = 70,
				Loop = false,
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
				TimeBetweenFrames = 70,
			},
		},
	];

	public Dictionary<string, Texture2D>? Textures { get; set; }
}