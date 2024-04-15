//============================================================================
// WeeblePlain.cs  Simple model for the Weeble
//============================================================================
using Godot;
using System;

public partial class WeeblePlain : Node3D
{
	CsgSphere3D sphere;
	CsgBox3D cutBox;
	CylinderMesh stubMesh;

	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{

		//GD.Print("WeeblePlain");
		sphere = GetNode<CsgSphere3D>("CSGSphere3D");
		cutBox = GetNode<CsgBox3D>("CSGSphere3D/CSGBox3D");
		stubMesh = (CylinderMesh)GetNode<MeshInstance3D>("Stub").Mesh;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
