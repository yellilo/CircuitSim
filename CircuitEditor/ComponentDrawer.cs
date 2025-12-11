using Godot;
using System;

namespace CircuitSim.CircuitEditor;

[Tool]
[GlobalClass]
public partial class ComponentDrawer : VBoxContainer
{
	public float MinWidth => CustomMinimumSize.X;
	public float MaxWidth => Math.Max(MinWidth, GetParentAreaSize().X / 3);

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
