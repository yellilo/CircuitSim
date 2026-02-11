namespace CircuitSim.Circuitry;

public partial class Cell : RigidComponent
{
	/// <summary>
	/// Voltage supplied to the circuit, measured in volts.
	/// </summary>
	public double EMF => ((CellModel)Model).EMF;

	public override double Resistance => 0;
}