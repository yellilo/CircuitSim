using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace CircuitSim;

public enum ConfirmationResponse
{
	Confirmed,
	Rejected,
	Cancelled
}

public partial class ConfirmationDialog : GodotObject
{
	readonly Dictionary<ConfirmationResponse, string> ButtonLabels = [];
	readonly TaskCompletionSource<ConfirmationResponse> TaskCompletionSource = new();

	public static async Task<ConfirmationResponse> Show(string title, string description, string? confirmLabel = null, string? rejectLabel = null, string? cancelLabel = null)
	{
		var dialog = new ConfirmationDialog();

		if (confirmLabel != null) dialog.ButtonLabels.Add(ConfirmationResponse.Confirmed, confirmLabel);
		if (rejectLabel != null) dialog.ButtonLabels.Add(ConfirmationResponse.Rejected, rejectLabel);
		if (cancelLabel != null) dialog.ButtonLabels.Add(ConfirmationResponse.Cancelled, cancelLabel);

		DisplayServer.DialogShow(
			title,
			description,
			dialog.ButtonLabels.Values.ToArray(),
			new Callable(dialog, nameof(OnResponse))
		);

		return await dialog.TaskCompletionSource.Task;
	}

	private void OnResponse(int buttonIndex)
	{
		var response = ButtonLabels.Keys.ElementAt(buttonIndex);
		Free();

		TaskCompletionSource.TrySetResult(response);
	}
}