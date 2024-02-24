//============================================================================
// SpinTopSim.cs   Class for creating a simulation of a spinning top.
//============================================================================
using System;

public class SpinTopSim : Simulator
{
    // physical parameters
    double h;    // distance of CG from contact rotation point
    double m;   // mass of top
    double IGa;  // moment of inertia about its spin axis
    double IGp;  // moment of inertia about its CG, perpendicular to spin axis


    //------------------------------------------------------------------------
    // Constructor      [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public SpinTopSim() : base(6)
    {
        m = 4.0;
        h = 0.8;
        double r = 0.5;
        IGa = 0.5*m*r*r;
        IGp = 0.5*IGa;

        // Default initial conditions
        x[0] = 0.0;    // generalized coord: precession angle psi
        x[1] = 0.0;    // generalized coord: lean angle phi
        x[2] = 0.0;    // generalized coord: spin angle theta
        x[3] = 0.0;    // generalized speed: omegaX
        x[4] = 0.0;    // generalized speed: omegaY (spin rate)
        x[5] = 0.0;    // generalized speed: omegaZ

        SetRHSFunc(RHSFuncSpinTopBody);
    }

    //------------------------------------------------------------------------
    // RHSFuncSpinTopBody:  Evaluates the right sides of the differential
    //                 equations for the spinning top (body frame)
    //------------------------------------------------------------------------
    private void RHSFuncSpinTopBody(double[] xx, double t, double[] ff)
    {
        double psi = xx[0];
        double phi = xx[1];
        double theta = xx[2];
        double omegaX = xx[3];
        double omegaY = xx[4];
        double omegaZ = xx[5];
    }
}