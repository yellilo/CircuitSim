using CircuitSim.Circuitry;
using Godot;
using System;

namespace CircuitSim.Editor;

[Tool]
[GlobalClass]
public partial class EditorSidebar : VBoxContainer
{
	[Export]
	public required CircuitBoard Board;

	public float MinWidth => CustomMinimumSize.X;
	public float MaxWidth => Math.Max(MinWidth, GetParentAreaSize().X / 3);

	public override void _Ready()
	{
		AddComponentsSection();
	}

	public EditorSidebarSection AddSection(string title)
	{
		var section = new EditorSidebarSection()
		{
			Title = title
		};

		AddChild(section);
		return section;
	}

	public EditorSidebarSection AddComponentsSection()
	{
		var section = AddSection("Electronic Components");

		var componentTypes = (ComponentType[])Enum.GetValues(typeof(ComponentType));

		foreach (var type in componentTypes)
		{
			var button = new Button() { Text = type.ToString() };
			button.Pressed += () => Board.Cursor.ComponentType = type;
			section.AddChild(button);
		}

		return section;
	}
}
