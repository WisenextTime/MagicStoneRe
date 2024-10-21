using System.Reflection.Metadata;
using Godot;
using Godot.Collections;
using MagicStoneRe.Scripts.Resources;

namespace MagicStoneRe.Scripts.Scenes;

public partial class BasicItemEdit : Window
{
	public Item Item;
	
	private Button _texture;

	private SpinBox _x;
	private SpinBox _y;

	private CheckBox _lightable;
	private ColorPicker _lightColor;
	private Button _lightTexture;
	private SpinBox _lightIntensity;
	private SpinBox _lightRange;
	private MapEditor _editor;
	private Button _type;
	
	private ItemType _itemType;

	private PopupMenu _imageType;
	private FileDialog _indexFile;
	private FileDialog _customFile;
	
	private Button _nowButton;

	public override void _Ready()
	{
		_texture = GetNode<Button>("UI/UI/Box/Texture");
		_x = GetNode<SpinBox>("UI/UI/Box/Pos/X/Value");
		_y = GetNode<SpinBox>("UI/UI/Box/Pos/Y/Value");
		_lightable = GetNode<CheckBox>("UI/UI/Box/Lightable");
		_lightColor = GetNode<ColorPicker>("UI/UI/Box/LightColor");
		_lightTexture = GetNode<Button>("UI/UI/Box/LightTexture");
		_lightIntensity = GetNode<SpinBox>("UI/UI/Box/LightIntensity/Value");
		_lightRange = GetNode<SpinBox>("UI/UI/Box/LightRange/Value");
		_editor = GetTree().Root.GetNode<MapEditor>("MapEditor");
		_type = GetNode<Button>("UI/UI/Box/ChangeType");

		_imageType = _editor.GetNode<PopupMenu>("Ui/OpenImageSelect");
		_indexFile = _editor.GetNode<FileDialog>("Ui/OpenIndexImage");
		_customFile = _editor.GetNode<FileDialog>("Ui/OpenCustomImage");
	}

	public void ReadyToPop()
	{
		if (!Visible)return;
		_texture.Icon = Item.Texture;
		_x.Value = Item.Position.X;
		_x.MaxValue = _editor.Map.Size.X - 1;
		_y.Value = Item.Position.Y;
		_y.MaxValue = _editor.Map.Size.Y - 1;
		_lightable.ButtonPressed = Item.Lightable;
		
		_lightColor.Visible =
			_lightRange.GetParent<HSplitContainer>().Visible = 
			_lightIntensity.GetParent<HSplitContainer>().Visible = 
			_lightTexture.Visible = Item.Lightable;
		
		_lightColor.Color = Item.LightColor;
		_lightRange.Value = Item.LightRange;
		_lightIntensity.Value = Item.LightIntensity;
		_lightTexture.Icon = Item.LightTexture;
		SetType((int)Item.Type);
	}

	public void ChangeType()
	{
		_editor.GetNode<PopupMenu>("Ui/NewItemPopup").Popup();
	}
	
	private void SetType(int index)
	{
		_type.Icon = index switch
		{
			1 => IconTexture.Portal,
			2 => IconTexture.Destructible,
			3 => IconTexture.Buffer,
			_ => IconTexture.BasicItem,
		};
		_itemType = (ItemType)index;
	}

	public void SaveChanges()
	{
		Item.Texture = _texture.Icon;
		Item.Position = new Vector2I((int)_x.Value, (int)_y.Value);
		Item.Lightable = _lightable.ButtonPressed;
		Item.LightRange = (int)_lightRange.Value;
		Item.LightIntensity = (int)_lightIntensity.Value;
		Item.Type = _itemType;
		switch (_itemType)
		{
			case ItemType.Portal:
				if (!Item.Data.ContainsKey("Waves")) Item.Data.Add("Waves", new Dictionary<string, int>());
				break;
			case ItemType.Destructible:
				if(!Item.Data.ContainsKey("Hp")) Item.Data.Add("Hp", 0);
				if(!Item.Data.ContainsKey("CanBeAttackedDirectly")) Item.Data.Add("CanBeAttackedDirectly", false);
				if(!Item.Data.ContainsKey("Coin")) Item.Data.Add("Coin", 0);
				break;
		}
		Hide();
		_editor.ReloadTree();
	}

	public void LightChange(bool light)
	{
		_lightColor.Visible =
			_lightRange.GetParent<HSplitContainer>().Visible = 
			_lightIntensity.GetParent<HSplitContainer>().Visible = 
			_lightTexture.Visible = light;
	}

	public void ChangeTexture()
	{
		_nowButton = _texture;
		_imageType.Popup();
	}

	public void ChangeLightTexture()
	{
		_nowButton = _lightTexture;
		_imageType.Popup();
	}

	public void SourceSelect(int index)
	{
		if (index == 0)_indexFile.Popup();
		else _customFile.Popup();
	}

	public void SelectImage(string path)
	{
		_nowButton.Icon = ResourceLoader.Load<Texture2D>(path);
	}
}