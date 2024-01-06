//============================================================================
// SimplePend.cs
// Code for runing the Godot scene of a simple pendulum
//============================================================================
using Godot;
using System;

public partial class SimplePend : Node3D
{
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	StickBall pModel;    // 3D model of pendulum

	enum OpMode
	{
		Manual,
		Sim
	}

	OpMode opMode;    // operation mode
	Vector3 pendRotation;
	float dthetaMan;

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("Hello MEE 620");

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 15.0f;
		camDist = 2.7f;

		camTg = new Vector3(0.0f, 1.0f, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.Target = camTg;

		// Set up simulation
		opMode = OpMode.Manual;
		pendRotation = new Vector3();
		dthetaMan = 0.03f;


		// Set up model
		float mountHeight = 1.4f;
		Node3D mnt = GetNode<Node3D>("Axle");
		mnt.Position = new Vector3(0.0f, mountHeight, 0.0f);
		pModel = GetNode<StickBall>("StickBall");
		pModel.Position = new Vector3(0.0f, mountHeight, 0.0f);
		//pModel.Length = 1.1f;
		//pModel.StickDiameter = 0.15f;
		//pModel.BallDiameter = 0.6f;
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		if(opMode == OpMode.Manual){  // change angle manually
			if(Input.IsActionPressed("ui_right")){
				pendRotation.Z += dthetaMan;
			}
			if(Input.IsActionPressed("ui_left")){
				pendRotation.Z -= dthetaMan;
			}
		}
		else{   // angle determined by simulation

		}

		pModel.Rotation = pendRotation;
	}

    //------------------------------------------------------------------------
    //
    //------------------------------------------------------------------------
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		if(opMode == OpMode.Manual)
			return;

    }
}
