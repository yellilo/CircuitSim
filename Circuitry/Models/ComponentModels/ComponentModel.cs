using System;

namespace CircuitSim.Circuitry;

public abstract class ComponentModel(string id, ComponentType type) : Model(id)
{
	public ComponentType Type = type;

	/// <summary>
	/// ID of the junction at the start of this component.
	/// </summary>
	public required string From;

	/// <summary>
	/// ID of the junction at the end of this component.
	/// </summary>
	public required string To;

	public static T FromType<T>(ComponentType componentType, string? modelId = null) where T : ComponentModel
	{
		var modelType = ComponentTypes.GetModelType(componentType);

		var modelConstructor = modelType.GetConstructor([typeof(string)]) ?? throw new NotImplementedException($"'{modelType}' has no constructor that takes the following parameters: (string id)");

		var model = (T)modelConstructor.Invoke([modelId ?? Global.RandomUUID()]);

		return model;
	}

	public static ComponentModel FromType(ComponentType componentType, string? modelId = null) => FromType<ComponentModel>(componentType, modelId);
}
