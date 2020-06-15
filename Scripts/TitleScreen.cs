using Godot;
using System;

public class TitleScreen : Node2D
{
	[Export]
	public PackedScene clouds1;

	[Export]
	public PackedScene clouds2;

	[Export]
	public PackedScene clouds3;

	[Export]
	public PackedScene clouds4;

	private Random rand = new Random();

	private Sprite secondSkyGradient;
	private Sprite controlsDesc;
	private TextureButton play;
	private TextureButton controls;
	private TextureButton quit;
	private AnimationPlayer startScene;
	private int pressTimer = 0;

	public override void _Ready()
	{
		secondSkyGradient = (Sprite)GetNode("ControlsBackground");
		controlsDesc = (Sprite)GetNode("ControlsDesc");
		play = (TextureButton)GetNode("Play");
		controls = (TextureButton)GetNode("Controls");
		quit = (TextureButton)GetNode("Quit");
		startScene = (AnimationPlayer)GetNode("StartScene");
		GameData.currentMap = this;
	}

	public override void _Process(float delta)		//270, 130
	{
		if (pressTimer > 0)
			pressTimer--;

		if (rand.Next(0, 201) <= 5)
		{
			switch (rand.Next(0, 5))
			{
				case 1:
					Sprite cloud = (Sprite)clouds1.Instance();
					cloud.GlobalPosition = new Vector2(270f, 120f + rand.Next(0, 16));
					GameData.currentMap.AddChild(cloud);
					break;
				case 2:
					Sprite cloud2 = (Sprite)clouds2.Instance();
					cloud2.GlobalPosition = new Vector2(270f, 120f + rand.Next(0, 16));
					GameData.currentMap.AddChild(cloud2);
					break;
				case 3:
					Sprite cloud3 = (Sprite)clouds3.Instance();
					cloud3.GlobalPosition = new Vector2(270f, 120f + rand.Next(0, 16));
					GameData.currentMap.AddChild(cloud3);
					break;
				case 4:
					Sprite cloud4 = (Sprite)clouds4.Instance();
					cloud4.GlobalPosition = new Vector2(270f, 120f + rand.Next(0, 16));
					GameData.currentMap.AddChild(cloud4);
					break;
			}
		}
		if (Input.IsKeyPressed((int)KeyList.Escape) && pressTimer <= 0)
		{
			pressTimer += 120;
			if (controlsDesc.Visible)
			{
				controlsDesc.Visible = false;
				secondSkyGradient.Visible = false;
				play.Disabled = false;
				controls.Disabled = false;
				quit.Disabled = false;
			}
		}
		GameData.playerMaxHealth = 100;		//Resetting variables
		GameData.playerHealth = 100;
		GameData.playerDamage = 2;
		GameData.playerDefense = 4;
		GameData.numberOfTeleports = 3;
		GameData.numberOfEnemiesLeftToKill = 30;
}

	private void OnPlayPressed()
	{
		SoundManager.clickSound.Play();
		startScene.Play("BScene");
	}

	private void OnQuitButtonPressed()
	{
		SoundManager.clickSound.Play();
		GetTree().Quit();
	}

	private void OnAnimationFinished(String anim_name)
	{
		if (anim_name == "BScene")
		{
			GetTree().ChangeScene("res://Scenes/OuterCastleMap.tscn");
		}
	}

	private void OnControlsPressed()
	{
		SoundManager.clickSound.Play();
		controlsDesc.Visible = true;
		secondSkyGradient.Visible = true;
		play.Disabled = true;
		controls.Disabled = true;
		quit.Disabled = true;
	}
}
