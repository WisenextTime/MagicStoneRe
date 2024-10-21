using Godot.Collections;
using Godot;

namespace MagicStoneRe.Scripts.Resources;

[GlobalClass]
public partial class Item :Resource
{
	[Export]public Vector2I Position = Vector2I.Zero;
	
	[Export]public int Type = (int)ItemType.BasicItem;
	
	//[Export]public Dictionary Data= new();
	
	[Export]public Texture2D Texture = ResourceLoader.Load<Texture2D>("res://Assets/Image/IconNone.png");
	
	
	
	[Export]public bool Lightable = false;

	[Export]public Texture2D LightTexture = ResourceLoader.Load<Texture2D>("res://Assets/Image/IconNone.png");

	[Export]public Color LightColor = Colors.White;
	
	[Export]public int LightIntensity = 0;
	
	[Export]public int LightRange = 0;
}