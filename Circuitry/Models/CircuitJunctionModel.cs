using Godot;

namespace CircuitSim.Circuitry.Models;

public class CircuitJunctionModel(string id) : CircuitModel(id)
{
  public required Vector2I Position = Vector2I.Zero;
}