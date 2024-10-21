using Godot;

namespace MagicStoneRe.Scripts.Resources;
[GlobalClass]

public partial class Destructible : Item
{

	[Export] public int Hp = 0;
	[Export] public bool CanBeDestroyed = false;
	[Export] public int Coin = 0;
	public Destructible()
	{
		Type = (int)ItemType.Destructible;
	}
}