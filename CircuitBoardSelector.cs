using CircuitSim.Circuitry;
using Godot;

namespace CircuitSim;

[Tool]
[GlobalClass]
public partial class CircuitBoardSelector : ScrollContainer
{
	public override void _Ready()
	{
		var container = GetChild(0);

		foreach (var metadata in CircuitBoardManager.MetadataList)
		{
			container.AddChild(new CircuitBoardSelectorButton() { CircuitBoardMetadata = metadata });
		}
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
		CircuitBoardManager.LoadCircuitBoard(CircuitBoardMetadata.Id);
	}
}