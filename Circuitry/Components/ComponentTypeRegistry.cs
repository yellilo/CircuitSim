using System;
using System.Collections.Generic;

namespace CircuitSim.Circuitry;

public static class ComponentTypeRegistry
{
    static ComponentTypeRegistry()
    {
        RegisterType<Ammeter>("Ammeter", CircuitSymbolType.Ammeter);
        RegisterType<Cell>("Cell", CircuitSymbolType.Cell);
        RegisterType<FixedResistor>("Fixed Resistor", CircuitSymbolType.FixedResistor);
        RegisterType<Wire>("Wire");
    }

    // Component type IDs (e.g. "FixedResistor") mapped to their Type objects
    public static readonly Dictionary<string, Type> ComponentTypes = [];
    public static readonly Dictionary<string, Type> RigidComponentTypes = [];

    // Component type IDs (e.g. "FixedResistor") mapped to their display names (e.g. "Fixed Resistor")
    public static readonly Dictionary<string, string> ComponentNames = [];

    // Component type IDs (e.g. "FixedResistor") mapped to the symbol they display in the component selector
    public static readonly Dictionary<string, CircuitSymbolType> SelectorCircuitSymbolTypes = [];

    private static void RegisterType<T>(string name, CircuitSymbolType? selectorCircuitSymbolType = null) where T : Component
    {
        var type = typeof(T);
        ComponentTypes.Add(type.Name, type);
        ComponentNames.Add(type.Name, name);

        if (selectorCircuitSymbolType is CircuitSymbolType t)
        {
            SelectorCircuitSymbolTypes.Add(type.Name, t);
        }

        if (IsTypeRigid(type)) {
            RigidComponentTypes.Add(type.Name, type);
        }
    }

    public static Type GetType(string typeId) => ComponentTypes[typeId];
    public static string GetName(string typeId) => ComponentNames[typeId];

    public static bool IsTypeRigid(Type type) => type.IsSubclassOf(typeof(RigidComponent));

    public static Component CreateComponent(string typeId) => CreateComponent<Component>(typeId);
    public static T CreateComponent<T>(string typeId) where T : Component
    {
        Type type = GetType(typeId);
        var constructor = type.GetConstructor([])!;
        return (T)constructor.Invoke([]);
    }
}