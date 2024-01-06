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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("Hello 620");

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 20.0f;
		camDist = 3.0f;

		camTg = new Vector3(0.0f, 1.0f, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.Target = camTg;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
