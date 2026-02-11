using System;
using System.Collections.Generic;
using System.Linq;
using CircuitSim.Circuitry;
using Godot;

namespace CircuitSim;

[Tool]
public partial class Global : Node
{
	public static readonly string ApplicationName = ProjectSettings.GetSetting("application/config/name").ToString();

#pragma warning disable CS8618 // The scene tree will only ever be accessed after it is no longer null
	public static SceneTree SceneTree { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SceneTree = GetTree();

		Engine.MaxFps = 30;
	}

	public static string RandomUUID() => Guid.NewGuid().ToString();

	public static void SetTitle(params string[] parts)
	{
		if (Engine.IsEditorHint()) return;
		SceneTree.Root.Title = string.Join(" - ", [.. parts, ApplicationName]);
	}

	public static readonly string CircuitBoardsDirectory = "user://circuit_boards";
	public static readonly string CircuitBoardMetadataListPath = CircuitBoardsDirectory + "/metadata.lst";

	public static readonly Dictionary<string, CircuitBoardMetadata> CircuitBoardMetadata = ListFileAccess<CircuitBoardMetadata>.Load(CircuitBoardMetadataListPath).ToDictionary(metadata => metadata.Id);

	private static string? CurrentBoardId = null;
	public static CircuitBoardMetadata? CurrentCircuitBoardMetadata => CurrentBoardId != null ? CircuitBoardMetadata[CurrentBoardId] : null;

	public static void OpenCircuitBoard(string id)
	{
		CurrentBoardId = id;
		SceneTree.ChangeSceneToFile("res://Scenes/Editor.tscn");
	}

	public static void CloseCircuitBoard()
	{
		SceneTree.ChangeSceneToFile("res://Scenes/Start.tscn");
		CurrentBoardId = null;
	}
}