namespace CircuitSim.Circuitry;

public partial class VariableResistor : RigidComponent
{
	public double MinimumResistance = 0;
	public double MaximumResistance = 10;

	public double SliderPosition = 0.5;

	public string? SliderTerminalId;
	public Junction? WiperTerminal => SliderTerminalId != null ? Board.Junctions[SliderTerminalId] : null;

	public override double Resistance => MinimumResistance + SliderPosition * (MaximumResistance - MinimumResistance);

    public VariableResistor()
    {
        CircuitSymbol.SymbolType = CircuitSymbolType.VariableResistor;
    }
}