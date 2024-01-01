using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Slicer.App.Models;

namespace Slicer.App.Entities;

public partial class Player
{
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
			new Animation()
			{
				Texture = "Player/_Attack",
				MetaData = new()
				{
					FrameSize = DefaultFrameSize,
					FramesPerRow = 4,
					NumberOfFrames = 4,
					TimeBetweenFrames = 75,
					Loop = false,
				},
			},
		];

	public Dictionary<string, Texture2D>? Textures { get; set; }
}