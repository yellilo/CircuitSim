namespace CircuitSim.Circuitry;

public class LampModel(string id) : RigidComponentModel(id, ComponentType.Lamp)
{
	public required double LuminousIntensity = 10;
}