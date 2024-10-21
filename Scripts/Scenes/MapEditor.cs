using System;
using System.Linq;
using Godot;
using Godot.Collections;
using MagicStoneRe.Scripts.Resources;
using MagicStoneRe.Scripts.Services;
using Buffer = MagicStoneRe.Scripts.Resources.Buffer;

namespace MagicStoneRe.Scripts.Scenes;

public partial class MapEditor : Node2D
{
	public Map Map;
	
	private TileMapLayer _map;
	private ItemList _tileList;
	private Button _tileSelect;
	private Camera2D _camera;
	private VBoxContainer _items;
	private Waves _waves;
	private HSlider _zoom;
	//private Tree _itemTree;
	//private TreeItem _rootItem;

	private PackedScene _item;
	private bool _mouseButtonLeftPressed;
	private bool _mouseButtonRightPressed;

	public override void _Ready()
	{
		_tileList = GetNode<ItemList>("Ui/UiBase/Tiles");
		_map = GetNode<TileMapLayer>("Map");
		_tileSelect = GetNode<Button>("Ui/UiBase/TopBar/TileSelect");
		_camera = GetNode<Camera2D>("Camera");
		_items = GetNode<VBoxContainer>("Ui/UiBase/Items/Tree/Items");
		_waves = GetNode<Waves>("Ui/WaveEditor/UI/UI/Waves");
		_item = ResourceLoader.Load<PackedScene>("res://Scenes/Prefabs/tree_item.tscn");
		_zoom = GetNode<HSlider>("Ui/UiBase/TopBar/Zoom");
		//_itemTree = GetNode<Tree>("Ui/UiBase/Items/Tree");
		Map = GameMap.OpenMap(GetGlobal.NowMapPath);
		_camera.GlobalPosition = Map.Size * 16;
		DrawMap();
		LoadUi();
		ReloadTree();
		_tileList.Select(0);
	}

	public void ReloadTree()
	{
		GetTree().CallGroup("TreeItem",Node.MethodName.QueueFree);
		//_itemTree.Clear();
		//_rootItem = _itemTree.GetRoot();
		//_rootItem = _itemTree.CreateItem(_itemTree.GetRoot(), 0);
		//_rootItem.SetIcon(0,IconTexture.Root);
		//_rootItem.SetText(0,"物体");
		foreach (var item in Map.Items.Select((item, index) => new { item, index }))
		{
			NewTreeItem((ItemType)item.item.Type,item.index);
		}
	}

	private void NewTreeItem(ItemType type, int index)
	{
		var newItem = _item.Instantiate<TreeItem>();
		newItem.Type = type;
		newItem.Index = index;
		newItem.Item = Map.Items[index];
		newItem.Editor = this;
		_items.AddChild(newItem);
	}

	private void LoadUi()
	{
		foreach (var tile in GetGlobal.Tiles)
		{
			_tileList.AddItem(Tr($"{tile.Id}_Name"), tile.Texture);
		}
	}

	private void DrawMap()
	{
		_map.TileSet = (TileSet)GetGlobal.TileSet.Duplicate();
		foreach (var tile in Map.Tiles.Select((tile, index) => new { tile, index }))
		{
			var pos = new Vector2I((int)(tile.index%Map.Size.X), (int)(tile.index/Map.Size.X));
			if(tile.tile == "") continue;
			_map.SetCell(pos,GetGlobal.TileIndex[tile.tile],Vector2I.Zero);
		}
	}

	public void InfoButton(bool pressed)
	{
		var InfoPanel = GetNode<Panel>("Ui/UiBase/InfoPanel");
		InfoPanel.Visible = pressed;
		if (pressed)
		{
			InfoPanel.GetNode<LineEdit>("MapName").Text = Map.MapName;
			InfoPanel.GetNode<LineEdit>("MapAuthor").Text = Map.Author;
		}
		else
		{
			Map.MapName = InfoPanel.GetNode<LineEdit>("MapName").Text;
			Map.Author = InfoPanel.GetNode<LineEdit>("MapAuthor").Text;
		}
	}

	public void ButtonItem(bool pressed)
	{
		GetNode<Panel>("Ui/UiBase/Items").Visible = pressed;
	}

	public void ResizeMapPopup()
	{
		var popup = GetNode<PopupPanel>("Ui/ResizePopup");
		popup.GetNode<LineEdit>("UI/X").Text = $"{Map.Size.X}";
		popup.GetNode<LineEdit>("UI/Y").Text = $"{Map.Size.Y}";
		popup.Popup();
	}

	public void ResizeMap()
	{
		GameMap.Resize(Map,
			int.Parse(GetNode<LineEdit>("Ui/ResizePopup/UI/X").Text),
			int.Parse(GetNode<LineEdit>("Ui/ResizePopup/UI/Y").Text));
		ResourceSaver.Save(Map,GetGlobal.NowMapPath);
		_Ready();
	}

	public void TileSelect(bool pressed)
	{
		_tileList.Visible = pressed;
	}

	public void TileSelectSuccess(int index)
	{
		_tileSelect.Icon = index == 0 ? 
			ResourceLoader.Load<Texture2D>("res://Assets/Image/Erase.png") : 
			GetGlobal.Tiles[index-1].Texture;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		switch (@event)
		{
			case InputEventMouseButton button:
				switch (button.ButtonIndex)
				{
					case MouseButton.Left:
						_mouseButtonLeftPressed = button.Pressed;
						if (button.Pressed) DrawTile(GetGlobalMousePosition());
						break;
					
					case MouseButton.Right:
						_mouseButtonRightPressed = button.Pressed;
						break;
					case MouseButton.WheelUp:
						_zoom.Value += 0.1;
						break;
					case MouseButton.WheelDown:
						_zoom.Value -= 0.1;
						break;
				}
				break;
			
			case InputEventMouseMotion motion:
				if (_mouseButtonRightPressed)
				{
					var tureRelative = _camera.GlobalPosition - motion.Relative / _camera.Zoom.X;
					if (tureRelative.X < 0) tureRelative.X = 0;
					if (tureRelative.Y < 0) tureRelative.Y = 0;
					if (tureRelative.X > Map.Size.X * 32) tureRelative.X = Map.Size.X * 32;
					if (tureRelative.Y > Map.Size.Y * 32) tureRelative.Y = Map.Size.Y * 32;
					_camera.GlobalPosition = tureRelative;
				}
				else if (_mouseButtonLeftPressed) DrawTile(GetGlobalMousePosition());
				break;
		}
	}

	private void DrawTile(Vector2 position)
	{
		var truePosition = VariantOperation.ToGodotVector2I(position / 32);
		if(truePosition.X < 0 || truePosition.Y < 0 || truePosition.X >= Map.Size.X || truePosition.Y >= Map.Size.Y) return;
		var index = _tileList.GetSelectedItems()[0] - 1;
		if(index == -1)
		{
			Map.Tiles[(int)(truePosition.Y * Map.Size.X + truePosition.X)] = "";
			_map.SetCell(truePosition);
		}
		else
		{
			Map.Tiles[(int)(truePosition.Y * Map.Size.X + truePosition.X)] = 
				GetGlobal.TileIndex.Keys.ToArray()[index];
			_map.SetCell(truePosition, index, Vector2I.Zero);
		}
	}

	public void NewItem()
	{
		if(GetNode<BasicItemEdit>("Ui/ItemEdit").Visible)return;
		GetNode<PopupMenu>("Ui/NewItemPopup").Popup();
	}

	/*private void NewTreeItem(int index,int id)
	{
		var newItem = _itemTree.CreateItem(_rootItem,id + 1);
		newItem.SetIcon(0,index switch
		{
			1 => IconTexture.Portal,
			2 => IconTexture.Destructible,
			3 => IconTexture.Buffer,
			_ => IconTexture.BasicItem,
		});
		newItem.SetText(0,index switch
		{
			1 => "敌人",
			2 => "可破坏物",
			3 => "状态器",
			_ => "基础物体"
	
		});
		switch (index)
		{
			case 1:
				break;
			case 2:
				break;
			case 3:
				//TODO Buffer
				break;
		}
		newItem.AddButton(0,IconTexture.Edit,id + 1);
	}*/

	public void NewItemOk(int index)
	{
		if (GetNode<BasicItemEdit>("Ui/ItemEdit").Visible) return;
		Map.Items.Add(index switch
		{
			1 => new Portal(),
			2 => new Destructible(),
			3 => new Buffer(),
			_ => new Item()
		});
		NewTreeItem((ItemType)index, Map.Items.Count - 1);
	}
	/*public void SelectItem()
	{
		GetNode<Button>("Ui/UiBase/Items/Buttons/ItemDelete").Disabled = false;
	}*/

	public void EditItem(Item item)
	{
		var itemEditor = GetNode<BasicItemEdit>("Ui/ItemEdit");
		itemEditor.Item = item;
		itemEditor.Visible = true;
	}

	/*public void DeleteItem()
	{
		Map.Items.Remove();
		ReloadTree();
	}*/
	public void EditWave(Item item, int id)
	{
		var portal = (Portal)item;
		var data = portal.Waves;
		_waves.EditWave(data[id],id);
	}

	public void ButtonSave(bool pressed)
	{
		if (!pressed) return;
		ResourceSaver.Save(Map, GetGlobal.NowMapPath);
		GetNode<AcceptDialog>("Ui/SaveSuccess").Popup();
	}

	public void Zoom(double zoom)
	{
		_camera.Zoom = Vector2.One * (float)Math.Pow(2, zoom);
	}
}