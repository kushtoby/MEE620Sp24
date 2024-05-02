//============================================================================
// WeeblePlainSim.cs   Class for creating a simulation of a "plain" weeble,
//      i.e. no spinning actuator disks.
//============================================================================
using System;
using Godot;

public class WeeblePlainSim : Simulator
{
    // initial condition exposed through export
    double leanDegIC = 0.0f;
    double omegaX_IC = 0.0f;
    double omegaY_IC = 0.0f;
    double omegaZ_IC = 0.0f;

    double mShell;   // mass of the shell
    double m;        // total mass
    double d;        // location of center of mass below center of sphere
    double R;        // sphere radius
    double thetaE;   // end theta for spherical shell

    double IGa;      // moment of inertia about center of mass symmetry axis
    double IGp;      // moment of inertia about cg, perpendicular axis

    double KE;       // kinetic energy
    double PE;       // potential energy

    // useful vectors
    VectorSpl omega;  // angular velocity of weeble in body frame
    VectorSpl rCrelP; // position of sphere center relative to contact point
    VectorSpl rGrelP; // position of center of mass relative to contact point
    VectorSpl vG;     // velocity of center of mass in body frame
    VectorSpl vPx;    // partial velocity in body frame
    VectorSpl vPy;
    VectorSpl vPz;
    VectorSpl Ny;     // N.y vector expressed in B frame
    VectorSpl NyDot;  // time deriv of N.y in the B frame, expressed in B
    VectorSpl Bx;     // B.x vector expressed in B frame;
    VectorSpl By;
    VectorSpl Bz;
    VectorSpl E;

    LinAlgEq sys;
    bool toDebug;
    bool toDisplay;

    //------------------------------------------------------------------------
    // Constructor      [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public WeeblePlainSim() : base(9)
    {

        // initialize mass properties
        R = 0.5;
        d = 0.2;
        mShell = 22.0;
        thetaE = 110.0 * 3.14159265/180.0;
        double sinThE = Math.Sin(thetaE);
        double cosThE = Math.Cos(thetaE);
        double cosThE3 = cosThE*cosThE*cosThE;

        double mx;
        double dShell = 0.5*R*sinThE*sinThE/(1.0 - cosThE);
        if(d >= dShell){
            mx = mShell*(d-dShell)/(R-d);
        } 
        else{
            mx = mShell*(dShell-d)/(R+d);
        }

        m = mShell + mx;
        IGa = m*R*R*(cosThE3/3.0 - cosThE + 2.0/3.0)/(1.0-cosThE);
        double ICp = 0.5*m*R*R*(-cosThE3/3.0 - cosThE + 4.0/3.0)/(1.0-cosThE);
        IGp = ICp - m*d*d;


        // Default initial conditions
        x[0] = 0.0;   // generalized coord: xC , coordinates of center
        x[1] = 0.0;   // generalized coord: zC 
        x[2] = 1.0;   // generalized coord: q0, quaternion coordinates
        x[3] = 0.0;   // generalized coord: q1
        x[4] = 0.0;   // generalized coord: q2
        x[5] = 0.0;   // generalized coord: q3
        x[6] = 3.0;   // generalized speed: omegaX
        x[7] = 4.0;   // generalized speed: omegaY
        x[8] = 0.0;   // generalized speed: omegaZ

        omega = new VectorSpl();
        vPx = new VectorSpl();
        vPy = new VectorSpl();
        vPz = new VectorSpl();
        Ny  = new VectorSpl();
        NyDot = new VectorSpl();
        Bx  = new VectorSpl(1.0, 0.0, 0.0);
        By  = new VectorSpl(0.0, 1.0, 0.0);
        Bz  = new VectorSpl(0.0, 0.0, 1.0);

        sys = new LinAlgEq(3);

        SetRHSFunc(RHSWeeblePlain);

        toDebug = false;   // SET TO TRUE IF YOU WANT TO DEBUG  **********
        if(toDebug){
            x[2] = 0.7;
            x[3] = 0.4;
            x[4] = 0.3;
            x[5] = Math.Sqrt(1.0-x[2]*x[2]-x[3]*x[3]-x[4]*x[4]);
            x[6] = 2.2;
            x[7] = -3.3;
            x[8] = -1.9;

            toDisplay = true;   // Don't change this line
        }
    }

    //------------------------------------------------------------------------
    // RHSWeeblePlain:  Evaluates the right sides of the differential
    //                  equations for the plain weeble
    //------------------------------------------------------------------------
    private void RHSWeeblePlain(double[] xx, double t, double[] ff)
    {
        double q0 = xx[2];
        double q1 = xx[3];
        double q2 = xx[4];
        double q3 = xx[5];

        // Define some vectors (and other quantities) that will help with the 
        // calculations
        //

        // some extra vectors if you want to use them
        VectorSpl vec1, vec2, vec3, vec4, vec5;

        // angular velocity vector
        omega.x = xx[6];  omega.y = xx[7]; omega.z = xx[8];

        // kinematic equations for quaternions
        double q0Dot = 0.5*(-q1*omega.x - q2*omega.y - q3*omega.z);
        double q1Dot = 0.5*(0.0); // replace the 0.0 with correct kinematic
        double q2Dot = 0.5*(0.0); //      equations for the quaternion
        double q3Dot = 0.5*(0.0);

        // N.y expressed in the B frame
        Ny.x = 2.0*(q0*q3 + q1*q2);             // bx component
        Ny.y = (q0*q0 - q1*q1 + q2*q2 - q3*q3); // by component
        Ny.z = 2.0*(-q0*q1 + q2*q3);            // bz component
        
        // time derivative of N.y in the B frame, expressed in B frame
        NyDot.x = 0.0; // replace the 0.0 with correct terms
        NyDot.y = 0.0;
        NyDot.z = 0.0;

        // position vector of sphere Center relative to contact point
        rCrelP = R*Ny;

        // position vector of center of mass relative to contact point
        rGrelP = rCrelP - (d*By);

        // partial velocities of center of mass
        vPx = VectorSpl.Cross(Bx, rGrelP);       // corresp to omega.x
        //vPy = put appropriate expression here  // corresp to omega.y
        //vPz = put appropriate expression here  // corresp to omega.z

        // velocity of center of mass
        // vG = put appropriate expression here

        // velocity of center of sphere (want this in N frame)
        // For this, I'd recommend just copying the expressions for vCx
        //      and vCz directly from the Python work
        double vCx = 0.0;   // replace the zeros
        double vCz = 0.0;

        // Construct E vector here. You might want to extra vectors
        //      v1, v2, etc

        // Construct system of three equations for the three unknowns: 
        //      omegaXdot, omegaYdot, omegaZdot
        double A00 = 1.0;  // replace the numbers in the coefficient matrix
        double A01 = 0.0;
        double A02 = 0.0;
        double A10 = 0.0;  
        double A11 = 1.0;
        double A12 = 0.0;
        double A20 = 0.0;  
        double A21 = 0.0;
        double A22 = 1.0;

        double B0 = 0.0;  // replace numbers in the RHS vector
        double B1 = 0.0;
        double B2 = 0.0;

        sys.A[0][0] = A00;  sys.A[0][1] = A01;  sys.A[0][2] = A02;
        sys.A[1][0] = A10;  sys.A[1][1] = A11;  sys.A[1][2] = A12;
        sys.A[2][0] = A20;  sys.A[2][1] = A21;  sys.A[2][2] = A22;
        
        sys.b[0] = B0;  sys.b[1] = B1; sys.b[2] = B2;
        sys.SolveGauss();

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        ff[0] = 0.0;
        ff[1] = 0.0;
        ff[2] = 0.0;
        ff[3] = 0.0;
        ff[4] = 0.0;
        ff[5] = 0.0;
        ff[6] = sys.sol[0]; // solutions to linear algebraic equations here
        ff[7] = sys.sol[1];
        ff[8] = sys.sol[2];

        // If you are debugging and want to print quantities to the Godot console
        // you can use the GD.Print command as shown in the example.
        if(toDisplay){   // Displays intermediate quantities on console window

            // Example
            // GD.Print("m = " + m);
            // GD.Print("q = " + q0 + ", " + q1 + ", " + q2 + ", " + q3);
            // GD.Print("omega = " + omega.x + ", " + omega.y + ", " + omega.z);
            // GD.Print("vPx = " + vPx.ToString());

            toDisplay = false;
        }

        // ##### Leave this alone ####
        if(toDebug){
            for(int i=0;i<9;++i){
                ff[i] = 0.0;
            }
        }
    }

    //------------------------------------------------------------------------
    // calcEnergy:  Calculates kinetic and potential energies
    //              ***** STUDENTS CALCULATE ENERGIES HERE *****
    //------------------------------------------------------------------------
    public void CalcEnergy()
    {
        double q0 = x[2];
        double q1 = x[3];
        double q2 = x[4];
        double q3 = x[5];

        // angular velocity vector
        omega.x = x[6];  omega.y = x[7];  omega.z = x[8];

        // N.y expressed in the B frame
        Ny.x = 2.0*(q0*q3 + q1*q2);             // bx component
        Ny.y = (q0*q0 - q1*q1 + q2*q2 - q3*q3); // by component
        Ny.z = 2.0*(-q0*q1 + q2*q3);            // bz component

        VectorSpl v1, v2, v3;    // some vectors if you want to use them

        rCrelP = R*Ny;     // position of sphere center rel to contact point
        rGrelP = rCrelP - (d*By);  // position of CG rel to contact point
        
        // do some calculations

        KE = 0.0;   // put kinetic energy here
        PE = 0.0;   // put potential energy here
    }

    //------------------------------------------------------------------------
    // GetLocOrient: Gets the location and orientation of the of the weeble
    // ***** STUDENTS DO NOT MODIFY THIS METHOD *******************
    //------------------------------------------------------------------------
    public void GetLocOrient(double[] loc, double[] quat)
    {
        loc[0] = x[0];
        loc[1] = x[1];

        quat[0] = x[2];
        quat[1] = x[3];
        quat[2] = x[4];
        quat[3] = x[5];
    }

    //------------------------------------------------------------------------
    // Getters/Setters
    //------------------------------------------------------------------------

    // Kinetic Energy   ****** Students do not modify
    public double KineticEnergy
    {
        get{
            return KE;
        }
    }

    // Potential Energy   ****** Students do not modify
    public double PotentialEnergy
    {
        get{
            return PE;
        }
    }
}