namespace CircuitSim.Circuitry;

public partial class FixedResistor : RigidComponent
{
	public double FixedResistance = 10;
	public override double Resistance => FixedResistance;

    public FixedResistor()
    {
        CircuitSymbol.SymbolType = CircuitSymbolType.FixedResistor;
    }
}