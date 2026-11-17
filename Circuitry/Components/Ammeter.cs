namespace CircuitSim.Circuitry;

public partial class Ammeter : RigidComponent
{
	public Ammeter() : base()
	{
		CircuitSymbol.SymbolType = CircuitSymbolType.Ammeter;
	}
}