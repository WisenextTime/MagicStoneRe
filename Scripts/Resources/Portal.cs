using Godot;
using Godot.Collections;

namespace MagicStoneRe.Scripts.Resources;
[GlobalClass]

public partial class Portal : Item
{
	public Portal()
	{
		Type = ItemType.Portal;
		Data.Add("Waves", new Array<Dictionary<string, int>>());
	}
}