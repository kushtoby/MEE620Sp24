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
		
		opMode = OpMode.Configure;

		sim = new SpinTopSim();
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

		// Set up data display
		datDisplay = GetNode<UIPanelDisplay>(
			"UINode/MgContainTL/VBox/DatDisplay");
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

		datDisplay.SetValue(0,30.0f);
		datDisplay.SetValue(1,0.0f);
		datDisplay.SetValue(2,0.0f);
		datDisplay.SetValue(3,0.0f);
		datDisplay.SetValue(4,0.0f);
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		if(opMode == OpMode.Simulate){
			
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
}
