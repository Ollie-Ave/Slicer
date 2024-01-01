using Microsoft.Xna.Framework;
using Slicer.App.Models;

namespace Slicer.App.Interfaces;

public interface IPhysicsHandlerService
{
	Vector2 Velocity { get; }

	Vector2 Position { get; }

	Vector2 HitBoxPosition { get; }

	Rectangle HitBox { get; }

	public Dictionary<string, Force> Forces { get; }

	bool IsTouchingGround { get; }

	void HandlePhysicsState(GameTime gameTime);

	void AddForce(string forceName, Force force);

	void SetForce(string forceName, Force  force);

	Vector2  GetNextPosition();

}