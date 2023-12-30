using Microsoft.Xna.Framework;

namespace Slicer.App.Models;

public record AnimationMetaData
{
	public required Vector2 FrameSize { get; init; }

	public required int NumberOfFrames { get; init; }

	public required int FramesPerRow { get; init; }

	public required int TimeBetweenFrames { get; init; }
}