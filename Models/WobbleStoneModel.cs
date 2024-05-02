//============================================================================
// WobbleStoneModel.cs
//============================================================================
using Godot;
using System;

public partial class WobbleStoneModel : Node3D
{
	CsgTorus3D torus;
	CsgBox3D blockBox;

	float rHoop;      // hoop radius
	float rRim;       // rim radius       

	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("WobbleStoneModel");
		float rHoop = 0.75f;
		float rRim = 0.25f;

		GetNode<CsgTorus3D>("CSGTorus3D");
		GetNode<CsgBox3D>("CSGBox3D");

		SetSize(rHoop, rRim);
	}

	//------------------------------------------------------------------------
	// SetSize:
	//------------------------------------------------------------------------
	public void SetSize(float rH, float rR)
	{

	}
	
	public override void _Process(double delta)
	{
	}
}
