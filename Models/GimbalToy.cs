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

	Vector3 rot1;    // first rotation vector
	Vector3 rot2;    // second rotation vector
	Vector3 rot3;    // third rotation vector

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

		rot1 = new Vector3();
		rot2 = new Vector3();
		rot3 = new Vector3();
	}

	//------------------------------------------------------------------------
	// Setup: Takes the requested gimbal configuration and calls the 
	//        appropriate setup routine
	//------------------------------------------------------------------------
	public void Setup(string mm)
	{
		string modeString = mm.ToUpper();

		if(modeString == "YPR")
			SetupYPR();
	}

	//------------------------------------------------------------------------
	// ApplyAngles:
	//------------------------------------------------------------------------
	public void SetAngles(float angle1, float angle2, float angle3)
	{
		if(eulerMode == EulerAngleMode.YPR)
			SetAnglesYPR(angle1, angle2, angle3);
		else{
			GD.PrintErr("ApplyAngles -- Something's wrong.");
		}

		outerRingNode.Rotation = rot1;
		middleRingNode.Rotation = rot2;
		innerRingNode.Rotation = rot3;
	}

	//------------------------------------------------------------------------
	// SetupYPR: Yaw Pich Roll gimbal configuration
	//------------------------------------------------------------------------
	private void SetupYPR()
	{
		eulerMode = EulerAngleMode.YPR;
		
		Node3D outerRing = GetNode<Node3D>("OuterRingNode/OuterRing");
		outerRing.Rotation = new Vector3(0.5f*Mathf.Pi, 0.5f*Mathf.Pi, 0.0f);

		Node3D onn = GetNode<Node3D>("OuterRingNode/OuterNubNode");
		onn.Rotation = new Vector3(0.0f, 0.0f, 0.5f*Mathf.Pi);
	}
	
	//------------------------------------------------------------------------
	// SetAnglesYPR: Yaw Pitch Roll Euler angle application
	//------------------------------------------------------------------------
	private void SetAnglesYPR(float angle1, float angle2, float angle3)
	{
		rot1.Y = angle1;
		rot2.Z = angle2;
		rot3.X = angle3;
	}

	// public override void _Process(double delta)
	// {
	// }
}
