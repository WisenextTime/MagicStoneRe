using Godot;

namespace MagicStoneRe.Scripts.Scenes;

public partial class Waves : VBoxContainer
{
	private Window _window;

	public override void _Ready()
	{
		_window = GetParent().GetParent().GetParent<Window>();
	}
}