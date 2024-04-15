//============================================================================
// QuatToyScene.cs     Scene for exploring quaternion rotations
//============================================================================
using Godot;
using System;
using System.ComponentModel.Design;

public partial class QuatToyScene : Node3D
{
	
	// Model
	WeeblePlain model;

	Node3D newAxis;
	Node3D totalAxis;

	// Camera Stuff
	CamRig cam;
	float longitudeDeg;
	float latitudeDeg;
	float camDist;
	float camFOV;
	Vector3 camTg;       // coords of camera target

	// rotational variables
	float longitDeg;
	float latitDeg;
	float rotDeg;
	float dAngle;
	float nx;          // normalized axis vector, x component
	float ny;	       //                         y component
	float nz;          //                         z component
	Vector3 newAxisRotation;
	Vector3 totalAxisRotation;
	Quaternion quatNew;
	Quaternion quatPrev;
	Quaternion quatProduct;
	Vector3 dvec;   // used for performing calcs

	
	// operational stuff
	bool axisAdjustMode;

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
		model = GetNode<WeeblePlain>("WeeblePlain");
		newAxis = GetNode<Node3D>("NewAxis");
		totalAxis = GetNode<Node3D>("TotalAxis");

		// Set up the camera rig
		longitudeDeg = 30.0f;
		latitudeDeg = 30.0f;
		camDist = 5.0f;
		camFOV = 35.0f;

		camTg = new Vector3(0.0f, 1.5f, 0.0f);
		cam = GetNode<CamRig>("CamRig");
		cam.LongitudeDeg = longitudeDeg;
		cam.LatitudeDeg = latitudeDeg;
		cam.Distance = camDist;
		cam.FOVDeg = camFOV;
		cam.Target = camTg;

		dAngle = 1.0f;
		newAxisRotation = new Vector3();
		totalAxisRotation = new Vector3();
		axisAdjustMode = true;
		quatNew = new Quaternion();
		quatPrev = new Quaternion();
		quatProduct = new Quaternion();
		dvec = new Vector3();

		SetupUI();

		Reset();
	}

	//------------------------------------------------------------------------
	// _Process: Called every frame
	//------------------------------------------------------------------------
	public override void _Process(double delta)
	{
		//--------- Axis Buttons ----------
		if(buttonsAxisAngle[0].ButtonPressed){  // left button pressed
			longitDeg -= dAngle;
			CalcAxisAngles();
		}

		if(buttonsAxisAngle[1].ButtonPressed){  // right button pressed
			longitDeg += dAngle;
			CalcAxisAngles();
		}

		if(buttonsAxisAngle[2].ButtonPressed){  // up button pressed
			latitDeg += dAngle;
			if(latitDeg > 90.0f)
				latitDeg = 90.0f;
			CalcAxisAngles();
		}

		if(buttonsAxisAngle[3].ButtonPressed){  // down button pressed
			latitDeg -= dAngle;
			if(latitDeg < -90.0f)
				latitDeg = -90.0f;
			CalcAxisAngles();
		}

		// if(buttonsAxisAngle[4].ButtonPressed){  //
		// 	GD.Print("Not Working Yet");
		// }

		//------- Angle Buttons ---------
		if(buttonsAxisAngle[5].ButtonPressed){  // left angle
			rotDeg -= dAngle;
			if(axisAdjustMode)
				DeactivateAxis();
			CalcRotation();
		}

		if(buttonsAxisAngle[6].ButtonPressed){  // right angle
			rotDeg += dAngle;
			if(axisAdjustMode)
				DeactivateAxis();
			CalcRotation();
		}

		if(buttonsAxisAngle[7].ButtonPressed){  // zero angle
			rotDeg = 0.0f;
			if(axisAdjustMode)
				DeactivateAxis();
			CalcRotation();
		}
	}

	//------------------------------------------------------------------------
	// Reset: resets all variables 
	//------------------------------------------------------------------------
	private void Reset()
	{
		longitDeg = 0.0f;
		latitDeg = 0.0f;
		rotDeg = 0.0f;

		newAxisRotation = Vector3.Zero;
		totalAxisRotation = Vector3.Zero;
		quatNew = Quaternion.Identity;
		quatPrev = Quaternion.Identity;
		quatProduct = Quaternion.Identity;
		ActivateAxis();

		CalcAxisAngles();
		CalcRotation();

		qGrid.SetValue(1,0, quatPrev.W);
		qGrid.SetValue(1,1, quatPrev.X);
		qGrid.SetValue(1,2, quatPrev.Y);
		qGrid.SetValue(1,3, quatPrev.Z);

		totalAxis.Rotation = totalAxisRotation;
	}

	//------------------------------------------------------------------------
	// CalcRotation
	//------------------------------------------------------------------------
	private void CalcRotation()
	{
		
		daa.SetValue(3, rotDeg);

		float rotRad = Mathf.DegToRad(rotDeg);
		float cosTerm = Mathf.Cos(0.5f * rotRad);
		float sinTerm = Mathf.Sin(0.5f * rotRad);

		// calculate new quaternion based on axis and angle
		quatNew.W = cosTerm;
		quatNew.X = sinTerm * nx;
		quatNew.Y = sinTerm * ny;
		quatNew.Z = sinTerm * nz;

		// update the product (and normalize)
		quatProduct = quatNew * quatPrev;
		quatProduct =quatProduct.Normalized();

		// calculate axis vector for quatProduct
		dvec.X = quatProduct.X;
		dvec.Y = quatProduct.Y;
		dvec.Z = quatProduct.Z;
		float mag = dvec.Length();
		if(mag > 0.0001f){
			dvec = dvec/mag;
			float phi = Mathf.Asin(dvec.Y);
			float psi;
			if(Mathf.Abs(dvec.Y) > 0.9999f){
				psi = 0.0f;
			}
			else{
				psi = Mathf.Atan2(-dvec.Z, dvec.X);
			}

			totalAxisRotation.Y = psi;
			totalAxisRotation.Z = phi;
			totalAxis.Rotation = totalAxisRotation;
		}

		qGrid.SetValue(0,0, quatNew.W);
		qGrid.SetValue(0,1, quatNew.X);
		qGrid.SetValue(0,2, quatNew.Y);
		qGrid.SetValue(0,3, quatNew.Z);

		qGrid.SetValue(2,0, quatProduct.W);
		qGrid.SetValue(2,1, quatProduct.X);
		qGrid.SetValue(2,2, quatProduct.Y);
		qGrid.SetValue(2,3, quatProduct.Z);

		model.Quaternion = quatProduct;
	}

	//------------------------------------------------------------------------
	// CalcAxisAngles:
	//------------------------------------------------------------------------
	private void CalcAxisAngles()
	{
		float longitRad = Mathf.DegToRad(longitDeg);
		float latitRad = Mathf.DegToRad(latitDeg);

		ny = Mathf.Sin(latitRad);
		nx = Mathf.Cos(latitRad) * Mathf.Cos(longitRad);
		nz = -Mathf.Cos(latitRad) * Mathf.Sin(longitRad); 

		daa.SetValue(0, nx);
		daa.SetValue(1, ny);
		daa.SetValue(2, nz);

		newAxisRotation.Y = longitRad;
		newAxisRotation.Z = latitRad;
		newAxis.Rotation = newAxisRotation;
	}

	//------------------------------------------------------------------------
	// Deactivate Axis
	//------------------------------------------------------------------------
	private void DeactivateAxis()
	{
		int i;
		for(i=0;i<5;++i){
			buttonsAxisAngle[i].Disabled = true;
		}
		axisAdjustMode = false;
	}

	//------------------------------------------------------------------------
	// Activate Axis
	//------------------------------------------------------------------------
	private void ActivateAxis()
	{
		int i;
		for(i=0;i<5;++i){
			buttonsAxisAngle[i].Disabled = false;
		}
		axisAdjustMode = true;
	}

	//------------------------------------------------------------------------
	// SetupUI
	//------------------------------------------------------------------------
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

		daa.SetDigitsAfterDecimal(0, 3);
		daa.SetDigitsAfterDecimal(1, 3);
		daa.SetDigitsAfterDecimal(2, 3);
		daa.SetDigitsAfterDecimal(3, 1);
		daa.SetSuffixDegree(3);

		daa.SetValue(0, 0.0f);
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

		buttonsAxisAngle[4].Pressed += OnAlignVectors;

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
		dumLbl2.Text = "Rotate Angle";
		vBoxA.AddChild(dumLbl2);

		HBoxContainer angHbox = new HBoxContainer();
		angHbox.AddChild(buttonsAxisAngle[5]);
		angHbox.AddChild(buttonsAxisAngle[7]);
		angHbox.AddChild(buttonsAxisAngle[6]);
		vBoxA.AddChild(angHbox);

		vBoxA.AddChild(new HSeparator());

		buttonNextRotation = new Button();
		buttonNextRotation.Text = "Next";
		buttonNextRotation.Pressed += OnButtonNextRot;
		vBoxA.AddChild(buttonNextRotation);

		buttonAbandonRotation = new Button();
		buttonAbandonRotation.Text = "Abandon";
		buttonAbandonRotation.Pressed += OnButtonAbandonRot;
		vBoxA.AddChild(buttonAbandonRotation);

		//-------- Quaternion UI ------------

		qGrid = new DatGrid(vBoxQ);
		qGrid.SetGridSize(3,4);
		qGrid.SetColLabel(0, "s");
		qGrid.SetColLabel(1, "x");
		qGrid.SetColLabel(2, "y");
		qGrid.SetColLabel(3, "z");
		qGrid.SetRowLabel(0, "New Rotation");
		qGrid.SetRowLabel(1, "All Previous");
		qGrid.SetRowLabel(2, "Product");
		qGrid.SetCornerLabel("Quaternions");

		for(i=0;i<3;++i){
			qGrid.SetDigitsAfterDecimal(i,0,4);
			qGrid.SetValue(i,0, 1.0f);
			for(j=1;j<4;++j){
				qGrid.SetDigitsAfterDecimal(i,j,4);
				qGrid.SetValue(i,j, 0.0f);
			}
		}

		vBoxQ.AddChild(new HSeparator());

		//HBoxContainer hBoxAfterQuat = new HBoxContainer();
		cBoxShowAxis = new CheckBox();
		cBoxShowAxis.Text = "Show Product Quaternion Axis";
		cBoxShowAxis.Pressed += OnCBoxShowAxis;
		buttonReset = new Button();
		buttonReset.Text = "Reset";
		buttonReset.Pressed += OnButtonReset;
		vBoxQ.AddChild(cBoxShowAxis);
		vBoxQ.AddChild(buttonReset);
		// hBoxAfterQuat.AddChild(cBoxShowAxis);
		// hBoxAfterQuat.AddChild(buttonReset);
		// vBoxQ.AddChild(hBoxAfterQuat);
	}

	//------------------------------------------------------------------------
	// OnAlignVectors
	//------------------------------------------------------------------------
	private void OnAlignVectors()
	{
		GD.Print("OnAlignVector");

		dvec.X = quatProduct.X;
		dvec.Y = quatProduct.Y;
		dvec.Z = quatProduct.Z;
		float mag = dvec.Length();
		if(mag > 0.0001f){
			dvec = dvec/mag;
			float phi = Mathf.Asin(dvec.Y);
			float psi;
			if(Mathf.Abs(dvec.Y) > 0.9999f){
				psi = 0.0f;
			}
			else{
				psi = Mathf.Atan2(-dvec.Z, dvec.X);
			}

			longitDeg = Mathf.RadToDeg(psi);
			latitDeg = Mathf.RadToDeg(phi);
			CalcAxisAngles();

			// totalAxisRotation.Y = psi;
			// totalAxisRotation.Z = phi;
			// totalAxis.Rotation = totalAxisRotation;
		}
	}

	//------------------------------------------------------------------------
	// OnButtonNextRot
	//------------------------------------------------------------------------
	private void OnButtonNextRot()
	{
		//GD.Print("ButtonNextRot");
		quatPrev = quatProduct;
		quatNew  = Quaternion.Identity;
		rotDeg = 0.0f;

		qGrid.SetValue(1,0, quatPrev.W);
		qGrid.SetValue(1,1, quatPrev.X);
		qGrid.SetValue(1,2, quatPrev.Y);
		qGrid.SetValue(1,3, quatPrev.Z);

		CalcRotation();

		ActivateAxis();
	}

	//------------------------------------------------------------------------
	// OnButtonAbandonRot
	//------------------------------------------------------------------------
	private void OnButtonAbandonRot()
	{
		//GD.Print("ButtonAbandonRot");

		quatNew = Quaternion.Identity;
		rotDeg = 0.0f;

		CalcRotation();

		ActivateAxis();
	}

	//------------------------------------------------------------------------
	// OnCBoxShowAxis
	//------------------------------------------------------------------------
	private void OnCBoxShowAxis()
	{

		if(cBoxShowAxis.ButtonPressed){
			totalAxis.Show();
		}
		else{
			totalAxis.Hide();
		}
	}

	//------------------------------------------------------------------------
	// OnButtonReset:
	//------------------------------------------------------------------------
	private void OnButtonReset()
	{
		//GD.Print("Reset Button Pressed");
		Reset();
	}
	
}
