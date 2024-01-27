//============================================================================
// StickPuck.cs  Script for customizing the model 
//============================================================================
using Godot;
using System;

public partial class PendCartModel : Node3D
{
	Node3D RootNode;

	MeshInstance3D[] Wheels;
	CylinderMesh[] WheelsMesh;

	MeshInstance3D[] Pin;
	CylinderMesh PinMesh;

	Vector3 boxSize;
	float wheelRad;
	float wheelThick;

	float pinOverhang;


	public override void _Ready()
	{
		boxSize = new Vector3(0.5f, 0.35f, 0.5f);
		wheelRad = 0.1f;
		wheelThick = 0.01f;

		
	}

	
	public override void _Process(double delta)
	{
	}

	private void SetSize()
	{

	}
}
