using Godot;

namespace CircuitSim.Editor;

[Tool]
public partial class CircuitBoardContent : Control
{
	public required CircuitBoard Board;
	private Control OffsetControl = new();

	public override void _Ready()
	{
		SetAnchorsPreset(LayoutPreset.Center);

		Scale = Board.Metadata.Scale * Vector2.One;
		Board.Zoomed += () => Scale = Board.Metadata.Scale * Vector2.One;

		OffsetControl.Position = Board.Metadata.Offset;
		Board.Dragged += () => OffsetControl.Position = Board.Metadata.Offset;

		AddChild(OffsetControl);

		Board.ComponentAdded += (c) => OffsetControl.AddChild(c);
		Board.JunctionAdded += (j) => OffsetControl.AddChild(j);
	}
}