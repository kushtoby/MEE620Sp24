//============================================================================
// PendCartScene.cs
// Code for runing the Godot scene of a planar planar pendulum attached
// a translating cart.
//============================================================================
using Godot;
using System;

public partial class PendCartScene : Node3D
{
	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	// model stuff
	PendCartModel model;

	// data display stuff

	// Mode of operation
	enum OpMode
	{
		SetPosition,
		SetAngle,
		Simulate,
	}

	OpMode opMode;         // operation mode
	float cartX;           // position of cart
	float pendAngle;       // angle of pendulum
	float dxMan;       // amount cart position is changed manually
	float dthetaMan;   // amount angle is changed each time updated manually
	bool manChanged;   // position or angle has been changed manually

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("Pendulum Cart Scene");

		// build the simulation
		float wallHeight = 2.0f;
		double pendLength = 1.5;
		//double pendMass = 1.9;
		//double cartMass = 2.8;
		opMode = OpMode.SetPosition;
		dxMan = 0.03f;
		dthetaMan = 0.03f;
		manChanged = false;

		// build the model
		model = GetNode<PendCartModel>("PendCartModel");
		model.Position = new Vector3(0.0f, wallHeight, 0.0f);
		model.PendulumLength = (float)pendLength;

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 15.0f;
		camDist = 4.0f;
		camFOV = 55.0f;

		camTg = new Vector3(0.0f, wallHeight, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.FOVDeg = camFOV;
		cam.Target = camTg;
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{

		if(opMode == OpMode.SetPosition){
			if(Input.IsActionPressed("ui_right")){
				cartX += dxMan;
				model.SetPositionAngle(cartX, pendAngle);
				//GD.Print(pendAngle);
				manChanged = true;
			}

			if(Input.IsActionPressed("ui_left")){
				cartX -= dxMan;
				model.SetPositionAngle(cartX, pendAngle);
				manChanged = true;
			}
		}
	}
}
