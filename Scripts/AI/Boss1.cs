using Godot;
using System;

public class Boss1 : RigidBody2D
{
	[Export]
	public PackedScene sprinkles;

	private int attackTimer = 0;
	private int attackNumber = 0;
	private int shootCooldown = 0;
	private int numberShot = 0;
	private string direction;
	private Vector2 dashDirection;
	private AnimatedSprite bossAnim;
	private Position2D sprinklePos;
	private AudioStreamPlayer2D shootSound;
	private Random rand = new Random();

	private float maxHealth = 3000f;		//used elsewhere
	private float health = 3000f;
	private int damage = 15;
	private bool dead = true;
	private bool spawning = false;
	private bool canHurtPlayer = true;

	public override void _Ready()
	{
		bossAnim = (AnimatedSprite)GetNode("BossSprite");
		sprinklePos = (Position2D)GetNode("SprinklePos");
		shootSound = (AudioStreamPlayer2D)GetNode("SprinkleShootSound");
		direction = "Front";
		GameData.currentBoss = this;
	}

	private void OnDashHitboxEntered(object body)
	{
		if (body == Player.player)
		{
			if (!dead && !spawning)
			{
				if (canHurtPlayer)
				{
					canHurtPlayer = false;
					GameData.HurtPlayer(damage);
				}
			}
			else
			{
				GameData.currentBoss = null;
				QueueFree();
			}
		}
		if (attackNumber == 2)
		{
			attackNumber = 0;
			attackTimer = 0;
			dashDirection = Vector2.Zero;
		}
	}
	private void OnDashHitboxExited(object body)
	{
		if (body == Player.player)
		{
			canHurtPlayer = true;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		Vector2 velocity = Vector2.Zero;
		attackTimer++;

		if (!dead && !spawning)
		{
			if (attackTimer >= 120)
			{
				if (attackNumber == 0)
				{
					if (rand.NextDouble() >= 0.5f)
					{
						attackNumber = 1;
					}
					else
					{
						attackNumber = 2;
						dashDirection = Player.player.GlobalPosition - GlobalPosition;
					}
				}
				if (attackNumber == 1)
				{
					if (shootCooldown > 0)
					{
						shootCooldown--;
					}
					if (shootCooldown <= 0)
					{
						shootCooldown += 15;
						numberShot += 1;
						Vector2 angle = Player.player.GlobalPosition - GlobalPosition;
						Area2D sprinkle = (Area2D)sprinkles.Instance();
						sprinkle.GlobalPosition = GlobalPosition;
						Vector2 projVel = angle;
						sprinkle.Set("velocity", projVel.Normalized() * 1.8f);
						GameData.currentMap.AddChild(sprinkle);
						shootSound.Play();
					}
					if (numberShot >= 9)
					{
						attackNumber = 0;
						attackTimer = 0;
						numberShot = 0;
					}
				}
				if (attackNumber == 2)
				{
					velocity = dashDirection.Normalized() * 1.6f;
					if (attackTimer >= 240)
					{
						attackNumber = 0;
						attackTimer = 0;
						dashDirection = Vector2.Zero;
					}
				}
			}

			if (velocity.y > 0)
			{
				direction = "Back";
			}
			if (velocity.y < 0)
			{
				direction = "Front";
			}
			if (velocity.x > 0)
			{
				direction = "Right";
			}
			if (velocity.x < 0)
			{
				direction = "Left";
			}


			if (velocity != Vector2.Zero)
			{
				bossAnim.Play("Walking_" + direction);
			}
			else
			{
				bossAnim.Play("Idle_" + direction);
			}

			MoveLocalX(velocity.x);
			MoveLocalY(velocity.y);
		}
	}

	public override void _Process(float delta)
	{
		if (health <= 0)
		{
			health = 0;
			dead = true;
			bossAnim.Play("Dying");
		}
		if (spawning)
		{
			bossAnim.Play("Spawning");
		}
	}
}
