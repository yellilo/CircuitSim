using Godot;

namespace CircuitSim.Circuitry.Models;

public abstract class CircuitComponentModel(string id, CircuitComponentType type) : CircuitModel(id)
{
  public readonly CircuitComponentType Type = type;

  public required string From;
  public required string To;
}

/// <summary>
/// Component model that has a defined position and size.
/// This includes every type of component except wires, which may span any size between two junctions.
/// </summary>
public abstract class RigidCircuitComponentModel(string id, CircuitComponentType type) : CircuitComponentModel(id, type)
{
  public required Vector2I Position;
}

public class AmmeterModel(string id) : RigidCircuitComponentModel(id, CircuitComponentType.Ammeter)
{
}

public class CellModel(string id) : RigidCircuitComponentModel(id, CircuitComponentType.Cell)
{
  public required double Resistance = 0;
  public required double EMF = 10;
}

public class FixedResistorModel(string id) : RigidCircuitComponentModel(id, CircuitComponentType.FixedResistor)
{
  public required double Resistance = 0;
}

public class FuseModel(string id) : RigidCircuitComponentModel(id, CircuitComponentType.Fuse)
{
  public required double CurrentRating = 10;
  public bool Tripped = false;
}

public class VoltmeterModel(string id) : RigidCircuitComponentModel(id, CircuitComponentType.Voltmeter)
{
}

public class WireModel(string id) : CircuitComponentModel(id, CircuitComponentType.Wire)
{
}
