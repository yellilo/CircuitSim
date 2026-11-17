namespace CircuitSim;

public class Unit(string name, string symbol)
{
    public static readonly Unit Ampere = new("Ampere", "A");
    public static readonly Unit Candela = new("Candela", "cd");
    public static readonly Unit Ohm = new("Ohm", "Ω");
    public static readonly Unit Volt = new("Volt", "V");
    public static readonly Unit Watt = new("Watt", "W");

    public readonly string Name = name;
    public readonly string Symbol = symbol;
}
