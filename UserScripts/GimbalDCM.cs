//============================================================================
// GimbalDCM.cs   Users supply scripts that compute the direction cosine
//       matrices for selected Euler angle transformations
//============================================================================
using System;

public class GimbalDCM
{
    double [,] DCM;

    //------------------------------------------------------------------------
    // GimbalDCM  constructor
    //------------------------------------------------------------------------
    public GimbalDCM()
    {
        DCM = new double[3,3];
    }

    //------------------------------------------------------------------------
    // CalcDCM_YPR: Calculate direction cosine matrix (DCM) for Yaw Pitch Roll
    //      Euler angle transformation.
    //------------------------------------------------------------------------
    public void CalcDCM_YPR(double ang1, double ang2, double ang3)
    {
        // calculate useful trigonometric funcs
        double cos1 = Math.Cos(ang1);
        double sin1 = Math.Sin(ang1);
        double cos2 = Math.Cos(ang2);
        double sin2 = Math.Sin(ang2);
        double cos3 = Math.Cos(ang3);
        double sin3 = Math.Sin(ang3);
        //-----------------------------------------------------------------
        // students, write your expressions that go into the DCM below
        //        Array indices start at zero in C#.

        DCM[0,0] = 1.0; // first row
        DCM[0,1] = 0.0;
        DCM[0,2] = 0.0;

        DCM[1,0] = 0.0; // second row
        DCM[1,1] = 1.0;
        DCM[1,2] = 0.0;

        DCM[2,0] = 0.0; // third row
        DCM[2,1] = 0.0;
        DCM[2,2] = 1.0;
    }



    //------------------------------------------------------------------------
    // GetDCM:    STUDENTS DO NOT MODIFY THIS METHOD
    //------------------------------------------------------------------------
    public float GetDCM(int i, int j)
    {
        return (float)DCM[i,j];
    }
}