using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using MagicStoneRe.Scripts.Resources;

namespace MagicStoneRe.Scripts;

public class Global
{
	public static class IconTexture
	{
		public static readonly Texture2D Root = ResourceLoader.Load<Texture2D>("res://Assets/Image/IconRoot.png");
		public static readonly Texture2D Buffer = ResourceLoader.Load<Texture2D>("res://Assets/Image/IconBuffer.png");
		public static readonly Texture2D BasicItem = ResourceLoader.Load<Texture2D>("res://Assets/Image/IconItem.png");
		public static readonly Texture2D Portal = ResourceLoader.Load<Texture2D>("res://Assets/Image/IconPortal.png");
		public static readonly Texture2D Destructible = ResourceLoader.Load<Texture2D>("res://Assets/Image/IconDestructible.png");
		public static readonly Texture2D Edit = ResourceLoader.Load<Texture2D>("res://Assets/Image/ItemEdit.png");
		public static readonly Texture2D Delete = ResourceLoader.Load<Texture2D>("res://Assets/Image/ItemDelete.png");
		public static readonly Texture2D New = ResourceLoader.Load<Texture2D>("res://Assets/Image/ItemNew.png");
	}
	
	private static Global _global;
	
	public static Global GetGlobal => _global??= new Global();
	
	public enum ItemType
	{
		BasicItem = 0,
		Portal = 1,
		Buffer = 3,
		Destructible = 2,
	}

	public string NowMapPath;
	
	public List<Tile> Tiles = new();
	public TileSet TileSet = new()
	{
		TileSize = Vector2I.One * 32
	};
	public Dictionary<string,int> TileIndex = new();

	public Global()
	{
		#if DEBUG
		NowMapPath = "res://Assets/Game/Maps/TestMap.res";
		#endif
		PreLoadIndexTiles();
		PreLoadTileSet();
	}

	private void PreLoadTileSet()
	{
		foreach (var tile in Tiles.Select((value, index) => new { value, index }))
		{
			var tileSource = new TileSetAtlasSource();
			tileSource.Texture = tile.value.Texture;
			tileSource.TextureRegionSize = tile.value.Size;
			tileSource.CreateTile(Vector2I.Zero);
			TileSet.AddSource(tileSource,tile.index);
			TileIndex.Add(tile.value.Id,tile.index);
		}
	}

	private void PreLoadIndexTiles()
	{
		foreach (var tile in DirAccess.GetFilesAt("res://Assets/Game/Data/Tile/"))
		{
			Tiles.Add(ResourceLoader.Load<Tile>($"res://Assets/Game/Data/Tile/{tile}"));
		}
	}
}