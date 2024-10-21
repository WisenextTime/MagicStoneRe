using Godot;

namespace MagicStoneRe.Scripts.Resources;

[GlobalClass]
public partial class Tile : Resource
{
	[Export] public string Id;
	
	[Export] public string Description = "";
	
	[Export] public uint Navigation = 0b_0000;
	
	
	[ExportGroup("Image")]
	
	[Export] public Texture2D Texture;
	
	[Export] public Vector2I Size = Vector2I.One * 32;

	[ExportGroup("Function")]
	
	[Export] public bool IsFoundation;
	
	[Export] public bool IsCore;
	
	[Export] private bool IsPortal;

	[Export] public int ProvideHealth;
	
	[Export] public int ProvideCoin;
}
