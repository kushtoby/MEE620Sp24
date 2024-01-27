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

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		// build the simulation
		float wallHeight = 2.0f;
		double pendLength = 1.5;
		//double pendMass = 1.9;
		//double cartMass = 2.8;

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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
