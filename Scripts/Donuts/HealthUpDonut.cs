using Godot;
using System;
using System.Collections;

public class HealthUpDonut : Area2D
{
	private void OnBodyEntered(object body)
	{
		if (body == Player.player)
		{
			if (GameData.playerHealth < GameData.playerMaxHealth - 10)
			{
				GameData.playerHealth += 10;
			}
			if (GameData.playerHealth >= GameData.playerMaxHealth - 10)
			{
				GameData.playerHealth = GameData.playerMaxHealth;
			}
			if (!GameData.showedDonutPanel)
			{
				GameData.showedDonutPanel = true;
			}
			SoundManager.powerUpSound.Play();
			QueueFree();
		}
	}
}
