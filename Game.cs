using System;
using System.Reflection.Metadata;
using Godot;
using Godot.Collections;

public partial class Game : Node2D
{
	public bool dragging = false;
	public Array<Dictionary> selected = new Array<Dictionary>();
	public Vector2 dragStart = Vector2.Zero;
	RectangleShape2D selectionBox = new();

    public override void _UnhandledInput(InputEvent @event)
    {
		if(@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.ButtonIndex == MouseButton.Left){
			if(mouseButtonEvent.Pressed){
				if(selected.Count == 0){
					dragging = true;
					dragStart = mouseButtonEvent.Position;
				}
			} else {
				dragging = false;
				QueueRedraw();
				Vector2 dragEnd = mouseButtonEvent.Position;
				selectionBox.Size = (dragEnd - dragStart) / 2;
				PhysicsDirectSpaceState2D space = GetWorld2D().DirectSpaceState;
				PhysicsShapeQueryParameters2D query = new();
				query.Shape = selectionBox;
				query.Transform = new Transform2D(0, (dragEnd + dragStart) / 2);
				selected = space.IntersectShape(query);
				GD.Print(selected);
			}
		}
		if(@event is InputEventMouseMotion && dragging){
			QueueRedraw();
		}
    }
    public override void _Draw()
    {
        if(dragging) {
			DrawRect(new Rect2(dragStart, GetGlobalMousePosition() - dragStart), new Color(255,255,0), false);
		}
    }
}