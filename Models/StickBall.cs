//============================================================================
// StickBall.cs  Script for customizing the model 
//============================================================================
using Godot;
using System;

public partial class StickBall : Node3D
{
	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("StickBall:_Ready");
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
	}
}
