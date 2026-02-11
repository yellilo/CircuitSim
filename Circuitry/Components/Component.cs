using System;
using CircuitSim.Editor;
using Godot;

namespace CircuitSim.Circuitry;

public abstract partial class Component : Control
{
	public CircuitBoard Board
	{
		get
		{
			Node current = GetParent();

			while (current != null)
			{
				if (current is CircuitBoard board) return board;
				current = current.GetParent();
			}

			throw new Exception("Component is not on a circuit board.");
		}
	}
	public required ComponentModel Model;

	public ComponentType Type => Model.Type;

	public Junction From => Board.Junctions[Model.From];
	public Junction To => Board.Junctions[Model.To];

	/// <summary>
	/// Resistance of the component, measured in ohms.
	/// </summary>
	public virtual double Resistance { get => 0; }

	/// <summary>
	/// Potential lost across the component, measured in volts.
	/// <summary>
	public double PotentialDifference => From.Potential - To.Potential;

	/// <summary>
	/// Current in the component, measured in amperes.
	/// </summary>
	public double Current = 0;

	public override void _Ready()
	{
		Name = "Component#" + Model.Id;
		AddChild(new CircuitSymbol() { ComponentType = Model.Type });
	}

	public static T FromModel<T>(ComponentModel model) where T : Component
	{
		var componentType = ComponentTypes.GetType(model.Type);
		var constructor = componentType.GetConstructor([]);

		var component = (T)constructor!.Invoke([]);
		component.Model = model;

		return component;
	}

	public static Component FromModel(ComponentModel model) => FromModel<Component>(model);
}
