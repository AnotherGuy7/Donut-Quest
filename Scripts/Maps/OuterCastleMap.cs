using Godot;
using System;

public class OuterCastleMap : Node2D
{
	[Export]
	public PackedScene donutMen;

	private Position2D spawnPos1;
	private Position2D spawnPos2;
	private Position2D spawnPos3;
	private Position2D spawnPos4;
	private Position2D spawnPos5;
	private Position2D spawnPos6;
	private Position2D spawnPos7;
	private Position2D spawnPos8;
	private Position2D spawnPos9;
	private Sprite doorClosed;
	private Sprite doorOpen;
	private CollisionShape2D doorShape;

	private Random rand = new Random();

	public override void _Ready()
	{
		GameData.currentMap = this;
		spawnPos1 = (Position2D)GetNode("EnemySpawn1");
		spawnPos2 = (Position2D)GetNode("EnemySpawn2");
		spawnPos3 = (Position2D)GetNode("EnemySpawn3");
		spawnPos4 = (Position2D)GetNode("EnemySpawn4");
		spawnPos5 = (Position2D)GetNode("EnemySpawn5");
		spawnPos6 = (Position2D)GetNode("EnemySpawn6");
		spawnPos7 = (Position2D)GetNode("EnemySpawn7");
		spawnPos8 = (Position2D)GetNode("EnemySpawn8");
		spawnPos9 = (Position2D)GetNode("EnemySpawn9");
		doorClosed = (Sprite)GetNode("Door/DoorSprite");
		doorOpen = (Sprite)GetNode("Door/DoorSpriteOpen");
		doorShape = (CollisionShape2D)GetNode("Door/DoorShape");
	}

	private void OnTPAreaMouseEntered()
	{
		GameData.canTeleport = true;
	}


	private void OnTPAreaMouseExited()
	{
		GameData.canTeleport = false;
	}

	private void OnDoorBodyEntered(object body)
	{
		if (body == Player.player)
		{
			GetTree().ChangeScene("res://Scenes/InnerCastleMap.tscn");
		}
	}

	public override void _Process(float delta)
	{
		if (GameData.numberOfEnemiesLeftToKill <= 0 && !doorOpen.Visible)
		{
			doorClosed.Visible = false;
			doorOpen.Visible = true;
			doorShape.Disabled = false;
		}
	}

	private void OnEnemySpawnTimerOut()
	{
		if (GameData.numberOfEnemiesLeftToKill > 0)
		{
			RigidBody2D donutMan = (RigidBody2D)donutMen.Instance();
			switch (rand.Next(0, 10))
			{
				case 1:
					donutMan.GlobalPosition = spawnPos1.GlobalPosition;
					break;
				case 2:
					donutMan.GlobalPosition = spawnPos2.GlobalPosition;
					break;
				case 3:
					donutMan.GlobalPosition = spawnPos3.GlobalPosition;
					break;
				case 4:
					donutMan.GlobalPosition = spawnPos4.GlobalPosition;
					break;
				case 5:
					donutMan.GlobalPosition = spawnPos5.GlobalPosition;
					break;
				case 6:
					donutMan.GlobalPosition = spawnPos6.GlobalPosition;
					break;
				case 7:
					donutMan.GlobalPosition = spawnPos7.GlobalPosition;
					break;
				case 8:
					donutMan.GlobalPosition = spawnPos8.GlobalPosition;
					break;
				case 9:
					donutMan.GlobalPosition = spawnPos9.GlobalPosition;
					break;
			}
			GameData.currentMap.AddChild(donutMan);
		}
	}
}
