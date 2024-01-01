using Slicer.App.Models;

namespace Slicer.App.Interfaces;

public interface IAnimationHandlerServiceBuilder
{
	IAnimationHandlerService Build(List<Animation> animations);
}