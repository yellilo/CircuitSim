using System;
using CircuitSim.Circuitry.Models;
using Godot;

namespace CircuitSim;

public partial class Global : Node
{
	public static readonly string ApplicationName = ProjectSettings.GetSetting("application/config/name").ToString();

#pragma warning disable CS8618 // The scene tree will only ever be accessed after it is no longer null
	public static SceneTree SceneTree { get; private set; }
	public static Window Window => SceneTree.Root;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SceneTree = GetTree();
		Engine.MaxFps = 59;

		var path = CircuitBoardManager.CircuitBoardsDirectory + "/test/components.lst";

		var models = ListFileAccess<ComponentModel>.Load(path);

		DirAccess.MakeDirRecursiveAbsolute(CircuitBoardManager.CircuitBoardsDirectory + "/test");
		ListFileAccess<ComponentModel>.Save(path, [
			new AmmeterModel(Guid.NewGuid().ToString()) { From = "ssss", To = "hi", Position = Vector2I.Down }
		]);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
