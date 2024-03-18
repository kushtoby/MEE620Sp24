using Godot;
using System;

public partial class SpinTopScene : Node3D
{

	enum OpMode
	{
		Configure,
		Simulate,
	}
	OpMode opMode;         // operation mode

	// Simulation
	SpinTopSim sim;
	double time;
	int nSimSteps;         // number of sim steps per _PhysicsProcess

	// UI
	Button[] adjButtons;
	float leanICDeg;       // initial lean angle in degrees
	float leanICMin;
	float leanICMax;
	float dAngle;

	double spinRate;       // initial spinRate;
	float dumAngle;

	// Model
	TopDiskModel model;    // model

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

	// More UI stuff
	Button simButton;

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{
		
		opMode = OpMode.Configure;

		// ui
		leanICDeg = 30.0f;
		leanICMin = 5.0f;
		leanICMax = 170.0f;
		dAngle = 1.0f;

		

		// set up the model
		model = GetNode<TopDiskModel>("TopDiskModel");
		dumAngle = 0.0f;
		model.SetEulerAnglesYZY(0.0f, Mathf.DegToRad(leanICDeg), 0.0f);

		// Set up the simulation
		sim = new SpinTopSim();
		spinRate = 15.0;
		sim.LeanAngle = Mathf.DegToRad(leanICDeg);
		sim.SpinRate = spinRate;

		nSimSteps = 1;
		time = 0.0;

		// Set up the camera rig
		longitudeDeg = 20.0f;
		latitudeDeg = 25.0f;
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

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		if(opMode == OpMode.Simulate){
			
			return;
		}

		if(adjButtons[0].ButtonPressed){
            leanICDeg += dAngle;
            ProcessLeanAngle();
        }
        else if(adjButtons[3].ButtonPressed){
            leanICDeg -= dAngle;
            ProcessLeanAngle();
        }
	}

	//------------------------------------------------------------------------
	// _PhysicsProcess:
	//------------------------------------------------------------------------
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		if(opMode != OpMode.Simulate)
			return;

		double subdelta = delta/((double)nSimSteps);
		for(int i=0;i<nSimSteps;++i){
			sim.Step(time, subdelta);
			time += subdelta;
		}
    }

	//------------------------------------------------------------------------
	// ProcessLeanAngle
	//------------------------------------------------------------------------
	private void ProcessLeanAngle()
	{
		//GD.Print("Process Lean Angle");

		if(leanICDeg < leanICMin)
			leanICDeg = leanICMin;

		if(leanICDeg > leanICMax)
			leanICDeg = leanICMax;

		sim.ResetIC((double)Mathf.DegToRad(leanICDeg), 10.0f);
		model.SetEulerAnglesYZY(0.0f,Mathf.DegToRad(leanICDeg), 0.0f);
		datDisplay.SetValue(0, leanICDeg);
	}

	//------------------------------------------------------------------------
	// SetupUI
	//------------------------------------------------------------------------
	private void SetupUI()
	{
		VBoxContainer vbox = GetNode<VBoxContainer>("UINode/MgContainTL/VBox");

		// Set up data display
		datDisplay = vbox.GetNode<UIPanelDisplay>("DatDisplay");
		datDisplay.SetNDisplay(5);

		datDisplay.SetDigitsAfterDecimal(0,1);
		datDisplay.SetDigitsAfterDecimal(1,4);
		datDisplay.SetDigitsAfterDecimal(2,4);
		datDisplay.SetDigitsAfterDecimal(3,4);
		datDisplay.SetDigitsAfterDecimal(4,4);

		datDisplay.SetLabel(0,"Lean IC");
		datDisplay.SetLabel(1,"Kinetic");
		datDisplay.SetLabel(2,"Potential");
		datDisplay.SetLabel(3,"Total");
		datDisplay.SetLabel(4,"Ang.Mo.Vert");

		datDisplay.SetValue(0,leanICDeg);
		datDisplay.SetValue(1,0.0f);
		datDisplay.SetValue(2,0.0f);
		datDisplay.SetValue(3,0.0f);
		datDisplay.SetValue(4,0.0f);

		//--- Sim Button
		simButton = vbox.GetNode<Button>("SimButton");
		simButton.Pressed += OnSimButtonPressed;

		//--- Adjustment buttons
		adjButtons = new Button[4];
		adjButtons[0] = 
			GetNode<Button>("UINode/MgContainTL/VBox/HBoxAdjust/LLeftButton");
		adjButtons[1] = 
			GetNode<Button>("UINode/MgContainTL/VBox/HBoxAdjust/LeftButton");
		adjButtons[1].Pressed += OnAdjButtonSlow;
		adjButtons[2] = 
			GetNode<Button>("UINode/MgContainTL/VBox/HBoxAdjust/RightButton");
		adjButtons[2].Pressed += OnAdjButtonSlow;
		adjButtons[3] = 
			GetNode<Button>("UINode/MgContainTL/VBox/HBoxAdjust/RRightButton");
		
	}

	//------------------------------------------------------------------------
	// OnSimButtonPressed
	//------------------------------------------------------------------------
	private void OnSimButtonPressed()
	{
		//GD.Print("OnSimButtonPressed");
		if(opMode == OpMode.Configure){
			simButton.Text = "Stop Sim";
			opMode = OpMode.Simulate;
		}
		else{
			simButton.Text = "Simulate";
			opMode = OpMode.Configure;
		}
	}

	//------------------------------------------------------------------------
    // OnAdjButtonSlow
    //------------------------------------------------------------------------
    private void OnAdjButtonSlow()
    {
        if(adjButtons[1].ButtonPressed){
            leanICDeg += dAngle;
            ProcessLeanAngle();
        }
        else if(adjButtons[2].ButtonPressed){
            leanICDeg -= dAngle;
            ProcessLeanAngle();
        }
    }
}
