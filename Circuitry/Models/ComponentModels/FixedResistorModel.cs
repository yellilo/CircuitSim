namespace CircuitSim.Circuitry;

public class FixedResistorModel(string id) : RigidComponentModel(id, ComponentType.FixedResistor)
{
	public required double Resistance = 0;
}