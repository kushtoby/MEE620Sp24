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

    //------------------------------------------------------------------------
    // Getters and Setters
    //------------------------------------------------------------------------

    // Pendulum length ---------------------------
    public double Length
    {
        set{
            if(value > 0.05){
                L = value;
            }
        }

        get{
            return L;
        }
    }

    // Pendulum angle ----------------------------
    public double Angle
    {
        set{
            x[0] = value;
        }

        get{
            return x[0];
        }
    }

    // Time derivative of pendulum angle ---------
    public double AngleDot
    {
        set{
            x[1] = value;
        }

        get{
            return x[1];
        }
    }

    // Kinetic energy ----------
    public double KineticEnergy
    {
        get{
            double thetaDot = x[1];

            //########## YOU NEED TO CALCULATE THIS ###########
            return 0.0; 
        }
    }

    // Potential energy
    public double PotentialEnergy
    {
         get{
            double theta = x[0];

            //########## YOU NEED TO CALCULATE THIS ###########
            return 0.0; 
        }
    }
}