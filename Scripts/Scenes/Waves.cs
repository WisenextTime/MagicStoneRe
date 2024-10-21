using System.Linq;
using Godot;
using Godot.Collections;
using MagicStoneRe.Scripts.Resources;

namespace MagicStoneRe.Scripts.Scenes;

public partial class Waves : VBoxContainer
{
	private Window _window;
	private PackedScene _wave;
	private Array<PortalWave> _waves;
	private Array<Wave> _newWaves = new();
	private MapEditor _editor;


	public override void _Ready()
	{
		_window = GetParent().GetParent().GetParent<Window>();
		_wave = ResourceLoader.Load<PackedScene>("res://Scenes/Prefabs/wave.tscn");
		_editor = _editor = GetTree().Root.GetNode<MapEditor>("MapEditor");
	}

	public void EditWave(WavesContain waveContain,int index)
	{
		_window.Title = $"第{index + 1}波";
		_window.Show();
		_waves = waveContain.Waves;
		ReloadWave();
	}

	private void ReloadWave()
	{
		_newWaves.Clear();
		GetTree().CallGroup("Wave",Node.MethodName.QueueFree);
		foreach (var wave in _waves)
		{
			var newWave = _wave.Instantiate<Wave>();
			AddChild(newWave);
			newWave.Id = wave.Id;
			newWave.Delay = wave.Delay;
			newWave.Count = wave.Count;
			_newWaves.Add(newWave);
		}
	}

	public void Close()
	{
		SaveChanges();
		GetTree().CallGroup("Wave",Node.MethodName.QueueFree);
		_window.Hide();	
	}

	private void SaveChanges()
	{
		_waves.Clear();
		foreach (var wave in _newWaves)
		{
			var nowWave = new PortalWave();
			nowWave.Id = wave.Id;
			nowWave.Delay = wave.Delay;
			nowWave.Count = wave.Count;
			_waves.Add(nowWave);

		}
	}

	public void NewWave()
	{
		_waves.Add(new PortalWave());
		var newWave = _wave.Instantiate<Wave>();
		AddChild(newWave);
		newWave.Id = newWave.Id;
		newWave.Delay = newWave.Delay;
		newWave.Count = newWave.Count;
		_newWaves.Add(newWave);
	}

	public void RemoveWave(Wave wave)
	{
		_newWaves.Remove(wave);
		wave.QueueFree();
		SaveChanges();
		ReloadWave();
	}
}