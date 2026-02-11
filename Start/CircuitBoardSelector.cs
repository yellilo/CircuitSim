using CircuitSim.Circuitry;
using Godot;

namespace CircuitSim.Start;

[Tool]
[GlobalClass]
public partial class CircuitBoardSelector : ScrollContainer
{
	public override void _Ready()
	{
		var container = new VBoxContainer
		{
			SizeFlagsHorizontal = SizeFlags.ExpandFill
		};

		var newButton = new Button()
		{
			Text = "Create Circuit Board",
			ActionMode = BaseButton.ActionModeEnum.Press
		};

		newButton.Pressed += () =>
		{
			var id = Global.RandomUUID();
			var metadata = new CircuitBoardMetadata(id)
			{
				Name = "My Circuit Board"
			};

			Global.CircuitBoardMetadata.Add(id, metadata);
			Global.OpenCircuitBoard(id);
		};

		container.AddChild(newButton);
		container.AddChild(new HSeparator());

		foreach (var metadata in Global.CircuitBoardMetadata.Values)
		{
			container.AddChild(new CircuitBoardSelectorButton() { CircuitBoardMetadata = metadata });
		}

		AddChild(container);
	}
}

public partial class CircuitBoardSelectorButton : Button
{
	public required CircuitBoardMetadata CircuitBoardMetadata;

	public override void _Ready()
	{
		Text = CircuitBoardMetadata.Name;
		ActionMode = ActionModeEnum.Press;
		Pressed += OnPressed;
	}

	public void OnPressed()
	{
		Global.OpenCircuitBoard(CircuitBoardMetadata.Id);
	}
}