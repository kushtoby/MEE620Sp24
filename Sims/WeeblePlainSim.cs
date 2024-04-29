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
    double mExtra;   // other mass
    double d;        // location of center of mass below center of sphere
    double R;        // sphere radius

    double IGa;      // moment of inertia about center of mass symmetry axis
    double IGp;      // moment of inertia about cg, perpendicular axis

    // useful vectors
    VectorSpl omega;  // angular velocity of weeble in body frame
    VectorSpl vG;     // velocity of center of mass in body frame
    VectorSpl vPx;    // partial velocity in body frame
    VectorSpl vPy;
    VectorSpl vPz;
    VectorSpl Ny;     // N.y vector expressed in B frame
    VectorSpl Bx;     // B.x vector expressed in B frame;
    VectorSpl By;
    VectorSpl Bz;

    //------------------------------------------------------------------------
    // Constructor      [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public WeeblePlainSim() : base(9)
    {

        // initialize mass properties


        // Default initial conditions
        x[0] = 0.0;   // generalized coord: xC , coordinates of center
        x[1] = 0.0;   // generalized coord: zC 
        x[2] = 1.0;   // generalized coord: q0, quaternion coordinates
        x[3] = 0.0;   // generalized coord: q1
        x[4] = 0.0;   // generalized coord: q2
        x[5] = 0.0;   // generalized coord: q3
        x[6] = 1.0;   // generalized speed: omegaX
        x[7] = 0.0;   // generalized speed: omegaY
        x[8] = 0.0;   // generalized speed: omegaZ

        omega = new VectorSpl();
        vPx = new VectorSpl();
        vPy = new VectorSpl();
        vPz = new VectorSpl();
        Ny  = new VectorSpl();
        Bx  = new VectorSpl(1.0, 0.0, 0.0);
        By  = new VectorSpl(0.0, 1.0, 0.0);
        Bz  = new VectorSpl(0.0, 0.0, 1.0);
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

        omega.x = xx[6];  omega.y = xx[7]; omega.z = xx[8];
        Ny.x = 2.0*(q0*q3 + q1*q2);
        Ny.y = (q0*q0 - q1*q1 + q2*q2 - q3*q3);
        Ny.z = 2.0*(-q0*q1 + q2*q3);
        


        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        ff[0] = 0.0;
        ff[1] = 0.0;
        ff[2] = 0.0;
        ff[3] = 0.0;
        ff[4] = 0.0;
        ff[5] = 0.0;
        ff[6] = 0.0;
        ff[7] = 0.0;
        ff[8] = 0.0;
    }
}