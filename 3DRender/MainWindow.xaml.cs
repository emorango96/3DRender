using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Timers;
using Microsoft.Win32;
using System.ComponentModel;

namespace _3DRender
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public double FOV = 45.0;
        public double tan_FOV;

        public double DistanceToScreen;

        Triangle[] Shape;
        Triangle[] TransformedShape;

        private double _zoom = 2;
        public double ZoomConstant
        {
            get { return _zoom; }
            set
            {
                if (_zoom != value)
                {
                    _zoom = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZoomConstant)));
                }
            }
        }
        Timer Timer;

        private double _xR = 3.1416;
        public double XRotAngle
        {
            get { return _xR; }
            set
            {
                if (_xR != value)
                    _xR = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(XRotAngle)));
            }
        }

        private double _yR = 0;
        public double YRotAngle
        {
            get { return _yR; }
            set
            {
                if (_yR != value)
                    _yR = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(YRotAngle)));
            }
        }

        private double _zR = 0;
        public double ZRotAngle
        {
            get { return _zR; }
            set
            {
                if (_zR != value)
                    _zR = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ZRotAngle)));
            }
        }

        private double _eTime = 0;
        public double ETime
        {
            get { return _eTime; }
            set
            {
                if (_eTime != value)
                    _eTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ETime)));
            }
        }

        private double _speed = 0.04;
        public double Speed
        {
            get { return _speed; }
            set
            {
                if (_speed != value)
                    _speed = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Speed)));
            }
        }

        private bool _sE = false;
        public bool StopWatchEnabled
        {
            get { return _sE; }
            set
            {
                _sE = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StopWatchEnabled)));
            }
        }

        double XT = 0;
        double YT = 0;
        double ZT = 0;

        System.Diagnostics.Stopwatch Monitor;

        System.IO.StreamReader Reader;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();
            DistanceToScreen = 240 / Math.Tan(FOV * Math.PI / 180);

            Monitor = new System.Diagnostics.Stopwatch();

            //ExportObj();

            //DrawShape(Shape);

            tan_FOV = Math.Tan(FOV);
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left && Keyboard.IsKeyDown(Key.LeftCtrl))
                XT -= .2;

            else if (e.Key == Key.Right && Keyboard.IsKeyDown(Key.LeftCtrl))
                XT += .2;

            else if (e.Key == Key.Up && Keyboard.IsKeyDown(Key.LeftCtrl))
                YT += .2;

            else if (e.Key == Key.Down && Keyboard.IsKeyDown(Key.LeftCtrl))
                YT -= .2;

            else if (e.Key == Key.W && Keyboard.IsKeyDown(Key.LeftCtrl))
                ZT -= .2;

            else if (e.Key == Key.S && Keyboard.IsKeyDown(Key.LeftCtrl))
                ZT += .2;

            else if (e.Key == Key.Right || e.Key == Key.Left)
                YRotAngle += (e.Key == Key.Right) ? -0.08 : 0.08;

            else if (e.Key == Key.Up || e.Key == Key.Down)
                XRotAngle += (e.Key == Key.Up) ? -0.08 : 0.08;

            else if (e.Key == Key.A || e.Key == Key.D)
                ZRotAngle += (e.Key == Key.A) ? -0.08 : 0.08;

            else if (e.Key == Key.W || e.Key == Key.S)
                ZoomConstant += (e.Key == Key.W) ? -.2 : .2;

            else if (e.Key == Key.R)
                XRotAngle = YRotAngle = ZRotAngle = XT = YT = ZT = 0;

            else if (e.Key == Key.Escape)
                Close();

            else if (e.Key == Key.O)
                DrawASimpleTriangle();

            else if (e.Key == Key.T)
            {
                ToggleStopWatch();
                return;
            }
            else if (e.Key == Key.Space)
            {
                ToggleTimer();
                return;
            }

            if (Shape != null)
                DrawShapeFull(Shape, XRotAngle, YRotAngle, ZRotAngle);
        }

        private void ToggleTimer()
        {
            if (Timer != null)
                Timer.Enabled = !Timer.Enabled;
        }

        private void ToggleStopWatch()
        {
            StopWatchEnabled = !StopWatchEnabled;
        }

        public Task ExportObj(string path = null)
        {
            if (path != null)
                Reader = new System.IO.StreamReader(path);
            else
                Reader = new System.IO.StreamReader(@"C:\Users\ICAP\Desktop\Shapes\icosahedron.obj");

            List<double[]> vValues = new List<double[]>();
            List<int[]> iNumbers = new List<int[]>();

            string line;
            string[] values;
            while ((line = Reader.ReadLine()) != null)
            {
                if (line != "" && line[0] == 'v' && !line.Contains("vn"))
                {
                    values = line.Split(' ');
                    double[] num = new double[3];
                    int i = 0;
                    foreach (string s in values)
                        if (s != "v" && s != "")
                        {
                            num[i] = double.Parse(s);
                            i++;
                        }

                    vValues.Add(num);
                }
                else if (line != "" && line[0] == 'f')
                {
                    values = line.Split(' ');
                    int[] num = new int[3];
                    int i = 0;
                    foreach (string s in values)
                        if (s != "f" && s != "")
                        {
                            if (s.Contains("//"))
                            {
                                int index = 0;
                                for (; index < s.Length; index++)
                                    if (s[index] == '/')
                                        break;

                                num[i] = int.Parse(s.Substring(0, index));
                            }
                            else
                                num[i] = int.Parse(s);
                            i++;
                        }

                    iNumbers.Add(num);
                }
            }

            Shape = new Triangle[iNumbers.Count];
            for (int i = 0; i < iNumbers.Count; i++)
            {
                Shape[i] = new Triangle(new Point3D[]
                {
                    new Point3D(vValues[iNumbers[i][0] - 1][0], vValues[iNumbers[i][0] - 1][1], vValues[iNumbers[i][0] - 1][2]),
                    new Point3D(vValues[iNumbers[i][1] - 1][0], vValues[iNumbers[i][1] - 1][1], vValues[iNumbers[i][1] - 1][2]),
                    new Point3D(vValues[iNumbers[i][2] - 1][0], vValues[iNumbers[i][2] - 1][1], vValues[iNumbers[i][2] - 1][2])
                });
            }

            return Task.CompletedTask;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            YRotAngle += Speed;
                        
            if (StopWatchEnabled)
            {
                Monitor = System.Diagnostics.Stopwatch.StartNew();
                DrawShapeFull(Shape, XRotAngle, YRotAngle, ZRotAngle);
                Monitor.Stop();
                ETime = Monitor.ElapsedMilliseconds;
            }
            else
                DrawShapeFull(Shape, XRotAngle, YRotAngle, ZRotAngle);
        }

        private void DrawASimpleTriangle()
        {
            GeometryGroup gg = new GeometryGroup();
            gg.Children.Add(new LineGeometry(new Point(0, 0), new Point(0, 50)));
            gg.Children.Add(new LineGeometry(new Point(0, 50), new Point(50, 50)));
            gg.Children.Add(new LineGeometry(new Point(50, 50), new Point(0, 0)));
            Path tPath = new Path() { Data = gg, Stroke = Brushes.Red, Fill = Brushes.Red, StrokeThickness = 1 };
            _ = Workspace.Children.Add(tPath);
        }

        public Point[] TransformCoordinates(params Point3D[] _3dPoints)
        {
            System.Windows.Point[] projectedPoints = new Point[_3dPoints.Length];
            for (int i = 0; i < projectedPoints.Length; i++)
            {
                projectedPoints[i].X = 1 / (_3dPoints[i].Z + 1 + ZoomConstant) * _3dPoints[i].X * 480 + 240;
                projectedPoints[i].Y = 1 / (_3dPoints[i].Z + 1 + ZoomConstant) * _3dPoints[i].Y * 480 + 240;
            }

            return projectedPoints;
        }

        public void DrawShapeFull(Triangle[] shape, double xRot, double yRot, double zRot)
        {
            if (!Dispatcher.HasShutdownStarted)
                Dispatcher.Invoke(new Action(() =>
                {
                    Workspace.Children.Clear();

                    //foreach (Triangle triangle in shape)
                        //Workspace.Children.Add(triangle.DrawTriangle(TransformCoordinates(MatrixOperations.FullRotation(MatrixOperations.ApplyTranslation(triangle.Points, XT, YT, ZT), xRot, yRot, zRot)), false));

                    System.Diagnostics.Stopwatch a = System.Diagnostics.Stopwatch.StartNew();
                    foreach (Triangle triangle in shape)
                        Workspace.Children.Add(triangle.DrawTriangle(TransformCoordinates(MatrixOperations.FullSpatialTransformation(triangle.Points, xRot, yRot, zRot, XT, YT, ZT)), true));
                    a.Stop();
                    _ = a.ElapsedMilliseconds;

                    a.Restart();
                    Triangle t = shape[0];

                    Path p = t.DrawTriangle(TransformCoordinates(MatrixOperations.FullSpatialTransformation(t.Points, xRot, yRot, zRot, XT, YT, ZT)), false);
                    Workspace.Children.Add(p);
                    a.Stop();
                    long tA = a.ElapsedMilliseconds;
                    _ = tA;
                }));
        }

        private async void DumbButton_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            if (!(ofd.ShowDialog() ?? false))
                return;

            if (!ofd.FileName.Contains(".obj"))
            {
                _ = MessageBox.Show("No se puede exportar ese formato");
                return;
            }
                
            await ExportObj(ofd.FileName);

            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();
            DrawShapeFull(Shape, XRotAngle, YRotAngle, ZRotAngle);
            watch.Stop();
            MessageBox.Show(watch.ElapsedMilliseconds.ToString());
            InitializeTimer(watch.ElapsedMilliseconds);
        }

        private void InitializeTimer(long ms)
        {
            if (Timer is null)
            {
                Timer = new Timer(ms) { AutoReset = true };
                Timer.Elapsed += Timer_Elapsed;
            }            
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            ZoomConstant += e.Delta > 0 ? .2 : -.2;

            if (e.Delta > 0)
            {
                ZoomConstant += .2;
                DrawShapeFull(Shape, XRotAngle, YRotAngle, ZRotAngle);
            }
            else if (e.Delta < 0)
            {
                ZoomConstant -= .2;
                DrawShapeFull(Shape, XRotAngle, YRotAngle, ZRotAngle);
            }
        }
    }    
}
