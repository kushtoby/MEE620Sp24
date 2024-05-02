//============================================================================
// WobbleStoneScene.cs     Scene in which we simulate the wobble stone
//============================================================================
using Godot;
using System;

public partial class WobbleStoneScene : Node3D
{
	// Model
	WobbleStoneModel model;

	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target


	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		model = GetNode<WobbleStoneModel>("WobblestoneModel");

		// Set up the camera rig
		longitudeDeg = 20.0f;
		latitudeDeg = 20.0f;
		camDist = 5.0f;
		camFOV = 35.0f;

		camTg = new Vector3(0.0f, 0.2f, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.FOVDeg = camFOV;
		cam.Target = camTg;
	}

	//------------------------------------------------------------------------
	// 
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
	}
}
