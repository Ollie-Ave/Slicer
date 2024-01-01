
using Microsoft.Xna.Framework;

namespace Slicer.App.Models;

public record Force
{
	public Vector2 Velocity { get; set; }

	public (Vector2? Positive, Vector2? Negative) TerminalVelocity { get; set; }
}