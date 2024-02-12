//============================================================================
// UJointPendScene.cs
//============================================================================
using Godot;
using System;

public partial class uJointPendSene : Node3D
{

	Node3D ax1Node;     // node that handles axis1
	Node3D ax2Node;     // node that handles axis2

	string[] angNames;
	float[] angles;
	int actvIdx;
	float dTheta;

	enum OpMode
	{
		Configure,
		Simulate,
	}
	OpMode opMode;         // operation mode

	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	// Data display stuff
	UIPanelDisplay datDisplay;
	int uiRefreshCtr;     //counter for display refresh
	int uiRefreshTHold;   // threshold for display refresh

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{

		ax1Node = GetNode<Node3D>("ModelNode/ConnectorNode");
		ax2Node = GetNode<Node3D>("ModelNode/ConnectorNode/PendNode");

		opMode = OpMode.Configure;
		angNames = new string[2];
		angNames[0] = "theta";  angNames[1] = "phi";
		angles = new float[2];
		angles[0] = 0.0f;
		angles[1] = 0.0f;
		actvIdx = 0;
		dTheta = 2.0f;

		// Set up the camera rig
		longitudeDeg = 20.0f;
		latitudeDeg = 45.0f;
		camDist = 4.0f;
		camFOV = 35.0f;

		camTg = new Vector3(0.0f, 1.4f, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.FOVDeg = camFOV;
		cam.Target = camTg;

		// Set up data display
		datDisplay = GetNode<UIPanelDisplay>(
			"UINode/MarginContainer/DatDisplay");
		datDisplay.SetNDisplay(7);

		datDisplay.SetDigitsAfterDecimal(0, 1);
		datDisplay.SetDigitsAfterDecimal(1, 1);
		datDisplay.SetDigitsAfterDecimal(2, 1);
		datDisplay.SetDigitsAfterDecimal(3, 4);
		datDisplay.SetDigitsAfterDecimal(4, 4);
		datDisplay.SetDigitsAfterDecimal(5, 4);
		datDisplay.SetDigitsAfterDecimal(6, 4);

		datDisplay.SetLabel(0,"Mode");
		datDisplay.SetValue(0, "Configure");
		datDisplay.SetLabel(1, angNames[0] + ">>");
		datDisplay.SetValue(1, angles[0]);
		datDisplay.SetLabel(2, angNames[1]);
		datDisplay.SetValue(2, angles[1]);
		datDisplay.SetLabel(3, "Kinetic");
		datDisplay.SetValue(3, 0.0f);
		datDisplay.SetLabel(4, "Potential");
		datDisplay.SetValue(4, 0.0f);
		datDisplay.SetLabel(5, "Tot. Erg.");
		datDisplay.SetValue(5, 0.0f);
		datDisplay.SetLabel(6, "Ang. Mo. Y");
		datDisplay.SetValue(6, 0.0f);
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
	}
}
