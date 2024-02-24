//============================================================================
// StickPuck.cs  Script for customizing the model 
//============================================================================
using Godot;
using System;

public partial class TopDiskModel : Node3D
{
	Node3D PrecessNode;
	Node3D LeanNode;
	Node3D SpinNode;
	
	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		
		PrecessNode = GetNode<Node3D>("PrecessNode");
		LeanNode = GetNode<Node3D>("PrecessNode/LeanNode");
		SpinNode = GetNode<Node3D>("PrecessNode/LeanNode/SpinNode");

		
	}

	
	// public override void _Process(double delta)
	// {
	// }
}
