using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRender
{
    public static class MatrixOperations
    {
        public static Point3D RotateX(Point3D point, double angleRad = 0)
        {
            double[] p = { point.X, point.Y, point.Z };
            double[] r = new double[3];
            double[,] xMatrix =
            {
                { 1, 0, 0 },
                { 0, Math.Cos(angleRad), -Math.Sin(angleRad)},
                { 0, Math.Sin(angleRad), Math.Cos(angleRad)}
            };

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    double res = 0;
                    for (int k = 0; k < 3; k++)
                        res += xMatrix[i, k] * p[k];

                    r[i] = res;
                }

            return new Point3D(r[0], r[1], r[2]);
        }

        public static Point3D[] RotateXTriangle(Point3D[] points, double angleRad = 0)
        {
            Point3D[] rotatedPoints = new Point3D[3];
            double[,] xMatrix =
            {
                { 1, 0, 0 },
                { 0, Math.Cos(angleRad), -Math.Sin(angleRad)},
                { 0, Math.Sin(angleRad), Math.Cos(angleRad)}
            };

            for (int m = 0; m < 3; m++)
            {
                double[] p = { points[m].X, points[m].Y, points[m].Z };
                double[] r = new double[3];

                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        double res = 0;
                        for (int k = 0; k < 3; k++)
                            res += xMatrix[i, k] * p[k];

                        r[i] = res;
                    }

                rotatedPoints[m] = new Point3D(r[0], r[1], r[2]);
            }
            return rotatedPoints;
        }

        public static Point3D[] RotateZTriangle(Point3D[] points, double angleRad = 0)
        {
            Point3D[] rotatedPoints = new Point3D[3];
            double[,] xMatrix =
            {
                { Math.Cos(angleRad), -Math.Sin(angleRad), 0 },
                { Math.Sin(angleRad), Math.Cos(angleRad), 0},
                { 0, 0, 1}
            };

            for (int m = 0; m < 3; m++)
            {
                double[] p = { points[m].X, points[m].Y, points[m].Z };
                double[] r = new double[3];

                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        double res = 0;
                        for (int k = 0; k < 3; k++)
                            res += xMatrix[i, k] * p[k];

                        r[i] = res;
                    }

                rotatedPoints[m] = new Point3D(r[0], r[1], r[2]);
            }
            return rotatedPoints;
        }

        public static Point3D[] RotateYTriangle(Point3D[] points, double angleRad = 0)
        {
            Point3D[] rotatedPoints = new Point3D[3];
            double[,] xMatrix =
            {
                { Math.Cos(angleRad), 0, Math.Sin(angleRad) },
                { 0, 1, 0 },
                { -Math.Sin(angleRad), 0, Math.Cos(angleRad) }
            };

            for (int m = 0; m < 3; m++)
            {
                double[] p = { points[m].X, points[m].Y, points[m].Z };
                double[] r = new double[3];

                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        double res = 0;
                        for (int k = 0; k < 3; k++)
                            res += xMatrix[i, k] * p[k];

                        r[i] = res;
                    }

                rotatedPoints[m] = new Point3D(r[0], r[1], r[2]);
            }
            return rotatedPoints;
        }

        public static Point3D[] FullRotation(Point3D[] points, double xRot = 0, double yRot = 0, double zRot = 0)
        {
            Point3D[] rotatedPoints = new Point3D[3];

            double[,] fullMatrix =
            {
                { Math.Cos(yRot) * Math.Cos(zRot), -Math.Cos(yRot) * Math.Sin(zRot), Math.Sin(yRot) },
                { Math.Sin(xRot) * Math.Sin(yRot) * Math.Cos(zRot) + Math.Cos(xRot) * Math.Sin(zRot), -Math.Sin(xRot) * Math.Sin(yRot) * Math.Sin(zRot) + Math.Cos(xRot) * Math.Cos(zRot), -Math.Sin(xRot) * Math.Cos(yRot) },
                { -Math.Cos(xRot) * Math.Sin(yRot) * Math.Cos(zRot) + Math.Sin(xRot) * Math.Sin(zRot), Math.Cos(xRot) * Math.Sin(yRot) * Math.Sin(zRot) + Math.Sin(xRot) * Math.Cos(zRot), Math.Cos(xRot) * Math.Cos(yRot)}
            };

            for (int m = 0; m < 3; m++)
            {
                double[] p = { points[m].X, points[m].Y, points[m].Z };
                double[] r = new double[3];

                for (int i = 0; i < 3; i++)
                    for (int j = 0; j < 3; j++)
                    {
                        double res = 0;
                        for (int k = 0; k < 3; k++)
                            res += fullMatrix[i, k] * p[k];

                        r[i] = res;
                    }

                rotatedPoints[m] = new Point3D(r[0], r[1], r[2]);
            }
            return rotatedPoints;
        }

        public static Point3D[] ApplyTranslation(Point3D[] points, double xTr, double yTr, double zTr)
        {
            Point3D[] translatedPoints = new Point3D[points.Length];
            Point3D translationVector = new Point3D(xTr, yTr, zTr);

            for (int i = 0; i < points.Length; i++)
                translatedPoints[i] = points[i] + translationVector;

            return translatedPoints;
        }

        public static Point3D[] FullSpatialTransformation(Point3D[] points, double xRot = 0, double yRot = 0, double zRot = 0, double xTr = 0, double yTr = 0, double zTr = 0)
        {
            Point3D[] transformatedPoints = new Point3D[points.Length];

            double[,] fullMatrix =
            {
                { Math.Cos(yRot) * Math.Cos(zRot), -Math.Cos(yRot) * Math.Sin(zRot), Math.Sin(yRot), xTr },
                { Math.Sin(xRot) * Math.Sin(yRot) * Math.Cos(zRot) + Math.Cos(xRot) * Math.Sin(zRot), -Math.Sin(xRot) * Math.Sin(yRot) * Math.Sin(zRot) + Math.Cos(xRot) * Math.Cos(zRot), -Math.Sin(xRot) * Math.Cos(yRot), yTr },
                { -Math.Cos(xRot) * Math.Sin(yRot) * Math.Cos(zRot) + Math.Sin(xRot) * Math.Sin(zRot), Math.Cos(xRot) * Math.Sin(yRot) * Math.Sin(zRot) + Math.Sin(xRot) * Math.Cos(zRot), Math.Cos(xRot) * Math.Cos(yRot), zTr },
                { 0, 0, 0, 1 }
            };

            for (int i = 0; i < points.Length; i++)
            {
                double[] vec = new double[4] { points[i].X, points[i].Y, points[i].Z, 1 };
                double[] res = MatrixMultiplication(fullMatrix, vec);
                transformatedPoints[i] = new Point3D(res[0], res[1], res[2]);
            }

            return transformatedPoints;
        }

        public static Point3D Rotate(Point3D point, double xRot = 0, double yRot = 0, double zRot = 0)
        {
            double[,] fRMatrix =
            {
                { Math.Cos(yRot) * Math.Cos(zRot), -Math.Cos(yRot) * Math.Sin(zRot), Math.Sin(yRot) },
                { Math.Sin(xRot) * Math.Sin(yRot) * Math.Cos(zRot) + Math.Cos(xRot) * Math.Sin(zRot), -Math.Sin(xRot) * Math.Sin(yRot) * Math.Sin(zRot) + Math.Cos(xRot) * Math.Cos(zRot), -Math.Sin(xRot) * Math.Cos(yRot) },
                { -Math.Cos(xRot) * Math.Sin(yRot) * Math.Cos(zRot) + Math.Sin(xRot) * Math.Sin(zRot), Math.Cos(xRot) * Math.Sin(yRot) * Math.Sin(zRot) + Math.Sin(xRot) * Math.Cos(zRot), Math.Cos(xRot) * Math.Cos(yRot)}
            };

            double[] r = new double[3];
            double[] p = new double[3] { point.X, point.Y, point.Z };

            for (int i = 0; i < fRMatrix.GetLength(0); i++)
                for (int j = 0; j < fRMatrix.GetLength(1); j++)
                {
                    double res = 0;
                    for (int k = 0; k < fRMatrix.GetLength(1); k++)
                        res += fRMatrix[i, k] * p[k];

                    r[i] = res;
                }

            return new Point3D(r[0], r[1], r[2]);
        }

        public static double[,] MatrixMultiplication(double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(1))
                throw new InvalidOperationException();

            double[,] r = new double[a.GetLength(0), b.GetLength(1)];

            for (int i = 0; i < r.GetLength(0); i++)
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    double res = 0;
                    for (int k = 0; k < a.GetLength(0); k++)
                        res += a[i, k] * b[k, j];

                    r[i, j] = res;
                }

            return r;
        }

        public static double[] MatrixMultiplication(double[,] a, double[] b)
        {
            if (a.GetLength(0) != b.Length)
                throw new InvalidOperationException("Matrix row number must be equal to vector rows");

            double[] r = new double[b.Length];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < b.Length; j++)
                {
                    double res = 0;
                    for (int k = 0; k < a.GetLength(0); k++)
                        res += a[i, k] * b[k];

                    r[i] = res;
                }

            return r;
        }

        public static double[,] MatrixAddition(double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                throw new InvalidOperationException("Both matrices must have same row and column numbers");

            double[,] r = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    r[i, j] = a[i, j] + b[i, j];

            return r;
        }

        public static double[,] MatrixSubstraction(double[,] a, double[,] b)
        {
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                throw new InvalidOperationException("Both matrices must have same row and column numbers");

            double[,] r = new double[a.GetLength(0), a.GetLength(1)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    r[i, j] = a[i, j] - b[i, j];

            return r;
        }

        public static T [,] Transpose<T>(T[,] a)
        {
            T[,] res = new T[a.GetLength(1), a.GetLength(0)];

            for (int i = 0; i < a.GetLength(0); i++)
                for (int j = 0; j < a.GetLength(1); j++)
                    res[j, i] = a[i, j];

            return res;
        }
        
        /*public static double GetDeterminant(double[,] a)
        {
            if (a.GetLength(0) != a.GetLength(1))
                throw new InvalidOperationException("Cannot get the determinant of a non-square matrix");
            else if (a.GetLength(0) == a.GetLength(1) && a.GetLength(0) == 1)
                return a[0, 0];            

            for (int j = 0; j < a.GetLength(1); j++)
            {

            }
        }*/

        /*public static double GetDeterminant(double[,] a)
        {

            if (a.GetLength(0) == 2)
                return a[0, 0] * a[1, 1] - a[0, 1] * a[1, 0];
            else
            {
                double sum = 0;
                int len = a.GetLength(0);
                for (int i = 0; i < len; i++)
                {
                    double[,] b = new double[len - 1, len - 1];

                    for (int j = 0; j < len - 1; j++)
                    {
                        b[]
                    }

                    sum += (0 - i % 2) * GetDeterminant(b);
                }

                return sum;
            }
        }*/
    }
}
