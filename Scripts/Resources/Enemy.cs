using Godot;

namespace MagicStoneRe.Scripts.Resources;

[GlobalClass]
public partial class Enemy : Resource
{
	[Export] public string Id;
	[Export] public Texture2D Texture;
}