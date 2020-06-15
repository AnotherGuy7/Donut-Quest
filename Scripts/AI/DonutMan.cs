using Godot;
using System;

public class DonutMan : RigidBody2D
{
	[Export]
	public PackedScene sprinkles;

	[Export]
	public PackedScene dmgUpDonut;

	[Export]
	public PackedScene dfUpDonut;

	[Export]
	public PackedScene healthUpDonut;

	private int attackTimer = 0;
	private int healthShowTimer = 0;
	private int dyingTimer = 0;
	private int attackNumber = 0;
	private string direction;
	private AnimatedSprite anim;
	private TextureRect healthRect;
	private TextureRect healthRectOutline;
	private Random rand = new Random();

	private float health = 50f;
	private int damage = 9;
	private bool dying = false;
	private bool canHurtPlayer = true;

	private readonly Color fullyVisible = new Color(1f, 1f, 1f, 1f);
	private readonly Color invisible = new Color(1f, 1f, 1f, 0f);

	public override void _Ready()
	{
		anim = (AnimatedSprite)GetNode("DonutManSprite");
		healthRectOutline = (TextureRect)GetNode("HealthOutline");
		healthRect = (TextureRect)GetNode("HealthOutline/Health");
		healthRectOutline.Modulate = invisible;
		direction = "Front";
	}
	private void OnHurtBoxEntered(object body)
	{
		if (body == Player.player && canHurtPlayer && !dying)
		{
			GameData.HurtPlayer(damage);
			if (attackNumber == 2)
			{
				attackNumber = 0;
				attackTimer = 0;
			}
		}
	}

	private void OnHurtBoxExited(object body)
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

		if (!dying)
		{
			if (attackTimer >= 240)
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
					}
				}
				if (attackNumber == 1)
				{
					Area2D sprinkle = (Area2D)sprinkles.Instance();
					sprinkle.GlobalPosition = GlobalPosition;
					Vector2 projVel = Player.player.GlobalPosition - GlobalPosition;
					sprinkle.Set("velocity", projVel.Normalized() * 1f);
					GameData.currentMap.AddChild(sprinkle);
					attackNumber = 0;
					attackTimer = 0;
				}
				if (attackNumber == 2)
				{
					Vector2 walkDirection = Player.player.GlobalPosition - GlobalPosition;
					velocity = walkDirection.Normalized() * 1.6f;
					if (attackTimer >= 300)
					{
						attackNumber = 0;
						attackTimer = 0;
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
				anim.Play("Walking_" + direction);
			}
			else
			{
				anim.Play("Idle_" + direction);
			}

			MoveLocalX(velocity.x);
			MoveLocalY(velocity.y);
		}
	}

	public void Hit(int damage = 0)     //updates life and shows the health bar
	{
		health -= damage;
		healthShowTimer = 240;
		healthRectOutline.Modulate = fullyVisible;
	}

	public override void _Process(float delta)
	{
		if (health <= 0 || GameData.numberOfEnemiesLeftToKill <= 0)
		{
			dyingTimer++;
			health = 0;
			dying = true;
			anim.Play("Dead");
		}

		healthRect.RectSize = new Vector2((health / 50f) * 12f, 2f);
		if (healthShowTimer > 0)
			healthShowTimer--;
		else
			healthRectOutline.Modulate = invisible;

		if (healthShowTimer < 120)
		{
			healthRectOutline.Modulate = new Color(1f, 1f, 1f, healthShowTimer / 120f);
		}
		if (health <= 0)
		{
			health = 0;
			dying = true;
		}
		if (dyingTimer > 0)
		{
			Modulate = new Color(1f, 1f, 1f, (-dyingTimer + 120) / 120f);
			if (dyingTimer >= 120)
			{
				Die();
			}
		}
	}

	private void Die()
	{
		GameData.numberOfEnemiesLeftToKill -= 1;
		switch (rand.Next(0, 4))
		{
			case 1:
				Area2D donut1 = (Area2D)dmgUpDonut.Instance();
				donut1.GlobalPosition = GlobalPosition;
				GameData.currentMap.AddChild(donut1);
				break;
			case 2:
				Area2D donut2 = (Area2D)dfUpDonut.Instance();
				donut2.GlobalPosition = GlobalPosition;
				GameData.currentMap.AddChild(donut2);
				break;
			case 3:
				Area2D donut3 = (Area2D)healthUpDonut.Instance();
				donut3.GlobalPosition = GlobalPosition;
				GameData.currentMap.AddChild(donut3);
				break;
		}
		QueueFree();
	}
}
