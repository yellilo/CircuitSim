using Godot;

namespace CircuitSim.Editor;

[Tool]
public partial class CircuitBoardGrid : Node2D
{
	public static readonly Color BackgroundColor = new(0.95f, 0.95f, 0.95f);
	public static readonly Color LineColor = new(0f, 0f, 0f, 0.1f);
	public static readonly int LineWidth = 1;

	public required CircuitBoard Board;

	public override void _Ready()
	{
		Board.Dragged += QueueRedraw;
		Board.Resized += QueueRedraw;
		Board.Zoomed += QueueRedraw;

		Visible = Board.Metadata.GridVisible;

		VisibilityChanged += () =>
		{
			Board.Metadata.GridVisible = Visible;
			Board.HasUnsavedChanges = true;
		};
	}

	public override void _Draw()
	{
		DrawRect(new Rect2(0, 0, Board.Size), BackgroundColor);

		// Draw vertical lines
		for (float x = Board.OriginPosition.X; x > 0; x -= Board.Metadata.Scale)
		{
			DrawGridLine(new Vector2(x, 0), new Vector2(x, Board.Size.Y));
		}
		for (float x = Board.OriginPosition.X; x < Board.Size.X; x += Board.Metadata.Scale)
		{
			DrawGridLine(new Vector2(x, 0), new Vector2(x, Board.Size.Y));
		}
		// Draw horizontal lines
		for (float y = Board.OriginPosition.Y; y > 0; y -= Board.Metadata.Scale)
		{
			DrawGridLine(new Vector2(0, y), new Vector2(Board.Size.X, y));
		}
		for (float y = Board.OriginPosition.Y; y < Board.Size.Y; y += Board.Metadata.Scale)
		{
			DrawGridLine(new Vector2(0, y), new Vector2(Board.Size.X, y));
		}
	}

	public void DrawGridLine(Vector2 from, Vector2 to)
	{
		DrawLine(from, to, LineColor, LineWidth, Engine.IsEditorHint());
	}
}