using Slicer.App.Interfaces;
using Slicer.App.Services;

namespace Slicer.App.Builders;

internal class AttackHandlerServiceBuilder : IAttackHandlerServiceBuilder
{
	public IAttackHandlerService Build(int attackCooldownDuration, int attackDuration)
	{
		return new AttackHandlerService()
		{
			AttackDuration = attackDuration,
			AttackCooldownDuration = attackCooldownDuration
		};
	}
}
