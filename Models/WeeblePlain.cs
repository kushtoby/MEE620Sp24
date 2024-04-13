//============================================================================
// WeeblePlain.cs  Simple model for the Weeble
//============================================================================
using Godot;
using System;

public partial class WeeblePlain : Node3D
{
	CsgSphere3D sphere;
	CsgBox3D cutBox;

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
