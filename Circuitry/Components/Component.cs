using Microsoft.Graphics.Canvas.Brushes;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace CircuitSim.Circuitry;

public abstract partial class Component : CircuitElement
{
    public string PositiveTerminalId = "";
    public string NegativeTerminalId = "";

    public Junction PositiveTerminal => Board.Junctions[PositiveTerminalId];
    public Junction NegativeTerminal => Board.Junctions[NegativeTerminalId];

    /// <summary>
    /// Resistance of the component, measured in ohms.
    /// </summary>
    public virtual double Resistance { get => 0; }

    /// <summary>
    /// Potential lost across the component, measured in volts.
    /// <summary>
    public double PotentialDifference => PositiveTerminal.Potential - NegativeTerminal.Potential;

    /// <summary>
    /// Current in the component, measured in amperes.
    /// </summary>
    public double Current => Resistance == 0 ? Board.MNA.ResolvedCurrents[this] : PotentialDifference / Resistance;

    public virtual bool IsPreview { get; set; } = false;
}
