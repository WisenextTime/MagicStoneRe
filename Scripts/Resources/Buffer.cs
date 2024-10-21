using Godot;

namespace MagicStoneRe.Scripts.Resources;
[GlobalClass]
public partial class Buffer : Item
{
	//TODO Buffer
	[Export] public string Buff = "";
	[Export] public float Range = 0;
	[Export] public string Scoping = "";
	
	public Buffer()
	{
		Type = (int)ItemType.Buffer;
		//Data.Add("Buff", "none");
		//Data.Add("Range", 0);
		//Data.Add("Scoping","None");
	}
}