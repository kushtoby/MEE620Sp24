//============================================================================
// UJointPendSim.cs   Class for creating a simulation of the universal pendulum.
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
        double cos2Phi  = Math.Cos(2.0*phi);
        double sin2Phi  = Math.Sin(2.0*phi);

        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //

        double cf = 0.5*(cos2Phi + 1.0);


        ff[0] = thetad;   // time derivative of state theta
        ff[1] = phid;   // time derivative of state phi
        ff[2] = (sin2Phi*thetad*phid - (g*sinTheta*cosPhi/L))/cf;   // time derivative of state thetad
        ff[3] = (-0.5*sin2Phi*thetad*thetad - (g*sinPhi*cosTheta)/L);   // time derivative of state phid
    }

    //------------------------------------------------------------------------
    // Getters/Setters
    //------------------------------------------------------------------------

    // Pendulum angleX ----------------------------
    public double AngleX
    {
        set{
            x[0] = value;
        }

        get{
            return x[0];
        }
    }

    // Pendulum angleZ ----------------------------
    public double AngleZ
    {
        set{
            x[1] = value;
        }

        get{
            return x[1];
        }
    }

    // Pendulum angleXDot ----------------------------
    public double AngleXDot
    {
        set{
            x[2] = value;
        }

        get{
            return x[2];
        }
    }

    // Pendulum angleZDot ----------------------------
    public double AngleZDot
    {
        set{
            x[3] = value;
        }

        get{
            return x[3];
        }
    }

    // Kinetic energy ----------------------------
    public double KineticEnergy
    {
        get{
            double theta  = x[0];
            double phi    = x[1];
            double thetad = x[2];
            double phid   = x[3];

            //########## YOU NEED TO CALCULATE THIS ###########
            double cosTheta = Math.Cos(theta);
            double sinTheta = Math.Sin(theta);
            double cosPhi = Math.Cos(phi);
            double sinPhi = Math.Sin(phi);

            double vx = L*cosTheta*phid;
            double vy = L*(sinPhi*cosTheta*phid + sinTheta*cosPhi*thetad);
            double vz = L*(sinPhi*sinTheta*phid - cosTheta*cosPhi*thetad);
            double totalVel = vx + vy + vz;

            return 0.5* m * totalVel * totalVel; 
        }
    }

    // Potential energy ------------------------------
    public double PotentialEnergy
    {
         get{
            double theta  = x[0];
            double phi    = x[1];

            //########## YOU NEED TO CALCULATE THIS ###########
            return -m*g*L*Math.Cos(phi)*Math.Cos(theta); 
        }
    }

    // Angular momentum about pivot, vert coordinate ---------------
    public double AngMoY
    {
        get{
            double theta  = x[0];
            double phi    = x[1];
            double thetad = x[2];
            double phid   = x[3];

            //########## YOU NEED TO CALCULATE THIS ###########
            return 0.0; 
        }
    }
}