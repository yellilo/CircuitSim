namespace CircuitSim.Circuitry;

public partial class Cell : RigidComponent
{
    /// Voltage supplied to the circuit, measured in volts.
    public double EMF = 10;

    public Cell()
    {
        CircuitSymbol.SymbolType = CircuitSymbolType.Cell;
    }
}