using System.Collections.Generic;
using Slicer.App.Interfaces;
using Slicer.App.Models;
using Slicer.App.Services;

namespace Slicer.App.Builders;

internal class AnimationHandlerServiceBuilder : IAnimationHandlerServiceBuilder
{
	public IAnimationHandlerService Build(List<Animation> animations)
	{
		return new AnimationHandlerService(animations);
	}
}
