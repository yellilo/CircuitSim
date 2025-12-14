using Godot;

namespace CircuitSim.Circuitry.Models;

public abstract class ComponentModel(string id, CircuitComponentType type) : Model(id)
{
	public readonly CircuitComponentType Type = type;

	/// <summary>
	/// ID of the junction at the start of this component.
	/// </summary>
	public required string From;

	/// <summary>
	/// ID of the junction at the end of this component.
	/// </summary>
	public required string To;
}

/// <summary>
/// Component model that has a defined position, orientation and size.
/// This includes every type of component except wires, which may span any size between two junctions.
/// </summary>
public abstract class RigidComponentModel(string id, CircuitComponentType type) : ComponentModel(id, type)
{
	public required Vector2I Position;
	public Direction Direction = Direction.Right;
}

public class AmmeterModel(string id) : RigidComponentModel(id, CircuitComponentType.Ammeter)
{
}

public class CellModel(string id) : RigidComponentModel(id, CircuitComponentType.Cell)
{
	public required double Resistance = 0;
	public required double EMF = 10;
}

public class FixedResistorModel(string id) : RigidComponentModel(id, CircuitComponentType.FixedResistor)
{
	public required double Resistance = 0;
}

public class FuseModel(string id) : RigidComponentModel(id, CircuitComponentType.Fuse)
{
	public required double CurrentRating = 10;
	public bool Tripped = false;
}

public class VoltmeterModel(string id) : RigidComponentModel(id, CircuitComponentType.Voltmeter)
{
}

public class WireModel(string id) : ComponentModel(id, CircuitComponentType.Wire)
{
}
