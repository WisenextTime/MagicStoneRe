using Godot;

namespace MagicStoneRe.Scripts.Resources;
[GlobalClass]

public partial class Destructible : Item
{
	public Destructible()
	{
		Type = ItemType.Destructible;
		Data.Add("Hp", 0);
		Data.Add("CanBeAttackedDirectly", false);
		Data.Add("Coin", 0);
	}
}