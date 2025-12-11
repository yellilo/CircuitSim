using System;
using System.Collections.Generic;
using Godot;

namespace CircuitSim.Circuitry.Models;

public static class CircuitModelFileAccess<M> where M : CircuitModel
{
  public static M[] Load(string path)
  {
    Type[] modelConstructorParameterTypes = [typeof(string)];

    var models = new List<M>();

    var configFile = new ConfigFile();
    configFile.Load(path);

    var modelIds = configFile.GetSections();

    foreach (var modelId in modelIds)
    {

      Type modelType = typeof(M);


      if (typeof(M) == typeof(CircuitComponentModel))
      {
        CircuitComponentType componentType = (CircuitComponentType)Enum.Parse(typeof(CircuitComponentType), (string)configFile.GetValue(modelId, "Type"));

        modelType =
          componentType == CircuitComponentType.Ammeter ? typeof(AmmeterModel) :
          componentType == CircuitComponentType.Cell ? typeof(CellModel) :
          componentType == CircuitComponentType.FixedResistor ? typeof(FixedResistorModel) :
          componentType == CircuitComponentType.Fuse ? typeof(FuseModel) :
          componentType == CircuitComponentType.Voltmeter ? typeof(VoltmeterModel) :
          componentType == CircuitComponentType.Wire ? typeof(Wire) :
          throw new NotImplementedException($"Component type '{componentType}' has no class defined.");
      }

      var modelConstructor = modelType.GetConstructor(modelConstructorParameterTypes) ?? throw new NotImplementedException($"'{modelType}' has no constructor that takes the following parameters: (string id)");
      var model = (M)modelConstructor.Invoke([modelId]);

      foreach (var field in modelType.GetFields())
      {
        var name = field.Name;
        if (name == "Id") continue;

        var type = field.FieldType;
        var variant = configFile.GetValue(modelId, field.Name);

        field.SetValue(model, ValueFromVariant(type, variant));
      }

      models.Add(model);
    }


    return models.ToArray();
  }

  public static void Save(string path, IEnumerable<M> models)
  {
    var configFile = new ConfigFile();

    foreach (var model in models)
    {
      foreach (var field in model.GetType().GetFields())
      {
        var name = field.Name;
        if (name == "Id") continue;

        var value = field.GetValue(model);
        if (value == null) continue;

        var type = field.FieldType;

        configFile.SetValue(model.Id, name, ValueToVariant(type, value));
      }
    }

    configFile.Save(path);
  }

  static object ValueFromVariant(Type type, Variant variant)
  {
    if (type == typeof(double)) return variant.AsDouble();
    else if (type == typeof(int)) return variant.AsInt32();
    else if (type == typeof(string)) return variant.AsString();
    else if (type == typeof(Vector2I)) return variant.AsVector2I();
    else if (type.IsEnum) return Enum.Parse(type, (string)variant);
    else throw new NotImplementedException();
  }

  static Variant ValueToVariant(Type type, object value)
  {
    if (value is double d) return d;
    else if (value is int i) return i;
    else if (value is string s) return s;
    else if (value is Vector2I v) return v;
    else if (type.IsEnum) return Enum.GetName(type, value)!;
    else throw new NotImplementedException();
  }
}