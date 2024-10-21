using Godot;
using Godot.Collections;

namespace MagicStoneRe.Scripts.Resources;
[GlobalClass]

public partial class Portal : Item
{
	[Export]public Array<WavesContain> Waves = new();
	public Portal()
	{
		Type = (int)ItemType.Portal;
		//Data.Add("Waves", new Array<PortalWave>());
	}
}