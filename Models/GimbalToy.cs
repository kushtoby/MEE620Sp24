//============================================================================
// GimbalToy.cs
//============================================================================
using Godot;
using System;

public partial class GimbalToy : Node3D
{
	Node3D outerRingNode;
	Node3D middleRingNode;
	Node3D innerRingNode;


	enum EulerAngleMode{
		None,
		YPR,
		YRP,
		RPY,
	}

	EulerAngleMode eulerMode;

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		eulerMode = EulerAngleMode.None;

		outerRingNode = GetNode<Node3D>("OuterRingNode");
		middleRingNode = GetNode<Node3D>("OuterRingNode/MiddleRingNode");
		innerRingNode = GetNode<Node3D>
			("OuterRingNode/MiddleRingNode/InnerRingNode");

		
	}

	public void SetUp(string mm)
	{
		string modeString = mm.ToUpper();

		if(modeString == "YPR")
			SetUpYPR();
	}

	private void SetUpYPR()
	{
		eulerMode = EulerAngleMode.YPR;
		
		Node3D outerRing = GetNode<Node3D>("OuterRingNode/OuterRing");
		outerRing.Rotation = new Vector3(0.5f*Mathf.Pi, 0.5f*Mathf.Pi, 0.0f);

		Node3D onn = GetNode<Node3D>("OuterRingNode/OuterNubNode");
		onn.Rotation = new Vector3(0.0f, 0.0f, 0.5f*Mathf.Pi);
	}
	
	// public override void _Process(double delta)
	// {
	// }
}
