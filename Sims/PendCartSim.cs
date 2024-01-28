//============================================================================
// PendCartSim.cs   Class for creating a simulation of a the cart/pendulum.
//============================================================================
using System;

public class PendCartSim : Simulator
{
    // physical parameters
    double L;    // Length of rod
    double mc;   // mass of cart
    double mp;   // mass of pendulum


    //------------------------------------------------------------------------
    // Constructor      [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public PendCartSim() : base(4)
    {
        L = 0.9;
        mc = 1.4;
        mp = 1.1;

        // Default initial conditions
        x[0] = 0.0;    // shoulder angle
        x[1] = 0.0;    // elbow angle
        x[2] = 0.0;    // generalized speed 1
        x[3] = 0.0;    // generalized speed 2

        SetRHSFunc(RHSFuncPendCart);
    }

    //------------------------------------------------------------------------
    // RHSFuncPendCart:  Evaluates the right sides of the differential
    //                   equations for the pendulum/cart
    //------------------------------------------------------------------------
    private void RHSFuncPendCart(double[] xx, double t, double[] ff)
    {
        double theta1 = xx[0];
        double theta2 = xx[1];
        double u1 = xx[2];
        double u2 = xx[3];

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        ff[0] = 0.0;   // time derivative of state theta1
        ff[1] = 0.0;   // time derivative of state theta2
        ff[2] = 0.0;   // time derivative of state u1
        ff[3] = 0.0;   // time derivative of state u2
    }

}