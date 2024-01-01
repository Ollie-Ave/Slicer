
using Microsoft.Xna.Framework;

namespace Slicer.App.Interfaces;

public interface IPhysicsHandlerServiceBuilder
{
    IPhysicsHandlerService Build(Vector2 initialPosition, Rectangle hitBoxDimensions, int spriteScaling);
}