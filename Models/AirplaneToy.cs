//============================================================================
// AirplaneToy.cs
//============================================================================
using Godot;
using System;

public partial class AirplaneToy : Node3D
{
	GeometryInstance3D geo;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		geo = GetNode<GeometryInstance3D>("11805AirplaneV2L2");

	}

	public void SetTransparency(float val)
	{
		geo.Transparency = val;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
