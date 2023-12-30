using System.Reflection;

namespace Slicer.App.Models;

public record AnimationState
{
	public int CurrentFrame { get; set; }

	public  int TimeSinceLastFrame { get; set; }
}