using Godot;
using System;

public class InnerCastleMap : Node2D
{
	[Export]
	public PackedScene healingDonut;

	private AnimationPlayer cutscene;
	private CollisionShape2D wall3Shape;
	private Sprite doorClosed;
	private Sprite doorOpen;
	private CollisionShape2D doorShape;
	private Position2D pos1;
	private Position2D pos2;
	private Position2D pos3;
	private Position2D pos4;

	private Random rand = new Random();

	public override void _Ready()
	{
		cutscene = (AnimationPlayer)GetNode("Cutscene");
		wall3Shape = (CollisionShape2D)GetNode("Wall3/Wall3Shape");
		doorClosed = (Sprite)GetNode("Door/DoorSprite");
		doorOpen = (Sprite)GetNode("Door/DoorSpriteOpen");
		doorShape = (CollisionShape2D)GetNode("Door/DoorShape");
		pos1 = (Position2D)GetNode("DonutSpawn1");
		pos2 = (Position2D)GetNode("DonutSpawn2");
		pos3 = (Position2D)GetNode("DonutSpawn3");
		pos4 = (Position2D)GetNode("DonutSpawn4");
		GameData.currentMap = this;
	}

	public override void _Process(float delta)
	{
		if (GameData.currentBoss == null)
		{
			doorClosed.Visible = false;
			doorOpen.Visible = true;
			doorShape.Disabled = false;
		}
		else
		{
			doorClosed.Visible = true;
			doorOpen.Visible = false;
			doorShape.Disabled = true;
		}
		if (rand.Next(0, 250) <= 1)
		{
			Area2D donut = (Area2D)healingDonut.Instance();
			switch (rand.Next(0, 5))
			{
				case 1:
					donut.GlobalPosition = pos1.GlobalPosition;
					break;
				case 2:
					donut.GlobalPosition = pos2.GlobalPosition;
					break;
				case 3:
					donut.GlobalPosition = pos3.GlobalPosition;
					break;
				case 4:
					donut.GlobalPosition = pos4.GlobalPosition;
					break;
			}
			AddChild(donut);
		}
	}

	private void OnCutsceneAreaEntered(object body)
	{
		if (body == Player.player)
		{
			cutscene.Play("Cutscene");
		}
	}

	private void OnCutsceneDone(String anim_name)
	{
		if (anim_name == "Cutscene")
		{
			Player.playerCam.Current = true;
			GameData.currentBoss.Set("spawning", false);
			GameData.currentBoss.Set("dead", false);
			wall3Shape.Disabled = false;
		}
	}

	private void OnCutsceneStarted(String anim_name)
	{
		if (anim_name == "Cutscene")
		{
			GameData.currentBoss.Set("spawning", true);
			GameData.currentBoss.Set("dead", false);
		}
	}

	private void OnDoorEntered(object body)
	{
		if (body == Player.player)
		{
			GetTree().ChangeScene("res://Scenes/GoldenDonutMap.tscn");
		}
	}
}
