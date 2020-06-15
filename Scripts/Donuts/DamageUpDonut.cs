using Godot;
using System;

public class DamageUpDonut : Area2D
{
	private void OnBodyEntered(object body)
	{
		if (body == Player.player)
		{
			GameData.playerDamage += 2;
			if (!GameData.showedDonutPanel)
			{
				GameData.showedDonutPanel = true;
			}
			SoundManager.powerUpSound.Play();
			QueueFree();
		}
	}
}
