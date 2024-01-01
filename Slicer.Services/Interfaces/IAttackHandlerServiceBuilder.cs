namespace Slicer.App.Interfaces;

public interface IAttackHandlerServiceBuilder
{
	IAttackHandlerService Build(int attackCooldownDuration, int attackDuration);
}