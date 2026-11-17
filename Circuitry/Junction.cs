using CircuitSim.Numerics;

namespace CircuitSim.Circuitry;

public partial class Junction : CircuitElement
{
    public Vector2Int Position = Vector2Int.Zero;

    // Voltage at the junction in volts.
    public double Potential => Board.MNA.ResolvedVoltages[this];
}