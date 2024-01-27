//============================================================================
// StickPuck.cs  Script for customizing the model 
//============================================================================
using Godot;
using System;

public partial class StickPuck : Node3D
{
	MeshInstance3D stick;
	BoxMesh stickMesh;
	MeshInstance3D puck;
	CylinderMesh puckMesh;

	float stickLength;  // length of stick
	float stickThick;    // diameter of stick
	float puckDiam;     // diameter of ball
	float puckOverHang; // how much wider puck is (on each side of stick)

	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
