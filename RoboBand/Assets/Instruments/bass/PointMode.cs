using System;
using UnityEngine;

/**
 * <summary> a class contains subclasses and methods to operate with points</summary>
 */
public class PointMode : MonoBehaviour
{
    /**
     * <summary> a class about points (for manipulator) </summary>
     */
    public class point 
    {  /**
        * <value> X, Y, Z - coordinates</value>
        * <value> A, B, C - Euler angles</value>
        */
        public float X, Y, Z, A, B, C;

        /**
         * <value> constructer</value>
         */
        public point(float X, float Y, float Z, float A, float B, float C)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;

            this.A = A; 
            this.B = B;
            this.C = C;
        }

        /**
         * <value> Edit - a method to edit coordinates</value>
         * <param name="X">coordinate</param>
         * <param name="Y">coordinate</param>
         * <param name="Z">coordinate</param>
         */
        public void Edit(float X, float Y, float Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }

        /**
         * <value> Clone - a method to dublicate object</value>
         */
        public point Clone()
        {
            return new point(this.X, this.Y, this.Z, this.A, this.B, this.C);
        }
    }

    /**
     * <summary> a class contains information about point shift</summary>
     */
    public class pointShift
    {
        /**
         * <value> Lx, Ly, Lz - shifts linear</value>
         */
        public float Lx, Ly, Lz;

        /**
         * <value> constructor</value>
         */
        public pointShift(float Lx, float Ly, float Lz) 
        {
            this.Lx = Lx;
            this.Ly = Ly;
            this.Lz = Lz;
        }

        /**
         * <value> Edit -a method to edit shifts</value>
         */
        public void Edit(float Lx, float Ly, float Lz)
        {
            this.Lx = Lx;
            this.Ly = Ly;
            this.Lz = Lz;
        }
    }

    /**
     * <value> Modification - a method calculates point position </value>
     * <param name="angleZX"> guitar rotation</param>
     * <param name="angleZY"> guitar rotation</param>
     * <param name="p"> a point to modificate</param>
     * <param name="ps"> linear shift</param>
     * <returns point> new point (depends on rotation)</returns>
     */
    public point Modification(point p, pointShift ps, float angleZX, float angleZY)
    {
        var cosA = Mathf.Cos(DegToRad(angleZX));
        var sinA = Mathf.Sin(DegToRad(angleZX));
        var cosT = Mathf.Cos(DegToRad(angleZY));
        var sinT = Mathf.Sin(DegToRad(angleZY));
        point point = p.Clone();
        point.Edit((p.X + ps.Lx * cosA * cosT - ps.Ly * sinA * sinT + ps.Lz * sinA), (p.Y + ps.Lx * sinT - ps.Ly * cosT), (p.Z - ps.Lx * sinA * cosT + ps.Ly * sinA * sinT + ps.Lz * cosA));
        return point;
    }

    /**
     * <value> DegToRad - method to calculate radians</value>
     * <param name="angleDeg"> degrees</param>
     * <returns float> radians</returns>
     */
    public float DegToRad(float angleDeg) { 
        return Convert.ToSingle(angleDeg * 3.1416) / 180;
    }
    /**
     * <value> RadToDeg - method to calculate degrees</value>
     * <param name="angleRad"> radians</param>
     * <returns float> degrees</returns>
     */
    public float RadToDeg(float angleRad){ 
    return Convert.ToSingle((angleRad*180)/3.1416);
        }
}

