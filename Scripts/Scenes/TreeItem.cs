using System;
using Godot;
using Godot.Collections;
using MagicStoneRe.Scripts.Resources;
using Array = Godot.Collections.Array;

namespace MagicStoneRe.Scripts.Scenes;

public partial class TreeItem : Tree
{
	public int Index = -1;
	public ItemType Type = ItemType.BasicItem;
	public Item Item;
	public MapEditor Editor;

	private Godot.TreeItem _root;

	private Dictionary<Godot.TreeItem, int> _items = new();

	public override void _Ready()
	{
		_root = CreateItem();
		_root.SetIcon(0,Type switch
		{
			ItemType.Portal => IconTexture.Portal,
			ItemType.Buffer => IconTexture.Buffer,
			ItemType.Destructible => IconTexture.Destructible,
			_ => IconTexture.BasicItem,
		});
		_root.SetText(0,Type switch
		{
			ItemType.Portal => "敌人",
			ItemType.Buffer => "状态器",
			ItemType.Destructible => "可破坏物",
			_ => "物体",
		});
		_root.SetText(1, $"{Item.Position.X}, {Item.Position.Y}");
		_root.AddButton(2,IconTexture.Edit,0,false,"编辑");
		_root.AddButton(2,IconTexture.Delete,1,false,"删除");
		if(Type == ItemType.Portal)
		{
			_root.AddButton(2, IconTexture.New, 2, false, "新建波次");
			var index = 0;
			foreach (var wave in (Array)Item.Data["Waves"])
			{
				NewWave(index);
				index++;
			}
		}
	}

	public void Pressed(Godot.TreeItem item, int column, int id, int mouseButtonIndex)
	{
		if(item == _root)
		{
			switch (id)
			{
				case 0:
					Editor.EditItem(Item);
					break;
				case 1:
					Editor.Map.Items.Remove(Item);
					Editor.ReloadTree();
					break;
				case 2:
					NewWave(_items.Count);
					var waves = (Array)Item.Data["Waves"];
					waves.Add(new Dictionary<string, int>());
					break;
			}
		}
		else
		{
			
		}
	}

	private void NewWave(int index)
	{
		var newItem = CreateItem(_root, index);
		newItem.SetText(0, $"第{index + 1}波");
		newItem.AddButton(2,IconTexture.Edit,3+2*index,false,"编辑");
		newItem.AddButton(2,IconTexture.Delete,4+2*index,false,"删除");
		_items.Add(newItem, _items.Count);
		CustomMinimumSize = Vector2.Up * (index + 1) * 40;
	}

	public void Fold(Godot.TreeItem item)
	{
		if (_root.Collapsed) CustomMinimumSize = Vector2.Up * 40;
		else CustomMinimumSize = Vector2.Up * (_items.Count + 1) * 40;
	}
}