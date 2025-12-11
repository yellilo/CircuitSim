using CircuitSim.Circuitry;
using System.Collections.Generic;

namespace CircuitSim;

public static class CircuitBoardManager
{
  public static readonly string CircuitsDirectory = "user://Circuits";

  public static readonly List<CircuitBoardMetadata> CircuitMetadataList = [new("test") { Name = "Test Circuit" }];

  public static CircuitBoard? CurrentBoard { get; private set; }

  public static void LoadCircuitBoard(string id)
  {
    var metadata = CircuitMetadataList.Find((s) => s.Id == id)!;

    CurrentBoard = new()
    {
      Metadata = metadata
    };

    Global.Window.Title = $"{metadata.Name} - {Global.ApplicationName}";
    Global.SceneTree.ChangeSceneToFile("res://Scenes/CircuitEditor.tscn");
  }
}