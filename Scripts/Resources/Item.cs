using Godot.Collections;
using Godot;

namespace MagicStoneRe.Scripts.Resources;

[GlobalClass]
public partial class Item : GodotObject
{
	public Vector2I Position = Vector2I.Zero;
	
	public ItemType Type = ItemType.BasicItem;
	
	public Dictionary Data= new();
	
	public Texture2D Texture = ResourceLoader.Load<Texture2D>("res://Assets/Image/IconNone.png");
	
	
	
	public bool Lightable = false;

	public Texture2D LightTexture = ResourceLoader.Load<Texture2D>("res://Assets/Image/IconNone.png");

	public Color LightColor = Colors.White;
	
	public int LightIntensity = 0;
	
	public int LightRange = 0;
}