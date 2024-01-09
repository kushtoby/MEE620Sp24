//============================================================================
// DoublePendScene.cs
// Code for runing the Godot scene of a planar double pendulum
//============================================================================
using Godot;
using System;

public partial class DoublePendScene : Node3D
{
	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	// Model stuff
	StickBall pModel1;
	StickBall pModel2;
	double pendLen1;
	double pendLen2;


	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("Double Pendulum Scene");

		// build the simulation
		pendLen1 = 0.9;
		pendLen2 = 0.7;

		// build the model
		float mountHeight = 1.9f;
		Node3D mnt = GetNode<Node3D>("Axle");
		mnt.Position = new Vector3(0.0f, mountHeight, 0.0f);
		var sbScene = GD.Load<PackedScene>("res://Models/StickBall.tscn");
		pModel1 = (StickBall)sbScene.Instantiate();
		AddChild(pModel1);
		pModel1.Position = new Vector3(0.0f, mountHeight, 0.0f);
		pModel1.Length = (float)pendLen1;
		pModel1.BallDiameter = 0.25f;

		pModel2 = (StickBall)sbScene.Instantiate();
		pModel1.AddChild(pModel2);
		pModel2.Position = new Vector3(0.0f, -(float)pendLen1, 0.0f);
		pModel2.Length = (float)pendLen2;
		pModel2.BallDiameter = 0.25f;

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 15.0f;
		camDist = 2.7f;

		camTg = new Vector3(0.0f, mountHeight, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.Target = camTg;
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
	}

	//------------------------------------------------------------------------
    // _PhysicsProcess:
    //------------------------------------------------------------------------
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

	}
}
