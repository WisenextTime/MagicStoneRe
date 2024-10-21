using System.Linq;
using Godot;
using Godot.Collections;
using MagicStoneRe.Scripts.Resources;

namespace MagicStoneRe.Scripts.Services;

public static class GameMap
{
	public static Map NewMap(Vector2I size, string mapName, string author)
	{
		var map = new Map{Size = size, MapName = mapName, Author = author};
		// while (map.Tiles.Count < size.X * size.Y)
		// {
		// 	map.Tiles.Add(null);
		// }
		map.Tiles.Resize(size.X * size.Y);
		return map;
	}

	public static string NewDebugMap(Vector2I size, string mapName, string author)
	{
		var map = NewMap(size, mapName, author);
		ResourceSaver.Save(map,$"res://Assets/Game/Maps/{map.MapName}.res");
		return map.ResourcePath;
	}
	
	public static string NewUserMap(Vector2I size, string mapName, string author)
	{
		var map = NewMap(size, mapName, author);
		ResourceSaver.Save(map,$"user://Maps/{map.MapName}.res");
		return map.ResourcePath;
	}

	public static Map OpenMap(string path)
	{
		return ResourceLoader.Load<Map>(path);
	}

	public static Map Resize(Map map, int width, int height)
	{
		var newMapTiles = map.Tiles.SkipWhile((_, index) => index%map.Size.X > width || index/map.Size.X > height);
		return new Map
		{
			Size = new Vector2(width, height), MapName = map.MapName, Author = map.Author,
			Tiles = VariantOperation.ToArray(newMapTiles)
		};
	}
}