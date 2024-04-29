//============================================================================
// VectorSpl.cs     A simple struct for defining a 3D vector and vector
//                  operations    (Generated from ChatGPT)
//============================================================================

using System;

struct VectorSpl
{
    public double x, y, z;

    // Constructor
    public VectorSpl(double x, double y, double z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public VectorSpl()
    {
        this.x = 0.0;
        this.y = 0.0;
        this.z = 0.0;
    }

    // Addition
    public static VectorSpl operator +(VectorSpl a, VectorSpl b)
    {
        return new VectorSpl(a.x + b.x, a.y + b.y, a.z + b.z);
    }

    // Subtraction
    public static VectorSpl operator -(VectorSpl a, VectorSpl b)
    {
        return new VectorSpl(a.x - b.x, a.y - b.y, a.z - b.z);
    }

    // Scalar multiplication
    public static VectorSpl operator *(double scalar, VectorSpl vector)
    {
        return new VectorSpl(scalar * vector.x, scalar * vector.y, scalar * vector.z);
    }

    // Scalar multiplication (reverse order)
    public static VectorSpl operator *(VectorSpl vector, double scalar)
    {
        return scalar * vector;
    }

    // Dot product
    public static double Dot(VectorSpl a, VectorSpl b)
    {
        return a.x * b.x + a.y * b.y + a.z * b.z;
    }

    // Cross product
    public static VectorSpl Cross(VectorSpl a, VectorSpl b)
    {
        return new VectorSpl(a.y * b.z - a.z * b.y,
                            a.z * b.x - a.x * b.z,
                            a.x * b.y - a.y * b.x);
    }

    // Magnitude (length) of the vector
    public double Magnitude()
    {
        return Math.Sqrt(x * x + y * y + z * z);
    }

    // Normalization of the vector
    // public Vector3D Normalize()
    // {
    //     float magnitude = Magnitude();
    //     if (magnitude == 0)
    //         return this;
    //     else
    //         return new Vector3D(x / magnitude, y / magnitude, z / magnitude);
    // }

    // Override ToString method for readable output
    public override string ToString()
    {
        return "(" + x + ", " + y + ", " + z + ")";
    }
}