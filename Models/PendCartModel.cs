//============================================================================
// StickPuck.cs  Script for customizing the model 
//============================================================================
using Godot;
using System;

public partial class PendCartModel : Node3D
{
	Node3D RootNode;

	Node3D[] Wheels;

	Vector3 boxSize;
	float wheelRad;
	float wheelThick;

	float pinOverhang;

	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("PendCartModel Ready");
		boxSize = new Vector3(0.5f, 0.35f, 0.5f);
		wheelRad = 0.1f;
		wheelThick = 0.05f;

		RootNode = GetNode<Node3D>("RootNode");
		Wheels = new Node3D[4];
		Wheels[0] = GetNode<Node3D>("RootNode/WheelNode1");
		Wheels[1] = GetNode<Node3D>("RootNode/WheelNode2");
		Wheels[2] = GetNode<Node3D>("RootNode/WheelNode3");
		Wheels[3] = GetNode<Node3D>("RootNode/WheelNode4");


		SetParams();
	}

	
	// public override void _Process(double delta)
	// {
		
	// }

	//------------------------------------------------------------------------
	// SetParams: Sets size parameters of the model
	//------------------------------------------------------------------------
	private void SetParams()
	{
		GD.Print("PendCartModel:SetParams");

		RootNode.Position = new Vector3(0.0f, 0.0f, -0.5f*boxSize.Z - 
			3.5f*wheelThick);

		MeshInstance3D box = GetNode<MeshInstance3D>("RootNode/Box");
		BoxMesh boxMesh = (BoxMesh)box.Mesh;
		boxMesh.Size = boxSize;
		box.Position = new Vector3(0.0f, 0.5f*boxSize.Y + wheelRad, 0.0f);

		Wheels[0].Position = new Vector3(-0.5f*boxSize.X + 0.5f*wheelRad,
			wheelRad, 0.5f*boxSize.Z + wheelThick);
		Wheels[1].Position = new Vector3(0.5f*boxSize.X - 0.5f*wheelRad,
			wheelRad, 0.5f*boxSize.Z + wheelThick);
		Wheels[2].Position = new Vector3(-0.5f*boxSize.X + 0.5f*wheelRad,
			wheelRad, -0.5f*boxSize.Z - wheelThick);
		Wheels[3].Position = new Vector3(0.5f*boxSize.X - 0.5f*wheelRad,
			wheelRad, -0.5f*boxSize.Z - wheelThick);
		
	}

	//------------------------------------------------------------------------
	// Setters
	//------------------------------------------------------------------------
	public float CartLength
	{
		set{
			if(value > 0.05){
				boxSize.X = value;
				SetParams();
			}
		}
	}
}
