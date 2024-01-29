//============================================================================
// GimbalToy.cs
//============================================================================
using Godot;
using System;

public partial class GimbalScene : Node3D
{
	// ModelStuff
	GimbalToy model;

	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{

		float ctrHeight = 1.7f;
		model = GetNode<GimbalToy>("GimbalToy");
		model.Position = new Vector3(0.0f, ctrHeight, 0.0f);
		model.SetUp("Ypr");

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 15.0f;
		camDist = 4.0f;
		camFOV = 55.0f;

		camTg = new Vector3(0.0f, ctrHeight, 0.0f);
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
