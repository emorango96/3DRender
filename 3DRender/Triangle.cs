using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace _3DRender
{
    public struct Triangle
    {
        public Point3D[] Points;
        public Point3D CrossProduct;

        public Triangle(Point3D[] points)
        {
            Points = points;

            Point3D aVector = points[1] - points[0];
            Point3D bVector = points[2] - points[0];

            CrossProduct = Point3D.CrossProduct(aVector, bVector).Normalize();
        }

        public Path DrawTriangle(Point[] points, bool fill)
        {
            GeometryGroup triangle = new GeometryGroup() { FillRule = FillRule.Nonzero};

            triangle.Children.Add(new LineGeometry(new Point(points[0].X, points[0].Y), new Point(points[1].X, points[1].Y)));
            triangle.Children.Add(new LineGeometry(new Point(points[1].X, points[1].Y), new Point(points[2].X, points[2].Y)));
            triangle.Children.Add(new LineGeometry(new Point(points[2].X, points[2].Y), new Point(points[0].X, points[0].Y)));

            Path trianglePath = new Path() { Data = triangle, Stroke = Brushes.White, StrokeThickness = 1, Fill = fill ? Brushes.Black : Brushes.Transparent };
            return trianglePath;
        }
    }
}
