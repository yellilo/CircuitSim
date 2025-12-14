using CircuitSim.Circuitry;
using System.Collections.Generic;
using System.Linq;

namespace CircuitSim;

public static class CircuitBoardManager
{
	public static readonly string CircuitBoardsDirectory = "user://circuit_boards";

	public static readonly List<CircuitBoardMetadata> MetadataList = ListFileAccess<CircuitBoardMetadata>.Load(CircuitBoardsDirectory + "/metadata.lst");

	public static CircuitBoard? CurrentBoard { get; private set; }

	public static void LoadCircuitBoard(string id)
	{
		var metadata = MetadataList.Find((s) => s.Id == id)!;

		CurrentBoard = new()
		{
			Metadata = metadata
		};

		Global.Window.Title = $"{metadata.Name} - {Global.ApplicationName}";
		Global.SceneTree.ChangeSceneToFile("res://Scenes/CircuitEditor.tscn");
	}
}