//============================================================================
// PendCartSim.cs   Class for creating a simulation of a the cart/pendulum.
//============================================================================
using System;

public class UJointPendSim : Simulator
{
    // physical parameters
    double L;    // Length of rod
    double m;   // mass of pendulum


    //------------------------------------------------------------------------
    // Constructor      [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public UJointPendSim() : base(4)
    {
        L = 1.1;
        m = 1.9;

        // Default initial conditions
        x[0] = 0.0;    // generalized coord: angle theta
        x[1] = 0.0;    // generalized coord: angle phi
        x[2] = 0.0;    // generalized speed: derivative of theta
        x[3] = 0.0;    // generalized speed: derivative of phi

        SetRHSFunc(RHSFuncUJPend);
    }

    //------------------------------------------------------------------------
    // RHSFuncUJPend:  Evaluates the right sides of the differential
    //                 equations for the universal joint pendulum
    //------------------------------------------------------------------------
    private void RHSFuncUJPend(double[] xx, double t, double[] ff)
    {
        double theta  = xx[0];
        double phi    = xx[1];
        double thetad = xx[2];
        double phid   = xx[3];

        double cosTheta = Math.Cos(theta);
        double sinTheta = Math.Sin(theta);
        double cosPhi   = Math.Cos(phi);
        double sinPhi   = Math.Sin(phi);

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        ff[0] = 0.0;   // time derivative of state theta
        ff[1] = 0.0;   // time derivative of state phi
        ff[2] = 0.0;   // time derivative of state thetad
        ff[3] = 0.0;   // time derivative of state phid
    }
}