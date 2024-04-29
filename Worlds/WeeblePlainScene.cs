//============================================================================
// WeeblePlainScene.cs     Scene in which we simulate the unactuated weeble
//============================================================================
using Godot;
using System;

public partial class WeeblePlainScene : Node3D
{
	float leanDeg = 30.0f;
	float initSpinRate = 0.0f;

	enum OpMode
	{
		Configure,
		Simulate,
	}
	OpMode opMode;         // operation mode

	// Model
	WeeblePlain model;
	Vector3 modelPos;
	Quaternion modelQuat;

	// Simulation
	WeeblePlainSim sim;
	double time;
	int nSimSteps;         // number of sim steps per _PhysicsProcess
	float R;              // radius of weeble sphere

	double[] loc;          // location of center of weeble (obtained from sim)
	double[] quat;         // quaternion of weeble (obtained from sim)

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
		opMode = OpMode.Simulate;

		model = GetNode<WeeblePlain>("WeeblePlain");
		modelPos = new Vector3();
		modelQuat = new Quaternion();

		// Set up the simulation
		sim = new WeeblePlainSim();
		//sim.LeanAngle = Mathf.DegToRad(leanICDeg);
		//sim.SpinRate = spinRate;

		nSimSteps = 4;
		time = 0.0;

		R = 0.5f;
		loc = new double[2];
		quat = new double[4];

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
	// _Process: Called every frame. 'delta' is the elapsed time since the 
	//           previous frame.
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		if(opMode == OpMode.Simulate){
			double dangle = Math.Sin(time);
			quat[0] = Math.Cos(0.5*dangle);
			quat[1] = quat[2] = 0.0;
			quat[3] = Math.Sin(0.5*dangle);

			loc[0] = -R*dangle;
			loc[1] = 0.0;

			SetModelLoc();
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
	// SetModelLoc
	//------------------------------------------------------------------------
	private void SetModelLoc()
	{
		modelPos.X = (float)loc[0];
		modelPos.Y = R;
		modelPos.Z = (float)loc[1];

		modelQuat.W = (float)quat[0];
		modelQuat.X = (float)quat[1];
		modelQuat.Y = (float)quat[2];
		modelQuat.Z = (float)quat[3];

		model.Position = modelPos;
		model.Quaternion = modelQuat;
	}
}
