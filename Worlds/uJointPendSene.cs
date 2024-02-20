//============================================================================
// UJointPendScene.cs
//============================================================================
using Godot;
using System;

public partial class uJointPendSene : Node3D
{

	[Export] float thetaDot = 0.0f;
	[Export] float phiDot = 0.0f;

	Node3D ax1Node;     // node that handles axis1
	Node3D ax2Node;     // node that handles axis2
	Vector3 ax1Angle;
	Vector3 ax2Angle;

	string[] angNames;
	float[] angles;
	float[] maxAngles;
	int actvIdx;
	float dTheta;
	bool manChanged;

	enum OpMode
	{
		Configure,
		Simulate,
	}
	OpMode opMode;         // operation mode

	UJointPendSim sim;
	double time;

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

	// Instructions label	
	Label instructLabel;
	String instStr;

	//------------------------------------------------------------------------
	// _Ready: Called once when the node enters the scene tree for the first 
	//         time.
	//------------------------------------------------------------------------
	public override void _Ready()
	{

		ax1Node = GetNode<Node3D>("ModelNode/ConnectorNode");
		ax2Node = GetNode<Node3D>("ModelNode/ConnectorNode/PendNode");
		ax1Angle = new Vector3();
		ax2Angle = new Vector3();

		opMode = OpMode.Configure;
		angNames = new string[2];
		angNames[0] = "theta";  angNames[1] = "phi";
		angles = new float[2];
		angles[0] = 0.0f;  angles[1] = 0.0f;
		maxAngles = new float[2];
		maxAngles[0] = 70.0f;   maxAngles[1] = 70.0f;
		actvIdx = 0;
		dTheta = 1.0f;
		manChanged = true;

		sim = new UJointPendSim();
		time = 0.0;

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
		datDisplay.SetLabel(1, angNames[0] + " >>");
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

		datDisplay.SetYellow(1);

		// instruction label
		instructLabel = GetNode<Label>(
			"UINode/MarginContainerBL/InstructLabel");
		instStr = "Press <TAB> to switch angles; " +
			"arrow keys to increase/decrease angle " + 
			"(up/down for incremental control); or 0 to "+
			"zero the angle. Press <Enter> to toggle simulation.";
		instructLabel.Text = instStr;
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		if(opMode == OpMode.Simulate){

			ax1Angle.X = (float)sim.AngleX;
			ax2Angle.Z = (float)sim.AngleZ;
			ax1Node.Rotation = ax1Angle;
			ax2Node.Rotation = ax2Angle;

			float ke = (float)sim.KineticEnergy;
			float pe = (float)sim.PotentialEnergy;
			float hy = (float)sim.AngMoY;

			datDisplay.SetValue(1, Mathf.RadToDeg(ax1Angle.X));
			datDisplay.SetValue(2, Mathf.RadToDeg(ax2Angle.Z));
			datDisplay.SetValue(3, ke);
			datDisplay.SetValue(4, pe);
			datDisplay.SetValue(5, ke+pe);
			datDisplay.SetValue(6, hy);

			if(Input.IsActionJustPressed("ui_accept")){
				opMode = OpMode.Configure;
				datDisplay.SetValue(0, opMode.ToString());
				actvIdx = 0;
				datDisplay.SetYellow(1);
			}
			return;
		}

		bool angleChanged = false;

		if(Input.IsActionPressed("ui_right")){
			angles[actvIdx] += dTheta;
			angleChanged = true;
		}

		if(Input.IsActionPressed("ui_left")){
			angles[actvIdx] -= dTheta;
			angleChanged = true;
		}

		if(Input.IsActionJustPressed("ui_up")){
			angles[actvIdx] += dTheta;
			angleChanged = true;
		}

		if(Input.IsActionJustPressed("ui_down")){
			angles[actvIdx] -= dTheta;
			angleChanged = true;
		}

		if(Input.IsActionJustPressed("ui_zero")){
			angles[actvIdx]  = 0.0f;
			angleChanged = true;
		}

		if(angleChanged){
			if(angles[actvIdx] > maxAngles[actvIdx])
				angles[actvIdx] = maxAngles[actvIdx];
			if(angles[actvIdx] < -maxAngles[actvIdx])
				angles[actvIdx] = -maxAngles[actvIdx];
			
			datDisplay.SetValue(actvIdx+1, angles[actvIdx]);
			ax1Angle.X = Mathf.DegToRad(angles[0]);
			ax2Angle.Z = Mathf.DegToRad(angles[1]);
			ax1Node.Rotation = ax1Angle;
			ax2Node.Rotation = ax2Angle;
			manChanged = true;
		}

		if(Input.IsActionJustPressed("ui_focus_next")){
			datDisplay.SetLabel(actvIdx+1, angNames[actvIdx]);
			datDisplay.SetWhite(actvIdx+1);
			++actvIdx;
			if(actvIdx >1)
				actvIdx = 0;
			datDisplay.SetLabel(actvIdx+1, angNames[actvIdx]+" >>");
			datDisplay.SetYellow(actvIdx+1);
		}

		if(Input.IsActionJustPressed("ui_accept")){
			if(manChanged){
				sim.AngleX = (double)Mathf.DegToRad(angles[0]);
				sim.AngleZ = (double)Mathf.DegToRad(angles[1]);
				sim.AngleXDot = (double)thetaDot;
				sim.AngleZDot = (double)phiDot;
			}
			datDisplay.SetWhite(1);
			datDisplay.SetWhite(2);
			opMode = OpMode.Simulate;
			datDisplay.SetValue(0, opMode.ToString());
			manChanged = false;
		}

	} // end _Process

	//------------------------------------------------------------------------
	// _PhysicsProcess:
	//------------------------------------------------------------------------
    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

		if(opMode != OpMode.Simulate)
			return;

		double deltaByTwo = 0.5*delta;
		sim.Step(time, deltaByTwo);
		time += deltaByTwo;
		sim.Step(time, deltaByTwo);
		time += deltaByTwo;
    }
}
