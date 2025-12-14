using CircuitSim.Circuitry.Models;
using Godot;

namespace CircuitSim.Circuitry;

public abstract partial class CircuitComponent<M> : Node where M : ComponentModel
{
	public CircuitBoard Board => GetParent<CircuitBoard>();
	public required M Model { get; init; }

	public Vector2I Position;

	public CircuitJunction From => Board.Junctions[Model.From];
	public CircuitJunction To => Board.Junctions[Model.To];

	/// <summary>
	/// Resistance of the component, measured in ohms.
	/// </summary>
	public abstract double Resistance { get; }

	/// <summary>
	/// Potential lost across the component, measured in volts.
	/// <summary>
	public double PotentialDifference => From.Potential - To.Potential;

	/// <summary>
	/// Current in the component, measured in amperes.
	/// </summary>
	public double Current = 0;
}

public partial class Ammeter : CircuitComponent<AmmeterModel>
{
	public override double Resistance => 0;
}

public partial class Voltmeter : CircuitComponent<VoltmeterModel>
{
	public override double Resistance => double.PositiveInfinity;
}

public partial class Wire : CircuitComponent<WireModel>
{
	public override double Resistance => 0;
}

public partial class Cell : CircuitComponent<CellModel>
{
	/// <summary>
	/// Voltage supplied to the circuit, measured in volts.
	/// </summary>
	public double EMF => Model.EMF;

	public override double Resistance => 0;
}

public partial class Fuse : CircuitComponent<FuseModel>
{
	public double CurrentRating => Model.CurrentRating;
	public bool Tripped => Model.Tripped;

	public override double Resistance => Tripped ? double.PositiveInfinity : 0;
}