using Godot;

namespace CircuitSim.Editor;

[Tool]
[GlobalClass]
public partial class EditorSidebarSection : VBoxContainer
{
	public EditorSidebar Sidebar => GetParent<EditorSidebar>();

	private Label TitleLabel = new() { Name = "TitleLabel" };
	public string Title
	{
		get => TitleLabel.Text;
		set => TitleLabel.Text = value;
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddChild(TitleLabel);

	}
}
