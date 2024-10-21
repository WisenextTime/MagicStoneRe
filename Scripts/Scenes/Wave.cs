using Godot;
using MagicStoneRe.Scripts.Resources;

namespace MagicStoneRe.Scripts.Scenes;

public partial class Wave : HBoxContainer
{
	//public int Index;
	private OptionButton _button;
	private SpinBox _count;
	private  SpinBox _delay;
	
	public string Id 
	{
		get => _button.Selected != 0 ? GetGlobal.Enemies[_button.Selected - 1].Id : null;
		set => _button.Selected = value != null ? GetGlobal.EnemyIndex[value] + 1 : 0;
	}

	public int Count
	{
		get => (int)_count.Value;
		set => _count.Value = value;
	}

	public float Delay
	{
		get => (float)_delay.Value;
		set => _delay.Value = value;
	}

	public override void _Ready()
	{
		_button = GetNode<OptionButton>("Enemy");
		_count = GetNode<SpinBox>("Count");
		_delay = GetNode<SpinBox>("Delay");
		foreach (var enemy in GetGlobal.Enemies)
		{
			_button.AddIconItem(enemy.Texture,Tr($"{enemy.Id}_Name"));
		}
	}

	public void Delete()
	{
		GetParent<Waves>().RemoveWave(this);
	}
}