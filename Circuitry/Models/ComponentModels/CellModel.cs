namespace CircuitSim.Circuitry;

public class CellModel(string id) : RigidComponentModel(id, ComponentType.Cell)
{
	public required double Resistance = 0;
	public required double EMF = 10;
}
