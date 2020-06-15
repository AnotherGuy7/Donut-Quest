using Godot;
using System;

public class GoldenDonutMap : Node2D
{
	private AnimationPlayer endingScene;

	public override void _Ready()
	{
		endingScene = (AnimationPlayer)GetNode("EndingScene");
		GameData.currentMap = this;
	}

	private void OnEndingBodyEntered(object body)
	{
		if (body == Player.player)
		{
			endingScene.Play("EndingCutscene");
		}
	}

	private void OnEndingSceneFinished(String anim_name)
	{
		if (anim_name == "EndingCutscene")
		{
			GetTree().ChangeScene("res://Scenes/TitleScreen.tscn");
		}
	}
}
