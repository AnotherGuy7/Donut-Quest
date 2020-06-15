using Godot;
using System;

public class Clouds : Sprite
{
	private Random rand = new Random();
	private float xVel = 0f;

	public override void _Ready()
	{
		xVel = rand.Next(1, 10);
	}

	public override void _PhysicsProcess(float delta)
	{
		MoveLocalX(-xVel / 10f);
	}
}
