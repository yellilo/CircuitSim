using System;
using Godot;

namespace CircuitSim.Circuitry;

public class CircuitBoardMetadata(string id)
{
	public readonly string Id = id;
	public required string Name;

	public int Scale = 60;
	public Vector2I Offset = Vector2I.Zero;

	public bool GridVisible = true;

	public DateTime LastEdited = DateTime.UtcNow;
}
