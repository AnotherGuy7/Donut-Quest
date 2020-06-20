using Godot;
using System;

public class Player : KinematicBody2D
{
	[Export]
	public PackedScene lasers;

	private AnimatedSprite playerAnim;
	private AudioStreamPlayer2D shootSound1;
	private AudioStreamPlayer2D shootSound2;
	private string direction;
	private float moveSpeed = 1f;
	private int shootCool;

	public static Camera2D playerCam;
	public static Player player;

	private Random rand = new Random();

	public override void _Ready()
	{
		playerAnim = (AnimatedSprite)GetNode("PlayerAnim");
		shootSound1 = (AudioStreamPlayer2D)GetNode("ShootSound");
		shootSound2 = (AudioStreamPlayer2D)GetNode("ShootSound2");
		direction = "Front";
		player = this;
		playerCam = (Camera2D)GetNode("PlayerCam");
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector2 velocity = Vector2.Zero;

		if (GameData.playerHealth > 0)
		{
			if (Input.IsActionPressed("move_up"))
			{
				direction = "Back";
				velocity.y -= moveSpeed;
			}
			if (Input.IsActionPressed("move_down"))
			{
				direction = "Front";
				velocity.y += moveSpeed;
			}
			if (Input.IsActionPressed("move_left"))
			{
				direction = "Left";
				velocity.x -= moveSpeed;
			}
			if (Input.IsActionPressed("move_right"))
			{
				direction = "Right";
				velocity.x += moveSpeed;
			}

			if (velocity != Vector2.Zero)
			{
				playerAnim.Play("Walking_" + direction);
			}
			else
			{
				playerAnim.Play("Idle_" + direction);
			}
			MoveAndCollide(velocity);
		}

		if (GameData.playerHealth <= 0)
		{
			playerAnim.Play("Dead");
		}
	}

	public override void _Process(float delta)
	{
		if (shootCool > 0)
		{
			shootCool--;
		}
		if (Input.IsActionJustPressed("attack") && GameData.playerHealth > 0)
		{
			Area2D laser = (Area2D)lasers.Instance();
			laser.GlobalPosition = GlobalPosition;
			Vector2 projVel = GetGlobalMousePosition() - GlobalPosition;
			laser.Set("velocity", projVel.Normalized() * 2f);
			laser.Set("rotation", projVel.Angle());
			GameData.currentMap.AddChild(laser);
			shootSound1.Play();
		}
		if (GameData.playerShootVector != Vector2.Zero && GameData.playerHealth > 0 && shootCool <= 0)
		{
			shootCool += 15;
			Area2D laser = (Area2D)lasers.Instance();
			laser.GlobalPosition = GlobalPosition;
			Vector2 projVel = GameData.playerShootVector;
			laser.Set("velocity", projVel.Normalized() * 2f);
			laser.Set("rotation", projVel.Angle());
			GameData.currentMap.AddChild(laser);
			shootSound1.Play();
		}
		/*if (Input.IsActionJustPressed("teleport") && GameData.canTeleport && GameData.numberOfTeleports > 0)
		{
			GlobalPosition = GetGlobalMousePosition();
			GameData.numberOfTeleports -= 1;
		}*/
		if (Input.IsKeyPressed((int)KeyList.Escape) && GameData.canPause)
		{
			GameData.paused = true;
		}
	}
}
