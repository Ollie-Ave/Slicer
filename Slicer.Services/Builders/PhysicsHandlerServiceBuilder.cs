using Microsoft.Xna.Framework;
using Slicer.App.Interfaces;
using Slicer.App.Services;

namespace Slicer.App.Builders;

internal class PhysicsHandlerServiceBuilder : IPhysicsHandlerServiceBuilder
{
    public IPhysicsHandlerService Build(Vector2 initialPosition, Rectangle hitBoxDimensions, int spriteScaling)
    {
		  return new PhysicsHandlerService(initialPosition, hitBoxDimensions, spriteScaling);
    }
}
