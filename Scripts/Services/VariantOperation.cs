using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace MagicStoneRe.Scripts.Services;

public static class VariantOperation
{
	public static Array<T> ToArray<[MustBeVariant]T>(IEnumerable<T> array)
	{
		var newArray = new Array<T>();
		foreach (var t in array)
		{
			newArray.Add(t);
		}
		return newArray;
	}

	public static Vector2 ToGodotVector2(System.Numerics.Vector2 vector)
	{
		return new Vector2(vector.X, vector.Y);
	}

	public static Vector3 ToGodotVector3(System.Numerics.Vector3 vector)
	{
		return new Vector3(vector.X, vector.Y, vector.Z);
	}

	public static Vector2I ToGodotVector2I(Vector2 vector)
	{
		return new Vector2I((int)vector.X, (int)vector.Y);
	}

	public static Vector2I ToGodotVector2I(System.Numerics.Vector2 vector)
	{
		return new Vector2I((int)vector.X, (int)vector.Y);
	}

	public static Vector3I ToGodotVector3I(System.Numerics.Vector3 vector)
	{
		return new Vector3I((int)vector.X, (int)vector.Y, (int)vector.Z);
	}

	public static Vector3I ToGodotVector3I(Vector3 vector)
	{
		return new Vector3I((int)vector.X, (int)vector.Y, (int)vector.Z);
	}

	public static Color ToGodotColor(System.Drawing.Color color)
	{
		return new Color(color.R, color.G, color.B, color.A);
	}
}