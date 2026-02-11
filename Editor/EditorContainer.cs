using Godot;

namespace CircuitSim.Editor;

[Tool]
[GlobalClass]
public partial class EditorContainer : HSplitContainer
{
	[Export]
	public required CircuitBoard CircuitBoard;

	[Export]
	public required EditorSidebar Sidebar;

	public int SeparatorWidth => GetThemeConstant("separation");

	public override void _Ready()
	{
		SplitOffset = (int)(Size.X - Sidebar.MinWidth - SeparatorWidth);
	}


	public override void _Process(double delta)
	{
		if (Sidebar.Size.X > Sidebar.MaxWidth)
		{
			SplitOffset = (int)(Size.X - Sidebar.MaxWidth - SeparatorWidth);
		}
	}
}
