using Microsoft.Xna.Framework;
using Slicer.App.Interfaces;
using Slicer.App.Models;

namespace Slicer.App.Services;

internal class PhysicsHandlerService : IPhysicsHandlerService
{
	private const int GravityTerminalVelocity = 25;

	private readonly Rectangle hitBoxDimensions;

	private readonly int spriteScaling;

	public PhysicsHandlerService(Vector2 position, Rectangle hitBoxDimensions, int spriteScaling)
	{
		this.Position = position;
		this.hitBoxDimensions = hitBoxDimensions;
		this.spriteScaling = spriteScaling;
	}

	public Vector2 HitBoxPosition => new(
		Position.X + hitBoxDimensions.X,
		Position.Y + hitBoxDimensions.Y);

	public Vector2 Position { get; private set; }

	public Rectangle HitBox { get; private set; }

	public Dictionary<string, Force> Forces { get; } = [];

	public Vector2 Velocity
	{
		get
		{
			var totalVelocity = new Vector2(
				Forces.Values.Sum(x => x.TerminalVelocity.Positive.HasValue && x.TerminalVelocity.Negative.HasValue
					? Math.Clamp(x.Velocity.X, x.TerminalVelocity.Negative.Value.X, x.TerminalVelocity.Positive.Value.X)
					: x.Velocity.X),
				Forces.Values.Sum(x => x.TerminalVelocity.Negative.HasValue && x.TerminalVelocity.Positive.HasValue
					? Math.Clamp(x.Velocity.Y, x.TerminalVelocity.Negative.Value.Y, x.TerminalVelocity.Positive.Value.Y)
					: x.Velocity.Y));

			return new(
				totalVelocity.X,
				totalVelocity.Y);
		}
	}

	public bool IsTouchingGround => Position.Y + (hitBoxDimensions.Y * spriteScaling) == Constants.FloorHeight;

	public void HandlePhysicsState(GameTime gameTime)
	{
		UpdateHitBoxPosition();

		AddGravityForce();
	}

	public void AddForce(string forceName, Force force)
	{
		if (!Forces.TryAdd(forceName, force))
		{
			var targetForce = Forces[forceName];
			targetForce.Velocity += force.Velocity;

			if (targetForce.TerminalVelocity.Positive.HasValue
				&& targetForce.TerminalVelocity.Negative.HasValue)
			{
				targetForce.Velocity = new System.Numerics.Vector2(
					Math.Clamp(
					targetForce.Velocity.X,
					targetForce.TerminalVelocity.Negative.Value.X,
					targetForce.TerminalVelocity.Positive.Value.X),
					Math.Clamp(
					targetForce.Velocity.Y,
					targetForce.TerminalVelocity.Negative.Value.Y,
					targetForce.TerminalVelocity.Positive.Value.Y));
			}
		}
	}

	public void SetForce(string forceName, Force force)
	{
		if (!Forces.TryAdd(forceName, force))
		{
			var targetForce = Forces[forceName];
			targetForce.Velocity = force.Velocity;

			if (targetForce.TerminalVelocity.Positive.HasValue
				&& targetForce.TerminalVelocity.Negative.HasValue)
			{
				targetForce.Velocity = new System.Numerics.Vector2(
					Math.Clamp(
					targetForce.Velocity.X,
					targetForce.TerminalVelocity.Negative.Value.X,
					targetForce.TerminalVelocity.Positive.Value.X),
					Math.Clamp(
					targetForce.Velocity.Y,
					targetForce.TerminalVelocity.Negative.Value.Y,
					targetForce.TerminalVelocity.Positive.Value.Y));
			}
		}
	}

	public Vector2 GetNextPosition()
	{
		Position += Velocity;
		Position = new(
			Position.X,
			Math.Clamp(Position.Y, 0, Constants.FloorHeight - hitBoxDimensions.Y * spriteScaling));

		return Position;
	}

	private void AddGravityForce()
	{
		const string Gravity = "Gravity";

		if (!IsTouchingGround)
		{
			AddForce(Gravity, new ()
			{
				Velocity = new Vector2(0, Constants.BaseGravityForce),
				TerminalVelocity = (new Vector2(0, GravityTerminalVelocity), Vector2.Zero)
			});
		}
		else
		{
			Vector2 newForce = new(
				Velocity.X,
				Math.Min(Velocity.Y, 0));

			SetForce(Gravity, new ()
			{
				Velocity = newForce,
				TerminalVelocity = (new Vector2(0, GravityTerminalVelocity), Vector2.Zero)
			});
		}
	}

	private void UpdateHitBoxPosition()
	{
		HitBox = new Rectangle(
			(int)Position.X + hitBoxDimensions.X,
			(int)Position.Y + hitBoxDimensions.Y,
			hitBoxDimensions.Width,
			hitBoxDimensions.Height);
	}
}