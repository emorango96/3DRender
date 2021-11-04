using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3DRender
{
    public struct RotationMatrix
    {
        public double[,] Elements;
        public double XRot;
        public double YRot;
        public double ZRot;

        public RotationMatrix(double[,] elements, double xAngle = 0, double yAngle = 0, double zAngle = 0)
        {
            Elements = elements;
            XRot = 0;
            YRot = 0;
            ZRot = 0;

            DefineRotationMatrix(xAngle, yAngle, zAngle);
        }

        private void DefineRotationMatrix(double xAngle, double yAngle, double zAngle)
        {
            XRot = xAngle;
            YRot = yAngle;
            ZRot = zAngle;

            Elements = 
                (
                new Matrix(new double[3, 3] { { 1, 0, 0 }, { 0, Math.Cos(XRot), -Math.Sin(XRot)}, { 0, Math.Sin(XRot), Math.Cos(XRot)} }) * 
                new Matrix(new double[3, 3] { { Math.Cos(YRot), 0, Math.Sin(YRot) }, { 0, 1, 0 }, { -Math.Sin(YRot), 0, Math.Cos(YRot) } }) * 
                new Matrix(new double[3, 3] { { Math.Cos(ZRot), -Math.Sin(ZRot), 0 }, { Math.Sin(ZRot), Math.Cos(ZRot), 0}, { 0, 0, 1} })
                ).Elements;
        }        
    }

    public struct Matrix
    {
        public double[,] Elements;

        public Matrix(double[,] elements)
        {
            Elements = elements;
        }

        public static Matrix operator *(Matrix a, Matrix b)
        {
            if (a.Elements.GetLength(0) != b.Elements.GetLength(1))
                throw new InvalidOperationException();

            double[,] r = new double[a.Elements.GetLength(0), b.Elements.GetLength(1)];

            for (int i = 0; i < r.GetLength(0); i++)
                for (int j = 0; j < r.GetLength(1); j++)
                {
                    double res = 0;
                    for (int k = 0; k < a.Elements.GetLength(0); k++)
                        res += a.Elements[i, k] * b.Elements[k, j];

                    r[i, j] = res;
                }

            return new Matrix(r);
        }

        public static Matrix operator +(Matrix a, Matrix b)
        {
            double[,] r = new double[a.Elements.GetLength(0), a.Elements.GetLength(1)];

            for (int i = 0; i < r.GetLength(0); i++)
                for (int j = 0; j < r.GetLength(1); j++)
                    r[i, j] = a.Elements[i, j] + b.Elements[i, j];

            return new Matrix(r);
        }

        public static Matrix operator -(Matrix a, Matrix b)
        {
            double[,] r = new double[a.Elements.GetLength(0), a.Elements.GetLength(1)];

            for (int i = 0; i < r.GetLength(0); i++)
                for (int j = 0; j < r.GetLength(1); j++)
                    r[i, j] = a.Elements[i, j] - b.Elements[i, j];

            return new Matrix(r);
        }

        public void Transpose()
        {
            double[,] x = Elements;

            for (int i = 0; i < Elements.GetLength(0); i++)
                for (int j = 0; j < Elements.GetLength(1); j++)
                    Elements[j, i] = x[i, j];
        }

        public void Add(double [] a)
        {
            for (int i = 0; i < Elements.GetLength(0); i++)
                for (int j = 0; j < Elements.GetLength(0); j++)
                    Elements[i, j] += a[j];

        }
    }    
}
