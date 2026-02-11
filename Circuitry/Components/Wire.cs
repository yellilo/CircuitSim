using System.Linq;
using Godot;

namespace CircuitSim.Circuitry;

public partial class Wire : Component
{
	public Vector2I[] Vertices => ((WireModel)Model).Vertices.ToArray();

	public override double Resistance => 0;
}