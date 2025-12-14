using System.Collections.Generic;
using CircuitSim.Circuitry.Models;

namespace CircuitSim.Circuitry;

using CircuitComponent = CircuitComponent<ComponentModel>;

/// <summary>
/// A graph with <see cref="CircuitJunction"/> nodes and <see cref="CircuitComponent"/> edges.
/// </summary>
public class CircuitBoard
{
	public required CircuitBoardMetadata Metadata;

	// Shorthands for accessing metadata properties
	public string Id => Metadata.Id;
	public string Name
	{
		get => Metadata.Name;
		set => Metadata.Name = value;
	}


	public Dictionary<string, CircuitComponent> Components = [];
	public Dictionary<string, CircuitJunction> Junctions = [];
}