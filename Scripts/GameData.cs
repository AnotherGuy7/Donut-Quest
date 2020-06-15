using Godot;
using System;

public class GameData : Node2D
{
	public static Node2D currentMap;
	public static RigidBody2D currentBoss;

	public static int playerMaxHealth = 100;
	public static int playerHealth = 100;
	public static int playerDamage = 2;
	public static int playerDefense = 4;
	public static int numberOfTeleports = 3;
	public static int numberOfEnemiesLeftToKill = 30;

	public static bool canTeleport = false;
	public static bool paused = false;
	public static bool showedDonutPanel = false;
	public static bool donutPanelClosed = false;
	public static bool canPause = true;

	private int teleportRegenTimer = 0;

	public static void HurtPlayer(int damage)
	{
		if (damage > playerDefense)
		{
			playerHealth -= damage - playerDefense;
		}
		else
		{
			playerHealth -= 1;
		}
		SoundManager.hurtSound.Play();
	}

	public override void _Process(float delta)
	{
		if (numberOfTeleports < 3)
		{
			teleportRegenTimer++;
			if (teleportRegenTimer >= (15 * 60))
			{
				numberOfTeleports += 1;
				teleportRegenTimer = 0;
			}
		}
		if (numberOfEnemiesLeftToKill <= 0)
		{
			numberOfEnemiesLeftToKill = 0;
		}
		GetTree().Paused = paused;
	}
}
