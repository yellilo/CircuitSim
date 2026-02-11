namespace CircuitSim.Circuitry;

public partial class Fuse : RigidComponent
{
	public double CurrentRating => ((FuseModel)Model).CurrentRating;
	public bool IsTripped => ((FuseModel)Model).IsTripped;

	public override double Resistance => IsTripped ? double.PositiveInfinity : 0;
}