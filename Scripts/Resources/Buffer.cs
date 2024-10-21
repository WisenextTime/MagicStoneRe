using Godot;

namespace MagicStoneRe.Scripts.Resources;
[GlobalClass]
public partial class Buffer : Item
{
	//TODO Buffer
	public Buffer()
	{
		Type = ItemType.Buffer;
		Data.Add("Buff", "none");
		Data.Add("Range", 0);
		Data.Add("Scoping","None");
	}
}