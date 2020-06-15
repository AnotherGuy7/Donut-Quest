using Godot;
using System;

public class SoundManager : Node2D
{
	public static AudioStreamPlayer2D clickSound;
	public static AudioStreamPlayer2D powerUpSound;
	public static AudioStreamPlayer2D hurtSound;
	public static AudioStreamPlayer2D bossHurtSound;

	public override void _Ready()
	{
		clickSound = (AudioStreamPlayer2D)GetNode("ClickSound");
		powerUpSound = (AudioStreamPlayer2D)GetNode("PowerUpSound");
		hurtSound = (AudioStreamPlayer2D)GetNode("HurtSound");
		bossHurtSound = (AudioStreamPlayer2D)GetNode("BossHurtSound");
	}
}
