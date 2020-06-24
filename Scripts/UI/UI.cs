using Godot;

public class UI : Control
{
	private TextureRect bossHealthRect;
	private TextureRect bossHealthOutline;
	private TextureRect playerHealthRect;
	private Label damageLabel;
	private Label defenceLabel;
	private Label enemiesToKill;
	private Panel pauseMenu;
	private Panel donutPanel;
	private Sprite darkening;
	private Position2D analogCenter;
	private Sprite analogSprite;

	private float bossMaxHealth = 0f;
	private float bossHealth = 0f;

	private float deathTimer = 0;
	private bool analogHeld = false;

	public override void _Ready()
	{
		bossHealthOutline = (TextureRect)GetNode("Layer1/BossHealthOutline");
		bossHealthRect = (TextureRect)GetNode("Layer1/BossHealthOutline/BossHealth");
		playerHealthRect = (TextureRect)GetNode("Layer1/PlayerHealthOutline/PlayerHealth");
		damageLabel = (Label)GetNode("Layer1/PauseMenu/Damage");
		defenceLabel = (Label)GetNode("Layer1/PauseMenu/Defence");
		enemiesToKill = (Label)GetNode("Layer1/EnemiesToKill");
		pauseMenu = (Panel)GetNode("Layer1/PauseMenu");
		donutPanel = (Panel)GetNode("Layer1/DonutPanel");
		darkening = (Sprite)GetNode("Layer1/Darkening");
		analogCenter = (Position2D)GetNode("Layer1/AnalogButton/Center");
		analogSprite = (Sprite)GetNode("Layer1/AnalogButton/AnalogSprite");
	}

	public override void _Process(float delta)
	{
		if (Player.player != null)
		{
			playerHealthRect.RectSize = new Vector2((GameData.playerHealth / 100f) * 58f, 8f);
		}
		if (GameData.currentBoss != null)
		{
			enemiesToKill.Visible = false;
			bossHealthOutline.Visible = true;
			bossHealthRect.Visible = true;
			if (bossMaxHealth == 0f)
			{
				bossMaxHealth = (float)GameData.currentBoss.Get("maxHealth");
			}
			bossHealth = (float)GameData.currentBoss.Get("health");

			bossHealthRect.RectSize = new Vector2((bossHealth / bossMaxHealth) * 58f, 8f);
		}
		else
		{
			if (GameData.currentMap.GetType().ToString() != "GoldenDonutMap")
			{
				enemiesToKill.Visible = true;
				enemiesToKill.Text = "Enemies To Kill:" + GameData.numberOfEnemiesLeftToKill;
				bossHealthOutline.Visible = false;
				bossHealthRect.Visible = false;
			}
			else
			{
				enemiesToKill.Visible = false;
				bossHealthOutline.Visible = false;
				bossHealthRect.Visible = false;
			}
		}
		if (GameData.showedDonutPanel && !GameData.donutPanelClosed)
		{
			GameData.canPause = false;
			donutPanel.Visible = true;
			GetTree().Paused = true;
		}
		damageLabel.Text = "Damage: " + GameData.playerDamage;
		defenceLabel.Text = "Defence: " + GameData.playerDefense;
		pauseMenu.Visible = GameData.paused;
		if (GameData.playerHealth <= 0)
		{
			deathTimer++;
			if (deathTimer >= 120)
			{
				darkening.Visible = true;
				darkening.Modulate = new Color(0f, 0f, 0f, deathTimer / 240);
				if (deathTimer >= 300)
				{
					GetTree().ChangeScene("res://Scenes/TitleScreen.tscn");
				}
			}
		}
		else
		{
			deathTimer = 0;
			darkening.Visible = false;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventScreenDrag drag && analogHeld && (drag.Position - analogCenter.GlobalPosition).Length() <= 30f)
		{
			analogSprite.GlobalPosition = drag.Position;
			GameData.playerShootVector = analogSprite.Position - analogCenter.Position;
		}
	}

	private void OnAnalogPressed()
	{
		analogHeld = true;
	}

	private void OnAnalogReleased()
	{
		analogHeld = false;
		analogSprite.Position = analogCenter.Position;
		GameData.playerShootVector = Vector2.Zero;
	}

	private void OnXButtonPressed()
	{
		if (donutPanel.Visible)
		{
			GameData.canPause = true;
			donutPanel.Visible = false;
			GameData.donutPanelClosed = true;
			GetTree().Paused = false;
		}
	}


	private void OnContinuePressed()
	{
		SoundManager.clickSound.Play();
		GameData.paused = false;
		pauseMenu.Visible = false;
	}

	private void OnBackToMenuPressed()
	{
		SoundManager.clickSound.Play();
		GameData.paused = false;
		GetTree().ChangeScene("res://Scenes/TitleScreen.tscn");
	}
}
