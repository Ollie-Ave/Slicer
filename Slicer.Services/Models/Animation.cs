namespace Slicer.App.Models;

public record Animation
{
	public required string Texture { get; init; }

	public required AnimationMetaData MetaData { get; init; }
}