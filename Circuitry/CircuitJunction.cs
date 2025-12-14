using CircuitSim.Circuitry.Models;
using Godot;

namespace CircuitSim.Circuitry;

public partial class CircuitJunction : Node
{
	public required JunctionModel Model { get; init; }

	public Vector2I Position = Vector2I.Zero;

	/// <summary>
	/// Voltage at the junction in Volts.
	/// </summary>
	public double Potential => 1;
}