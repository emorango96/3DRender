using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRender
{
    public struct Point3D
    {
        public double X;
        public double Y;
        public double Z;

        public Point3D(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Point3D operator +(Point3D a, Point3D b) => new Point3D(a.X + b.X, a.Y + b.Y, a.Z + b.Z);

        public static Point3D operator -(Point3D a, Point3D b) => new Point3D(a.X - b.X, a.Y - b.Y, a.Z - b.Z);

        public static Point3D CrossProduct(Point3D a, Point3D b)
        {
            double x = a.Y * b.Z - a.Z * b.Y;
            double y = a.Z * b.X - a.X * b.Z;
            double z = a.X * b.Y - a.Y * b.X;

            return new Point3D(x, y, z);
        }

        public static double DotProduct(Point3D a, Point3D b) => (double)(a.X * b.X + a.Y * b.Y + a.Z * b.Z);

        public Point3D Normalize()
        {
            double m = Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2) + Math.Pow(Z, 2));
            return new Point3D(X / m, Y / m, Z / m);
        }

        public override string ToString() => $"X: {X}, Y: {Y}, Z: {Z}";
    }
}
