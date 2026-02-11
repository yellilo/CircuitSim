namespace CircuitSim.Circuitry;

public partial class Lamp : RigidComponent
{
	public double LuminousIntensity => ((LampModel)Model).LuminousIntensity;

	public override double Resistance => 1;
}