namespace CircuitSim.Circuitry;

public class FuseModel(string id) : RigidComponentModel(id, ComponentType.Fuse)
{
	public required double CurrentRating = 10;
	public bool IsTripped = false;
}