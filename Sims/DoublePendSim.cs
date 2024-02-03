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
    // Constructor      [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public DoublePendSim() : base(4)
    {
        L1 = 0.9;
        L2 = 0.7;
        m1 = 1.4;
        m2 = 1.1;

        // Default initial conditions
        x[0] = 0.0;    // shoulder angle
        x[1] = 0.0;    // elbow angle
        x[2] = 0.0;    // generalized speed 1
        x[3] = 0.0;    // generalized speed 2

        SetRHSFunc(RHSFuncDoublePend);
    }

    //------------------------------------------------------------------------
    // RHSFuncDoublePend:  Evaluates the right sides of the differential
    //                     equations for the double pendulum
    //------------------------------------------------------------------------
    private void RHSFuncDoublePend(double[] xx, double t, double[] ff)
    {
        double theta1 = xx[0];
        double theta2 = xx[1];
        double u1 = xx[2];
        double u2 = xx[3];

        
        // Evaluate right sides of differential equations of motion
        // ##### You will need to provide these ###### //
        double sinTh1 = Math.Sin(theta1);
        double cosTh2 = Math.Cos(theta2);
        double sinTh2 = Math.Sin(theta2);
        double sinSum = Math.Sin(theta1 + theta2);

        double A = (m1 + m2) * L1 * L1;
        double B = m2 * L1 * L2 * cosTh2;
        double C = B;
        double D = m2 * L2 * L2;

        double det = A*D - B*C;

        double R1 = -m1*g*L1*sinTh1 - m2*g*L1*sinTh1 + m2*L1*L2*u2*u2*sinTh2;
        double R2 = -m2*g*L2*sinSum - m2*L1*L2*u1*u1*sinTh2;

        ff[0] = u1;   // time derivative of state theta1
        ff[1] = u2-u1;   // time derivative of state theta2
        ff[2] = (1/det) * ((D * R1)- (B*R2));   // time derivative of state u1
        ff[3] = (1/det) * ((-C *R1)+ (A * R2));   // time derivative of state u2
    }

    //------------------------------------------------------------------------
    // Getters and Setters
    //------------------------------------------------------------------------

    // Pendulum length ---------------------------
    public double Length1
    {
        set{
            if(value > 0.05){
                L1 = value;
            }
        }

        get{
            return L1;
        }
    }

    // Pendulum length ---------------------------
    public double Length2
    {
        set{
            if(value > 0.05){
                L2 = value;
            }
        }

        get{
            return L2;
        }
    }

    // Pendulum mass ---------------------------
    public double Mass1
    {
        set{
            if(value > 0.05){
                m1 = value;
            }
        }

        get{
            return m1;
        }
    }

    // Pendulum mass ---------------------------
    public double Mass2
    {
        set{
            if(value > 0.05){
                m2 = value;
            }
        }

        get{
            return m2;
        }
    }

    // Pendulum angle ----------------------------
    public double Angle1
    {
        set{
            x[0] = value;
        }

        get{
            return x[0];
        }
    }

    // Pendulum angle ----------------------------
    public double Angle2
    {
        set{
            x[1] = value;
        }

        get{
            return x[1];
        }
    }

    // Generalized Speed ---------
    public double GenSpeed1
    {
        set{
            x[2] = value;
        }

        get{
            return x[2];
        }
    }

    // Generalized Speed ---------
    public double GenSpeed2
    {
        set{
            x[3] = value;
        }

        get{
            return x[3];
        }
    }

    // Kinetic energy ----------
    public double KineticEnergy
    {
        get{
            double theta2 = x[1];
            double u1 = x[2];
            double u2 = x[3];

            //########## YOU NEED TO CALCULATE THIS ###########
            double v1Sq = L1*L1*u1*u1;
            double v2Sq = v1Sq + L2*L2*u2*u2 + 2.0*L1*L2*u1*u2*Math.Cos(theta2);
            
            return 0.5 * (m1*v1Sq + m2*v2Sq); 
        }
    }

    // Potential energy
    public double PotentialEnergy
    {
         get{
            double theta1 = x[0];
            double theta2 = x[1];

            //######1#### YOU NEED TO CALCULATE THIS ###########
            double pe = -1* ((m1 * g * L1 * Math.Cos(theta1)) + (m2 *g *(L1*Math.Cos(theta1) + L2*Math.Cos(theta1+theta2))));
            return pe; 
        }
    }
}