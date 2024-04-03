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

	OptionButton optionSimSubsteps;
	OptionButton optionSim;
	OptionButton optionSpinRate;

	double[] spinRateOptions;
	int spinRateIdx;

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

		spinRateOptions = new double[4];
		spinRateOptions[0] = 10.0;
		spinRateOptions[1] = 20.0;
		spinRateOptions[2] = 40.0;
		spinRateOptions[3] = 60.0;
		spinRateIdx = 3;

		// set up the model
		model = GetNode<TopDiskModel>("TopDiskModel");
		dumAngle = 0.0f;
		model.SetEulerAnglesYZY(0.0f, Mathf.DegToRad(leanICDeg), 0.0f);

		// Set up the simulation
		sim = new SpinTopSim();
		spinRate = spinRateOptions[spinRateIdx];
		sim.LeanAngle = Mathf.DegToRad(leanICDeg);
		sim.SpinRate = spinRate;

		nSimSteps = 16;
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
			
			model.SetEulerAnglesYZY((float)sim.PrecessionAngle,
				(float)sim.LeanAngle, (float)sim.SpinAngle);

			if(uiRefreshCtr > uiRefreshTHold){
				double ke = sim.KineticEnergy;
				double pe = sim.PotentialEnergy;
				double totErg = ke + pe;

				double angMoY = sim.AngMoY;

				datDisplay.SetValue(1,(float)ke);
				datDisplay.SetValue(2,(float)pe);
				datDisplay.SetValue(3,(float)totErg);
				datDisplay.SetValue(4,(float)angMoY);

				uiRefreshCtr = 0;
			}
			++uiRefreshCtr;

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

		sim.ResetIC((double)Mathf.DegToRad(leanICDeg), spinRate);
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

		uiRefreshCtr = 0;
		uiRefreshTHold = 3;

		//--- Option Button, Sim Substeps
		optionSimSubsteps = vbox.GetNode<OptionButton>("OptionSimSubSteps");
		optionSimSubsteps.AddItem("Substeps: 1",0);
		optionSimSubsteps.AddItem("Substeps: 2",1);
		optionSimSubsteps.AddItem("Substeps: 4",2);
		optionSimSubsteps.AddItem("Substeps: 8",3);
		optionSimSubsteps.AddItem("Substeps: 16",4);
		optionSimSubsteps.Selected = 4;
		optionSimSubsteps.ItemSelected += OnOptionSimSubsteps;

		//--- Option Button Spin Rate
		optionSpinRate = vbox.GetNode<OptionButton>("OptionSpinRate");
		optionSpinRate.AddItem("SpinRate: 10 rad/s", 0);
		optionSpinRate.AddItem("SpinRate: 20 rad/s", 1);
		optionSpinRate.AddItem("SpinRate: 40 rad/s", 2);
		optionSpinRate.AddItem("SpinRate: 60 rad/s", 3);
		optionSpinRate.Selected = 3;
		optionSpinRate.ItemSelected += OnOptionSpinRate;

		//--- Option Button, Sim choice
		optionSim = vbox.GetNode<OptionButton>("OptionSim");
		optionSim.AddItem("Sim: Fixed Body",0);
		optionSim.Selected = 0;

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
		int i;
		//GD.Print("OnSimButtonPressed");
		if(opMode == OpMode.Configure){
			simButton.Text = "Stop Sim";
			opMode = OpMode.Simulate;
			optionSim.Disabled = true;
			optionSimSubsteps.Disabled = true;
			optionSpinRate.Disabled = true;
			for(i=0;i<4;++i)
				adjButtons[i].Disabled = true;
		}
		else{
			simButton.Text = "Simulate";
			opMode = OpMode.Configure;
			optionSim.Disabled = false;
			optionSimSubsteps.Disabled = false;
			optionSpinRate.Disabled = false;
			for(i=0;i<4;++i)
				adjButtons[i].Disabled = false;
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

	//------------------------------------------------------------------------
    // OnOptionSimSubsteps
    //------------------------------------------------------------------------
    private void OnOptionSimSubsteps(long ii)
    {
		//GD.Print("Substeps: " + ii);
		if(ii == 0)
			nSimSteps = 1;
		if(ii == 1)
			nSimSteps = 2;
		if(ii == 2)
			nSimSteps = 4;
		if(ii == 3)
			nSimSteps = 8;
		if(ii == 4)
			nSimSteps = 16;
	}

	//------------------------------------------------------------------------
    // OnOptionSpinRate
    //------------------------------------------------------------------------
    private void OnOptionSpinRate(long ii)
    {
		spinRateIdx = (int)ii;
		spinRate = spinRateOptions[spinRateIdx];
		ProcessLeanAngle();

		//GD.Print("SpinRate: " + spinRate);
	}
}
