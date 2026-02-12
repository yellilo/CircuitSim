using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CircuitSim.Circuitry;
using Godot;

namespace CircuitSim.Editor;

[Tool]
[GlobalClass]
public partial class CircuitBoard : Control
{
	public CircuitBoardMetadata Metadata;

	// Save paths
	public readonly string DirectoryPath;
	public readonly string ComponentsFilePath;
	public readonly string JunctionsFilePath;
	
	// Nodes
	public CircuitBoardGrid Grid;
	public CircuitBoardContent Content;
	public CircuitBoardCursor Cursor;

	// Models
	public Dictionary<string, ComponentModel> ComponentModels;
	public Dictionary<string, JunctionModel> JunctionModels;

	// Live circuit elements
	public Dictionary<string, Component> Components = [];
	public Dictionary<string, Junction> Junctions = [];

	// State
	public bool HasUnsavedChanges = false;
	public bool IsHovered = false;

	// --------------------
	//        Events
	// --------------------

	// Adding/removing elements
	public event Action<Component>? ComponentAdded;
	public event Action<Junction>? JunctionAdded;

	// Saving
	public event Action? Changed;
	public event Action? Saved;

	// Board input
	public event Action? Dragged;
	public event Action? Pressed;
	public event Action? Zoomed;

	// --------------------
	//    Initialisation
	// --------------------

	public CircuitBoard()
	{
		Metadata = Global.CurrentCircuitBoardMetadata ?? new CircuitBoardMetadata("editor") { Name = "Editor" };

		DirectoryPath = Global.CircuitBoardsDirectory + "/" + Metadata.Id;
		ComponentsFilePath = DirectoryPath + "/components.lst";
		JunctionsFilePath = DirectoryPath + "/junctions.lst";

		ComponentModels = ListFileAccess<ComponentModel>.Load(ComponentsFilePath).ToDictionary((model) => model.Id);
		JunctionModels = ListFileAccess<JunctionModel>.Load(JunctionsFilePath).ToDictionary((model) => model.Id);

		Grid = new CircuitBoardGrid()
		{
			Board = this,
			Name = "Grid"
		};

		Content = new CircuitBoardContent()
		{
			Board = this,
			Name = "Content"
		};

		Cursor = new CircuitBoardCursor()
		{
			Board = this,
			Name = "Cursor"
		};

		AddChild(Grid);
		AddChild(Content);
		AddChild(Cursor);
	}

	public override void _Ready()
	{
		SetDefaultCursorShape(CursorShape.Cross);

		MouseEntered += () => IsHovered = true;
		MouseExited += () => IsHovered = false;

		Changed += () =>
		{
			HasUnsavedChanges = true;
			Global.SetTitle(Metadata.Name + "*");
		};

		Saved += () =>
		{
			HasUnsavedChanges = false;
			Global.SetTitle(Metadata.Name);
		};

		ComponentAdded += (_) => Changed?.Invoke();
		JunctionAdded += (_) => Changed?.Invoke();

		Dragged += Changed;
		Zoomed += Changed;

		foreach (var model in ComponentModels.Values)
		{
			AddComponentFromModel(model);
		}
	}

	// --------------------
	//   Editing Elements
	// --------------------

	public Junction AddJunction(Junction junction)
	{
		JunctionModels.TryAdd(junction.Model.Id, junction.Model);
		Junctions.Add(junction.Model.Id, junction);

		JunctionAdded?.Invoke(junction);

		return junction;
	}

	public Junction AddJunctionAtPosition(Vector2I pos)
	{
		var model = new JunctionModel(Global.RandomUUID())
		{
			Position = pos
		};

		var junction = new Junction()
		{
			Model = model
		};

		return AddJunction(junction);
	}

	public Component AddComponent(Component component)
	{
		ComponentModels.TryAdd(component.Model.Id, component.Model);
		Components.Add(component.Model.Id, component);

		// Add junctions at rigid component terminals.
		if (component.Model is RigidComponentModel rigidComponentModel)
		{
			component.Model.From ??= AddJunctionAtPosition(rigidComponentModel.Position + Vector2I.Left).Model.Id;
			component.Model.To ??= AddJunctionAtPosition(rigidComponentModel.Position + Vector2I.Right).Model.Id;
		}

		ComponentAdded?.Invoke(component);

		return component;
	}

	public Component AddComponentFromModel<M>(M model) where M : ComponentModel
	{
		return AddComponent(Component.FromModel(model));
	}

	// --------------------
	//        Input
	// --------------------

	public override void _Input(InputEvent e)
	{
		if (!IsHovered) return;

		if (e is InputEventMouseButton mouseButtonEvent)
		{
			if (mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.IsPressed()) Pressed?.Invoke();
			else if (mouseButtonEvent.ButtonIndex == MouseButton.Middle) IsDragging = mouseButtonEvent.IsPressed();
		}
		else if (e is InputEventMouseMotion && IsDragging && IsHovered)
		{
			Metadata.Offset = DragOffset + (Vector2I)((Cursor.Position - DragOrigin) / Metadata.Scale).Round();
			Dragged?.Invoke();
		}

		if (!e.IsPressed()) return;

		// Zoom
		if (e.IsAction("zoom_in")) Zoom(ZoomInterval);
		else if (e.IsAction("zoom_out")) Zoom(-ZoomInterval);
	}

	// --------------------
	//       Scaling
	// --------------------

	public static int ZoomInterval = 5;

	public void Zoom(int scaleModifier)
	{
		Metadata.Scale = Math.Clamp(Metadata.Scale + scaleModifier, 30, 120);
		Zoomed?.Invoke();
	}

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

	/// <summary>
	/// Converts a local screen position into a position on the circuit board.
	/// </summary>
	public Vector2I LocalToBoardPosition(Vector2 localPosition)
	{
		var originRelativePosition = localPosition - OriginPosition;
		var boardPosition = (originRelativePosition / Metadata.Scale).Round();

		return (Vector2I)boardPosition;
	}

	/// <summary>
	/// Converts a board position into a screen position local to this circuit board node.
	/// </summary>
	public Vector2 BoardToLocalPosition(Vector2I boardPosition)
	{
		return OriginPosition + boardPosition * Metadata.Scale;
	}

	public static async Task<ConfirmationResponse> ConfirmSave()
	{
		return await ConfirmationDialog.Show(
			"Unsaved Changes",
			"Do you want to save the changes that you have made to this circuit board?",
			"Save", "Discard", "Cancel"
		);
	}

	public void Save()
	{
		DirAccess.MakeDirRecursiveAbsolute(DirectoryPath);

		ListFileAccess<CircuitBoardMetadata>.Save(Global.CircuitBoardMetadataListPath, Global.CircuitBoardMetadata.Values);
		ListFileAccess<ComponentModel>.Save(ComponentsFilePath, ComponentModels.Values);
		ListFileAccess<JunctionModel>.Save(JunctionsFilePath, JunctionModels.Values);

		Saved?.Invoke();
	}

	public void Delete()
	{
		Global.CloseCircuitBoard();

		Global.CircuitBoardMetadata.Remove(Metadata.Id);
		ListFileAccess<CircuitBoardMetadata>.Save(Global.CircuitBoardMetadataListPath, Global.CircuitBoardMetadata.Values);

		DirAccess.RemoveAbsolute(ComponentsFilePath);
		DirAccess.RemoveAbsolute(JunctionsFilePath);
		DirAccess.RemoveAbsolute(DirectoryPath);
	}

	public async Task Close()
	{
		var response = await ConfirmSave();

		switch (response)
		{
			case ConfirmationResponse.Confirmed:
				Save();
				Global.CloseCircuitBoard();
				break;
			case ConfirmationResponse.Rejected:
				Global.CloseCircuitBoard();
				break;
		}
	}

	// --------------------
	//      Quitting
	// --------------------

	public override void _EnterTree()
	{
		Global.SetTitle(Metadata.Name);
		GetTree().AutoAcceptQuit = false;
	}

	public override void _ExitTree()
	{
		Global.SetTitle();
		GetTree().AutoAcceptQuit = true;
	}

	public override void _Notification(int what)
	{
		if (what == NotificationWMCloseRequest) Quit();
	}

	public async Task Quit()
	{
		if (!HasUnsavedChanges)
		{
			GetTree().Quit();
			return;
		}

		var response = await ConfirmSave();

		switch (response)
		{
			case ConfirmationResponse.Confirmed:
				Save();
				GetTree().Quit();
				break;
			case ConfirmationResponse.Rejected:
				GetTree().Quit();
				break;
		}
	}
}