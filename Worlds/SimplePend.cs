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

	UIPanelDisplay datDisplay;
	int uiRefreshCtr;     //counter for display refresh
	int uiRefreshTHold;   // threshold for display refresh

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

		// set up data display
		datDisplay = GetNode<UIPanelDisplay>(
			"UINode/MarginContainer/DatDisplay");
		datDisplay.SetNDisplay(5);
		datDisplay.SetLabel(0,"Mode");
		datDisplay.SetValue(0,opMode.ToString());
		datDisplay.SetLabel(1,"Angle (deg)");
		datDisplay.SetValue(1, 0.0f);
		datDisplay.SetLabel(2,"Kinetic");
		datDisplay.SetValue(2,"---");
		datDisplay.SetLabel(3,"Potential");
		datDisplay.SetValue(3,"---");
		datDisplay.SetLabel(4,"Tot Energy");
		datDisplay.SetValue(4,"---");

		datDisplay.SetDigitsAfterDecimal(1,1);
		datDisplay.SetDigitsAfterDecimal(2,4);
		datDisplay.SetDigitsAfterDecimal(3,4);
		datDisplay.SetDigitsAfterDecimal(4,4);

		uiRefreshCtr = 0;
		uiRefreshTHold = 3;
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
				datDisplay.SetValue(1, Mathf.RadToDeg(pendRotation.Z));
				pModel.Rotation = pendRotation;
			}
			if(Input.IsActionPressed("ui_left")){
				pendRotation.Z -= dthetaMan;
				datDisplay.SetValue(1, Mathf.RadToDeg(pendRotation.Z));
				pModel.Rotation = pendRotation;
			}

			return;
		}

		// angle determined by simulation

		

		// data display
		if(uiRefreshCtr > uiRefreshTHold){

		}

		//pModel.Rotation = pendRotation;
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
