
using Microsoft.Xna.Framework;

namespace Slicer.App.Models;

public struct Ray2
{
	public Ray2(Vector2 origin, Vector2 end, float length)
	{
		Origin = origin;

		Direction = Vector2.Normalize(end - origin);

		End = origin + length * Direction;
	}

	public Vector2 Origin { get; init; }

	public Vector2 End { get; init; }

	public Vector2 Direction { get;}
}