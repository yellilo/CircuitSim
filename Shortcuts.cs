using Godot;

namespace CircuitSim;

public static class Shortcuts
{
	public static readonly Shortcut Save = new()
	{
		Events = [
			new InputEventKey
			{
				CtrlPressed = true,
				Keycode = Key.S,
			}
		]
	};

	public static readonly Shortcut Quit = new()
	{
		Events = [
			new InputEventKey
			{
				AltPressed = true,
				Keycode = Key.F4,
			}
		]
	};
}