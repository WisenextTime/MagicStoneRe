using Godot;

namespace MagicStoneRe.Scripts.Resources;

[GlobalClass]
public partial class PortalWave : Resource
{
	[Export] public string Id = "";
	[Export] public int Count = 0;
	[Export] public float Delay = 0.1f;
}