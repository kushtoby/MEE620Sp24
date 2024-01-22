//============================================================================
// DoublePendSim.cs   Class for creating a simulation of a double pendulum.
//============================================================================
using System;

public class DoublePendSim : Simulator
{
    // physical parameters
    double L1;   // Length of rod 1
    double L2;   // Length of rod 2
    double m1;   // mass of pendulum 1
    double m2;   // mass of pendulum 2
    

    //------------------------------------------------------------------------
    // Constructor
    //------------------------------------------------------------------------
    public DoublePendSim() : base(4)
    {
        L1 = 1.0;

        // Default initial conditions
        x[0] = 0.0f;    // initial angle (radians)
        x[1] = 0.0f;    // initial rate of change of angle (radians/sec)

        SetRHSFunc(RHSFuncDoublePend);
    }

    //------------------------------------------------------------------------
    // RHSFuncSimplePend:  Evaluates the right sides of the differential
    //                     equations for the simple pendulum
    //------------------------------------------------------------------------
    private void RHSFuncDoublePend(double[] xx, double t, double[] ff)
    {

    }
}