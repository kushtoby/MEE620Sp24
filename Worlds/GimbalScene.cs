//============================================================================
// GimbalToy.cs
//============================================================================
using Godot;
using System;

public partial class GimbalScene : Node3D
{
	[Export]
	String modeString = "YPR";
	bool configStrValid;
	String modeStr;
	String[] angNames;
	float[] angles;
	int actvIdx;
	float dTheta;

	// ModelStuff
	GimbalToy model;

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

	Label instructLabel;
	String instStr;

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		GD.Print("Gimbal Scene");

		angNames = new string[3];
		angles = new float[3];
		actvIdx = 0;
		dTheta = 2.0f;

		configStrValid = SetConfig(modeString);

		float ctrHeight = 1.7f;
		model = GetNode<GimbalToy>("GimbalToy");
		model.Position = new Vector3(0.0f, ctrHeight, 0.0f);
		if(configStrValid)
			model.Setup(modeStr);

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

		// Set up data display
		datDisplay = GetNode<UIPanelDisplay>(
			"UINode/MarginContainer/DatDisplay");
		datDisplay.SetNDisplay(5);

		datDisplay.SetDigitsAfterDecimal(2, 1);
		datDisplay.SetDigitsAfterDecimal(3, 1);
		datDisplay.SetDigitsAfterDecimal(4, 1);

		datDisplay.SetLabel(0,"Euler Angles");
		datDisplay.SetValue(0,"");
		datDisplay.SetLabel(1,"Mode");
		datDisplay.SetValue(1, "Manual");
		datDisplay.SetLabel(2, angNames[0] + ">>");
		datDisplay.SetValue(2, angles[0]);
		datDisplay.SetLabel(3, angNames[1]);
		datDisplay.SetValue(3, angles[1]);
		datDisplay.SetLabel(4, angNames[2]);
		datDisplay.SetValue(4, angles[2]);

		// instruction label
		instructLabel = GetNode<Label>(
			"UINode/MarginContainerBL/InstructLabel");
		instStr = "Press <TAB> to switch angles, " +
			"arrow keys to increase/decrease angle, or 0 to "+
			"zero the angle.";
		instructLabel.Text = instStr;
	}

	private bool SetConfig(String mm)
	{
		String mStr = mm.ToUpper();
		if(mStr == "YPR"){
			modeStr = "YPR";
			angNames[0] = "Yaw (deg)";
			angNames[1] = "Pitch (deg)";
			angNames[2] = "Roll (deg)";
			angles[0] = angles[1] = angles[2] = 0.0f;
			return true;
		}

		mStr = "ERROR";
		angNames[0] = angNames[1] = angNames[2] = "ERROR";
		return false;
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		bool angleChanged = false;

		if(Input.IsActionPressed("ui_right")){
			angles[actvIdx] += dTheta;
			angleChanged = true;
		}

		if(Input.IsActionPressed("ui_left")){
			angles[actvIdx] -= dTheta;
			angleChanged = true;
		}

		if(Input.IsActionJustPressed("ui_zero")){
			angles[actvIdx]  = 0.0f;
			angleChanged = true;
		}

		if(angleChanged){
			datDisplay.SetValue(actvIdx+2, angles[actvIdx]);
			model.SetAngles(Mathf.DegToRad(angles[0]), 
				Mathf.DegToRad(angles[1]), Mathf.DegToRad(angles[2]));
		}

		if(Input.IsActionJustPressed("ui_focus_next")){
			datDisplay.SetLabel(actvIdx+2, angNames[actvIdx]);
			++actvIdx;
			if(actvIdx >2)
				actvIdx = 0;
			datDisplay.SetLabel(actvIdx+2, angNames[actvIdx]+">>");
		}
	}
}
