namespace CircuitSim.Circuitry;

public partial class Switch : RigidComponent
{
	private bool _closed = true;
	public bool Closed
	{
		get => _closed;
		set
		{
			_closed = value;
			CircuitSymbol.SymbolType = value ? CircuitSymbolType.SwitchClosed : CircuitSymbolType.SwitchOpen;
		}
	}

	public override double Resistance => Closed ? 0 : double.PositiveInfinity;

    public Switch()
    {
        CircuitSymbol.SymbolType = CircuitSymbolType.SwitchClosed;
    }
}