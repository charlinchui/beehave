using Godot;
using System;

public partial class BasicUnit : CharacterBody2D
{
	public int health = 100;
	public int damage = 20;
	public float moveSpeed = 150.0f;
	public float range = 0f;
	public float attackRate = .5f;
	public float lastAttack;
	public bool isSelected = true;

	public Vector2 target;
	public NavigationAgent2D agent;
	public Sprite2D sprite;

	public override void _Ready()
	{
		agent = GetNode<NavigationAgent2D>("NavigationAgent2D");
		sprite = GetNode<Sprite2D>("Sprite2D");
		target = GlobalPosition;
	}

	public void takeDamage(int damageTaken)
	{
		health -= damageTaken;
		if (health <= 0)
		{
			QueueFree();
		}
	}

	void SetTarget(Vector2 newTarget)
	{
		target = newTarget;
	}
	void TargetCheck()
	{
		float dist = GlobalPosition.DistanceTo(target);
		if (dist < range)
		{
			agent.TargetPosition = GlobalPosition;
		}
		else
		{
			agent.TargetPosition = target;
		}
	}

	public override void _Process(double delta)
	{
		TargetCheck();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (agent.IsNavigationFinished())
		{
			return;
		}
		Vector2 dir = GlobalPosition.DirectionTo(agent.GetNextPathPosition());
		Velocity = dir * moveSpeed;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseButtonEvent)
		{
			if (isSelected)
			{
				if (mouseButtonEvent.Pressed && (int)mouseButtonEvent.ButtonIndex == 2)
				{
					Vector2 selectedPosition = GetGlobalMousePosition();
					target = selectedPosition;
					agent.TargetPosition = target;
				}
			}
		}
	}
}
