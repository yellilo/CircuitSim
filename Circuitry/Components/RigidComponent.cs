namespace CircuitSim.Circuitry;

public abstract partial class RigidComponent : Component
{
	public override void _Process(double delta)
	{
		Position = ((RigidComponentModel)Model).Position;
	}
}
