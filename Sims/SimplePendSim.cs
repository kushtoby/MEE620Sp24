//============================================================================
using System;

public class SimplePendSim : Simulator
{
    // physical parameters
    double L;   // mass of vehicle

    //------------------------------------------------------------------------
    // Constructor
    //------------------------------------------------------------------------
    public SimplePendSim() : base(2)
    {
        L = 1.0;

        // Default initial conditions
        x[0] = 0.0f;    // initial angle (radians)
        x[1] = 0.0f;    // initial rate of change of angle (radians/sec)

        SetRHSFunc(RHSFuncSimplePend);
    }

    //------------------------------------------------------------------------
    // RHSFuncSimplePend:  Evaluates the right sides of the differential
    //                     equations for the simple pendulum
    //------------------------------------------------------------------------
    private void RHSFuncSimplePend(double[] xx, double t, double[] ff)
    {
        double theta = xx[0];

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        ff[0] = 0.0;   // time derivative of state theta
        ff[1] = 0.0;   // time derivative of state thetaDot
    }
}