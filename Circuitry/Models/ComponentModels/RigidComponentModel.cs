using Godot;

namespace CircuitSim.Circuitry;

/// <summary>
/// A component model that has a defined position, orientation and size.
/// This includes every type of component except wires, which may span any size between two junctions.
/// </summary>
public abstract class RigidComponentModel(string id, ComponentType type) : ComponentModel(id, type)
{
	public required Vector2I Position;
	public ComponentOrientation Orientation = ComponentOrientation.Right;
}
