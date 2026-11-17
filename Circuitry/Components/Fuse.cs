namespace CircuitSim.Circuitry;

public partial class Fuse : RigidComponent
{
	public double CurrentRating = 10;

	private bool _tripped = false;
	public bool Tripped
	{
		get => _tripped;
		set
		{
			_tripped = value;
			CircuitSymbol.SymbolType = value ? CircuitSymbolType.FuseTripped : CircuitSymbolType.Fuse;
		}
	}

	public override double Resistance => Tripped ? double.PositiveInfinity : 0;

    public Fuse()
    {
        CircuitSymbol.SymbolType = CircuitSymbolType.Fuse;
    }
}