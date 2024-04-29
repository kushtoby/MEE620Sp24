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

    //------------------------------------------------------------------------
    // Constructor      [STUDENTS: DO NOT CHANGE THIS FUNCTION]
    //------------------------------------------------------------------------
    public WeeblePlainSim() : base(6)
    {

    }
}