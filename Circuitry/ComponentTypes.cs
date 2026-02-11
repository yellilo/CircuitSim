using System;
using System.Collections.Generic;

namespace CircuitSim.Circuitry;

public enum ComponentType
{
	Ammeter,
	Cell,
	FixedResistor,
	Fuse,
	Lamp,
	Voltmeter,
	Wire
}

public static class ComponentTypes
{
	static ComponentTypes()
	{
		Register<Ammeter, AmmeterModel>(ComponentType.Ammeter);
		Register<Cell, CellModel>(ComponentType.Cell);
		Register<FixedResistor, FixedResistorModel>(ComponentType.FixedResistor);
		Register<Fuse, FuseModel>(ComponentType.Fuse);
		Register<Lamp, LampModel>(ComponentType.Lamp);
		Register<Voltmeter, VoltmeterModel>(ComponentType.Voltmeter);
		Register<Wire, WireModel>(ComponentType.Wire);
	}

	private static readonly Dictionary<ComponentType, Type> ComponentTypeMap = [];
	private static readonly Dictionary<ComponentType, Type> ComponentModelTypeMap = [];

	public static void Register<T, M>(ComponentType type) where T : Component where M : ComponentModel
	{
		ComponentTypeMap.Add(type, typeof(T));
		ComponentModelTypeMap.Add(type, typeof(M));
	}

	public static Type GetType(ComponentType type) => ComponentTypeMap[type];
	public static Type GetModelType(ComponentType type) => ComponentModelTypeMap[type];
}