using Godot;
using System;

public class DefenseUpDonut : Area2D
{
	private void OnBodyEntered(object body)
	{
		if (body == Player.player)
		{
			GameData.playerDefense += 2;
			if (!GameData.showedDonutPanel)
			{
				GameData.showedDonutPanel = true;
			}
			SoundManager.powerUpSound.Play();
			QueueFree();
		}
	}
}
