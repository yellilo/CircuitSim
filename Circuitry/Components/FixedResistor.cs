namespace CircuitSim.Circuitry;

public partial class FixedResistor : RigidComponent
{
	public override double Resistance => ((FixedResistorModel)Model).Resistance;
}