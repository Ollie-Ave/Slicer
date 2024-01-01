namespace Slicer.App.Accessors;

public static class GameEnvironment
{
	public static bool IsDebugMode => Environment.GetEnvironmentVariable("DEBUG") == "true";
}