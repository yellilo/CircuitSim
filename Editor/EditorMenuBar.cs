using System;
using Godot;

namespace CircuitSim.Editor;

[Tool]
[GlobalClass]
public partial class EditorMenuBar : MenuBar
{
	[Export]
	public required CircuitBoard Board;

	public override void _Ready()
	{
		var CircuitBoardMenu = new EditorMenu() { Title = "Circuit Board" };
		var ViewMenu = new EditorMenu() { Title = "View" };

		CircuitBoardMenu.AddButton("Save", Board.Save, Shortcuts.Save);
		CircuitBoardMenu.AddButton("Delete", Board.Delete);
		CircuitBoardMenu.AddButton("Quit", () => Board.Quit(), Shortcuts.Quit);

		ViewMenu.AddToggle("Show Grid", Board.Metadata.GridVisible, (v) => Board.Grid.Visible = v);

		AddChild(CircuitBoardMenu);
		AddChild(ViewMenu);
	}
}

[Tool]
[GlobalClass]
public partial class EditorMenu : PopupMenu
{
	public void AddButton(string text, Action action, Shortcut? shortcut = null)
	{
		int i = ItemCount;

		AddItem(text);

		IndexPressed += (pI) =>
		{
			if (i == pI) action();
		};

		if (shortcut != null)
		{
			SetItemShortcut(i, shortcut);
		}
	}

	public void AddToggle(string text, bool @default, Action<bool> action, Shortcut? shortcut = null)
	{
		int i = ItemCount;

		AddButton(text, () =>
		{
			bool isChecked = !IsItemChecked(i);
			SetItemChecked(i, isChecked);
			action(isChecked);
		}, shortcut);

		SetItemAsCheckable(i, true);
		SetItemChecked(i, @default);
	}
}