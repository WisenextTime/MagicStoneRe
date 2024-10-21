using Godot;
using Godot.Collections;

namespace MagicStoneRe.Scripts.Resources;

[GlobalClass]
public partial class Map : Resource
{
	[Export] public string MapName;
	
	[Export] public string Author;
	
	[Export] public Vector2 Size;


	[ExportGroup("Info")] 
	
	[Export] public int InitCoin;

	[Export] private int Difficulty;
	
	[ExportGroup("Contains")]
	
	[Export] public Array<string> Tiles = new();
	
	[Export] public Array<Item> Items = new();
}