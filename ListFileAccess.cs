using System;
using System.Collections.Generic;
using System.Globalization;
using CircuitSim.Circuitry;
using Godot;

namespace CircuitSim;

/// <summary>
/// Contains static methods for loading/saving list (.lst) files.
/// </summary>
/// <typeparam name="T">The type of object that is included in the list.</typeparam>
public class ListFileAccess<T> where T : notnull
{
	public static List<T> Load(string path)
	{
		var list = new List<T>();

		var file = new ConfigFile();
		var error = file.Load(path);

		// Return an empty list if nothing has been saved yet.
		if (error == Error.FileNotFound) return list;

		var ids = file.GetSections();

		foreach (var id in ids)
		{
			Type objType = typeof(T);
			T obj;

			if (objType == typeof(ComponentModel))
			{
				ComponentType componentType = (ComponentType)Enum.Parse(typeof(ComponentType), (string)file.GetValue(id, "Type"));

				objType = ComponentTypes.GetModelType(componentType);
				obj = (T)(object)ComponentModel.FromType(componentType);
			}
			else
			{
				var constructor = objType.GetConstructor([typeof(string)]) ?? throw new NotImplementedException($"'{objType}' has no constructor that takes the following parameters: (string id)");
				obj = (T)constructor.Invoke([id]);
			}

			foreach (var field in objType.GetFields())
			{
				var key = field.Name;
				if (key == "Id") continue;

				var type = field.FieldType;
				var variant = file.GetValue(id, key);

				field.SetValue(obj, VariantToFieldValue(type, variant));
			}

			list.Add(obj);
		}

		return list;
	}

	public static void Save(string path, IEnumerable<T> collection)
	{
		var configFile = new ConfigFile();

		foreach (var obj in collection)
		{
			var objType = obj.GetType();
			var objIdField = objType.GetField("Id");

			if (objIdField == null || objIdField.FieldType != typeof(string))
			{
				throw new MissingFieldException("Objects saved in a list file must have a public 'Id' field of type string.");
			}

			var id = (string)objIdField.GetValue(obj)!;

			foreach (var field in objType.GetFields())
			{
				var key = field.Name;
				if (key == "Id") continue;

				var value = field.GetValue(obj);
				if (value == null) continue;

				var type = field.FieldType;

				configFile.SetValue(id, key, FieldValueToVariant(type, value));
			}
		}

		configFile.Save(path);
	}

	static object VariantToFieldValue(Type type, Variant variant)
	{
		if (type == typeof(bool)) return variant.AsBool();
		if (type == typeof(double)) return variant.AsDouble();
		if (type == typeof(int)) return variant.AsInt32();
		if (type == typeof(string)) return variant.AsString();
		if (type == typeof(Vector2I)) return variant.AsVector2I();
		if (type == typeof(DateTime)) return DateTime.ParseExact(variant.AsString(), "o", CultureInfo.InvariantCulture);
		if (type == typeof(Godot.Collections.Array)) return variant;
		if (type.IsEnum) return Enum.Parse(type, (string)variant);

		throw new NotImplementedException();
	}

	static Variant FieldValueToVariant(Type type, object value)
	{
		if (value is bool b) return b;
		if (value is double d) return d;
		if (value is int i) return i;
		if (value is string s) return s;
		if (value is Vector2I v) return v;
		if (value is DateTime t) return t.ToString("o", CultureInfo.InvariantCulture);
		if (value is Godot.Collections.Array a) return a;
		if (type.IsEnum) return Enum.GetName(type, value)!;

		throw new NotImplementedException();
	}
}