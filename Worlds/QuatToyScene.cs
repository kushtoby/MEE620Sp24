//============================================================================
// QuatToyScene.cs     Scene for exploring quaternion rotations
//============================================================================
using Godot;
using System;

public partial class QuatToyScene : Node3D
{
	
	// Model
	WeeblePlain model;

	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	// UI stuff
	VBoxContainer vBoxA;    // vbox for the adjustment interface
	VBoxContainer vBoxQ;    // vbox for the quaternion table    

	DatDisplay2 dispAxisAngle;
	Button[] buttonsAxisAngle;
	Button buttonNextRotation;
	Button buttonAbandonRotaton;


	enum Qcat{
		Axle_x,
		Axle_y,
		Axle_z,
		Angle,
	}


	//------------------------------------------------------------------------
	// _Ready: called once
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		//model = GetNode<WeeblePlain>("WeeblePlain");

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 30.0f;
		camDist = 5.0f;
		camFOV = 35.0f;

		camTg = new Vector3(0.0f, 1.0f, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.FOVDeg = camFOV;
		cam.Target = camTg;

		SetupUI();
	}

	private void SetupUI()
	{
		MarginContainer margTL = GetNode<MarginContainer>("UINode/MargTL");
		vBoxA = new VBoxContainer();
		margTL.AddChild(vBoxA);

		MarginContainer margBL = GetNode<MarginContainer>("UINode/MargBL");
		vBoxQ = new VBoxContainer();
		margBL.AddChild(vBoxQ);

		dispAxisAngle = new DatDisplay2(vBoxA);
		dispAxisAngle.SetNDisplay(4,true);
		dispAxisAngle.SetTitle("New Rotation");
		dispAxisAngle.SetLabel(0,"Axle X");
		dispAxisAngle.SetLabel(1,"Axle Y");
		dispAxisAngle.SetLabel(2,"Axle Z");
		dispAxisAngle.SetLabel(3,"Angle");

		
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
