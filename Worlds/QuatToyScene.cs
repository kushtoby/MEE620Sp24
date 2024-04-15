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

	DatDisplay2 daa;        // display for axis angle info
	Button[] buttonsAxisAngle;
	Button buttonNextRotation;
	Button buttonAbandonRotation;

	DatGrid qGrid;
	CheckBox cBoxShowAxis;
	Button buttonReset;
	


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
		int i,j;

		Texture2D leftArrowIcon = GD.Load<Texture2D>("res://Textures/ArrowLeft.svg");
		Texture2D rightArrowIcon = GD.Load<Texture2D>("res://Textures/ArrowRight.svg");
		Texture2D upArrowIcon = GD.Load<Texture2D>("res://Textures/ArrowUp.svg");
		Texture2D downArrowIcon = GD.Load<Texture2D>("res://Textures/ArrowDown.svg");

		MarginContainer margTL = GetNode<MarginContainer>("UINode/MargTL");
		vBoxA = new VBoxContainer();
		margTL.AddChild(vBoxA);

		MarginContainer margTR = GetNode<MarginContainer>("UINode/MargTR");
		vBoxQ = new VBoxContainer();
		margTR.AddChild(vBoxQ);

		daa = new DatDisplay2(vBoxA);
		daa.SetNDisplay(4,true);
		daa.SetTitle("New Rotation");
		daa.SetLabel(0,"Axis X:");
		daa.SetLabel(1,"Axis Y:");
		daa.SetLabel(2,"Axis Z:");
		daa.SetLabel(3,"Angle:");

		daa.SetDigitsAfterDecimal(0, 2);
		daa.SetDigitsAfterDecimal(1, 2);
		daa.SetDigitsAfterDecimal(2, 2);
		daa.SetDigitsAfterDecimal(3, 1);
		daa.SetSuffixDegree(3);

		daa.SetValue(0, 1.0f);
		daa.SetValue(1, 0.0f);
		daa.SetValue(2, 0.0f);
		daa.SetValue(3, 0.0f);

		vBoxA.AddChild(new HSeparator());

		Label lblAxAdj = new Label();
		lblAxAdj.Text = "Adjust Axis";
		vBoxA.AddChild(lblAxAdj);

		buttonsAxisAngle = new Button[8];
		for(i=0;i<8;++i){
			buttonsAxisAngle[i] = new Button();
		}
		buttonsAxisAngle[0].Icon = leftArrowIcon;
		buttonsAxisAngle[1].Icon = rightArrowIcon;
		buttonsAxisAngle[2].Icon = upArrowIcon;
		buttonsAxisAngle[3].Icon = downArrowIcon;
		buttonsAxisAngle[4].Text = "V";
		buttonsAxisAngle[5].Icon = leftArrowIcon;
		buttonsAxisAngle[6].Icon = rightArrowIcon;
		buttonsAxisAngle[7].Text = "0";

		GridContainer grid = new GridContainer();
		grid.Columns = 3;

		grid.AddChild(new Control());
		grid.AddChild(buttonsAxisAngle[2]);
		grid.AddChild(new Control());
		grid.AddChild(buttonsAxisAngle[0]);
		grid.AddChild(buttonsAxisAngle[4]);
		grid.AddChild(buttonsAxisAngle[1]);
		grid.AddChild(new Control());
		grid.AddChild(buttonsAxisAngle[3]);
		grid.AddChild(new Control());
		vBoxA.AddChild(grid);

		vBoxA.AddChild(new HSeparator());

		Label dumLbl2 = new Label();
		dumLbl2.Text = "Adjust Angle";
		vBoxA.AddChild(dumLbl2);

		HBoxContainer angHbox = new HBoxContainer();
		angHbox.AddChild(buttonsAxisAngle[5]);
		angHbox.AddChild(buttonsAxisAngle[7]);
		angHbox.AddChild(buttonsAxisAngle[6]);
		vBoxA.AddChild(angHbox);

		vBoxA.AddChild(new HSeparator());

		buttonNextRotation = new Button();
		buttonNextRotation.Text = "Next";
		vBoxA.AddChild(buttonNextRotation);

		buttonAbandonRotation = new Button();
		buttonAbandonRotation.Text = "Abandon";
		vBoxA.AddChild(buttonAbandonRotation);

		//-------- Quaternion UI ------------

		qGrid = new DatGrid(vBoxQ);
		qGrid.SetGridSize(3,4);
		qGrid.SetColLabel(0, "s");
		qGrid.SetColLabel(1, "x");
		qGrid.SetColLabel(2, "y");
		qGrid.SetColLabel(3, "z");
		qGrid.SetRowLabel(0, "Current");
		qGrid.SetRowLabel(1, "All Previous");
		qGrid.SetRowLabel(2, "Product");
		qGrid.SetCornerLabel("Quaternions");

		for(i=0;i<3;++i){
			qGrid.SetDigitsAfterDecimal(i,0,3);
			qGrid.SetValue(i,0, 1.0f);
			for(j=1;j<4;++j){
				qGrid.SetDigitsAfterDecimal(i,j,3);
				qGrid.SetValue(i,j, 0.0f);
			}
		}

		vBoxQ.AddChild(new HSeparator());

		//HBoxContainer hBoxAfterQuat = new HBoxContainer();
		cBoxShowAxis = new CheckBox();
		cBoxShowAxis.Text = "Show Axis";
		buttonReset = new Button();
		buttonReset.Text = "Reset";
		vBoxQ.AddChild(cBoxShowAxis);
		vBoxQ.AddChild(buttonReset);
		// hBoxAfterQuat.AddChild(cBoxShowAxis);
		// hBoxAfterQuat.AddChild(buttonReset);
		// vBoxQ.AddChild(hBoxAfterQuat);
	}


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }
}
