namespace CircuitSim.Circuitry;

public partial class Lamp : RigidComponent
{
	public double LuminousIntensity = 10;

	public override double Resistance => 1;

    public Lamp()
    {
        CircuitSymbol.SymbolType = CircuitSymbolType.Lamp;
    }
}