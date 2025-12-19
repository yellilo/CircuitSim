using System;
using System.Collections.Generic;
using System.Linq;
using CircuitSim.Circuitry;
using CircuitSim.Circuitry.Models;
using Godot;

namespace CircuitSim.CircuitEditor;

[Tool]
[GlobalClass]
public partial class CircuitBoard : Control
{
	[Export]
	public required CircuitBoardCursor Cursor;
	[Export]
	public required CircuitBoardGrid Grid;

	public readonly string DirectoryPath;

	public CircuitBoardMetadata Metadata;

	// Models
	public Dictionary<string, ComponentModel> ComponentModels;
	public Dictionary<string, JunctionModel> JunctionModels;

	public event Action? Dragged;
	public event Action? Zoomed;

	public bool IsHovered = false;

	public CircuitBoard()
	{
		Metadata = CircuitBoardManager.CurrentBoard?.Metadata ?? new CircuitBoardMetadata("editor") { Name = "Editor" };
		DirectoryPath = $"{CircuitBoardManager.CircuitBoardsDirectory}/{Metadata.Id}";

		ComponentModels = ListFileAccess<ComponentModel>.Load($"{DirectoryPath}/components.lst").ToDictionary((model) => model.Id);
		JunctionModels = ListFileAccess<JunctionModel>.Load($"{DirectoryPath}/junction.lst").ToDictionary((model) => model.Id);
	}

	public override void _Ready()
	{
		MouseEntered += () => IsHovered = true;
		MouseExited += () => IsHovered = false;

		SetDefaultCursorShape(CursorShape.Cross);
	}

	public override void _PhysicsProcess(double delta)
	{
	}

	// --------------------
	//        Input
	// --------------------

	public override void _Input(InputEvent e)
	{
		if (!IsHovered) return;

		if (e is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.ButtonIndex == MouseButton.Middle)
		{
			IsDragging = mouseButtonEvent.IsPressed();
		}
		else if (e is InputEventMouseMotion && IsDragging && IsHovered)
		{
			Metadata.Offset = DragOffset + (Vector2I)((Cursor.Position - DragOrigin) / Metadata.Scale).Round();
			Dragged?.Invoke();
		}

		if (!e.IsPressed()) return;

		// Zoom
		if (e.IsAction("zoom_in")) Zoom(5);
		else if (e.IsAction("zoom_out")) Zoom(-5);
	}

	// --------------------
	//       Scaling
	// --------------------

	public void Zoom(int scaleModifier)
	{
		Metadata.Scale = Math.Clamp(Metadata.Scale + scaleModifier, 30, 120);
		Zoomed?.Invoke();
	}

	public float WireScale => (float)Metadata.Scale / 10;

	// --------------------
	//       Dragging
	// --------------------

	public Vector2 DragOrigin = Vector2.Zero;
	public Vector2I DragOffset = Vector2I.Zero;

	private bool _isDragging = false;
	public bool IsDragging
	{
		get => _isDragging;
		set
		{
			_isDragging = value;

			if (IsDragging)
			{
				SetDefaultCursorShape(CursorShape.Drag);
				DragOrigin = Cursor.Position;
				DragOffset = Metadata.Offset;
			}
			else
			{
				SetDefaultCursorShape(CursorShape.Cross);
			}
		}
	}

	// --------------------
	//     Positioning
	// --------------------

	public Vector2 CenterPosition => Size / 2;
	public Vector2 OriginPosition => CenterPosition + Metadata.Offset * Metadata.Scale;

	public Vector2 SnapPosition(Vector2 localPosition)
	{
		return BoardToLocalPosition(LocalToBoardPosition(localPosition));
	}

	public Vector2I LocalToBoardPosition(Vector2 localPosition)
	{
		var originRelativePosition = localPosition - OriginPosition;
		var boardPosition = (originRelativePosition / Metadata.Scale).Round();

		return (Vector2I)boardPosition;
	}

	public Vector2 BoardToLocalPosition(Vector2I boardPosition)
	{
		return OriginPosition + boardPosition * Metadata.Scale;
	}
}