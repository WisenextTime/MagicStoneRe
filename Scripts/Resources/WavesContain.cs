using Godot;
using Godot.Collections;

namespace MagicStoneRe.Scripts.Resources;
[GlobalClass]
public partial class WavesContain : Resource
{
	[Export] public Array<PortalWave> Waves = new();
}