using Godot;
using System;

public class Sprinkle : Area2D
{
	private Vector2 velocity;
	private Sprite sprinkleSprite;     //it's really dumb how rotation in Godot messes up velocity
	private float rotation;
	private int colorNumber = 0;
	private Random rand = new Random();

	public override void _Ready()
	{
		sprinkleSprite = (Sprite)GetNode("SprinkleSprite");
		colorNumber = rand.Next(0, 4);
	}

	private void OnSprinkleBodyEntered(object body)
	{
		if (body != GameData.currentBoss && body.GetType().ToString() != "DonutMan")
			QueueFree();
		if (body == Player.player)
		{
			GameData.HurtPlayer(6);
			QueueFree();
		}
	}

	private void OnTimeLeftOver()
	{
		QueueFree();
	}

	public override void _PhysicsProcess(float delta)
	{
		rotation += 0.02f;
		sprinkleSprite.Rotate(rotation);


		MoveLocalX(velocity.x);
		MoveLocalY(velocity.y);
	}

	public override void _Process(float delta)
	{
		if (Modulate == new Color(1f, 1f, 1f))
		{
			if (colorNumber == 0)
			{
				Modulate = new Color(1f, 0.1f, 0);
			}
			if (colorNumber == 1)
			{
				Modulate = new Color(0f, 1f, 0.1f);
			}
			if (colorNumber == 2)
			{
				Modulate = new Color(0.1f, 0f, 1f);
			}
			if (colorNumber == 3)
			{
				Modulate = new Color(1f, 0f, 1f);
			}
		}
	}
}
