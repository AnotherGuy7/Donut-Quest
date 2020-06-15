using Godot;
using System;

public class Laser : Area2D
{
	private Vector2 velocity;
	private Sprite laserSprite;     //it's really dumb how rotation in Godot messes up velocity
	private CollisionShape2D laserShape;
	private float rotation;
	private bool changedRotation = false;

	public override void _Ready()
	{
		laserSprite = (Sprite)GetNode("LaserSprite");
		laserShape = (CollisionShape2D)GetNode("LaserCollision");
	}

	private void OnLaserBodyEntered(object body)
	{
		if (body != Player.player)
			QueueFree();
		if (body == GameData.currentBoss)
		{
			RigidBody2D enemy = GameData.currentBoss;
			float damageDealt = (float)enemy.Get("health") - GameData.playerDamage;
			enemy.Set("health", damageDealt);
			SoundManager.bossHurtSound.Play();
			QueueFree();
		}
		if (body.GetType().ToString() == "DonutMan")
		{
			DonutMan enemy = body as DonutMan;
			enemy.Hit(GameData.playerDamage);
		}
	}

	private void OnTimeLeftOver()
	{
		QueueFree();
	}

	public override void _PhysicsProcess(float delta)
	{
		if (!changedRotation)
		{
			laserSprite.Rotate(1.571f + rotation);
			laserShape.Rotate(1.571f + rotation);
			changedRotation = true;
			Visible = true;
		}

		MoveLocalX(velocity.x);
		MoveLocalY(velocity.y);
	}
}
