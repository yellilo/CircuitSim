using Godot;

namespace CircuitSim.CircuitEditor;

[Tool]
[GlobalClass]
public partial class CircuitEditorContainer : HSplitContainer
{
	[Export]
	public required CircuitBoard CircuitBoard;

	[Export]
	public required ComponentDrawer ComponentDrawer;

	public int SeparatorWidth => GetThemeConstant("separation");

	public override void _Ready()
	{
		SplitOffset = (int)(Size.X - ComponentDrawer.MinWidth - SeparatorWidth);
	}


	public override void _Process(double delta)
	{
		if (ComponentDrawer.Size.X > ComponentDrawer.MaxWidth)
		{
			SplitOffset = (int)(Size.X - ComponentDrawer.MaxWidth - SeparatorWidth);
		}
	}
}
