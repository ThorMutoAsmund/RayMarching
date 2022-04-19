using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RayMarching
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RayMarcher rayMarcher;

        public MainWindow()
        {
            InitializeComponent();

            this.rayMarcher = new RayMarcher();

            this.rayMarcher.AddCamera(new StandardCamera(new Vector3(0d, 5d, -30d), new Vector3(0d, 2d, 0d), 800d));
            this.rayMarcher.AddObject(new PlaneObject(0d, 320d, new SolidMaterial(Color.FromArgb(255,40,40,90))));
            this.rayMarcher.AddObject(new SphereObject(new Vector3(0d, 2d, 0d), 2d, new SolidMaterial(Color.Red)));
            this.rayMarcher.AddObject(
                new BooleanObject(
                new SphereObject(new Vector3(7.5d, 1d, -1d), 2d),
                new CubeObject(new Vector3(8.5d, 2d, 0d), new Vector3(2d, 2d, 2d)),
                BooleanOperator.Subtraction,
                material: new SolidMaterial(Color.Yellow)));

            this.rayMarcher.AddObject(
                new BooleanObject(
                new CubeObject(new Vector3(-5.5d, 5d, 0d), new Vector3(1.5d, 1.5d, 1.5d)),
                new SphereObject(new Vector3(-2.2d, 5d, 0d), 1.5d),
                BooleanOperator.SmoothCombined,
                smoothness: 3,
                material: new SolidMaterial(Color.FromArgb(255,200,200,180))));


            this.rayMarcher.AddObject(new CubeObject(new Vector3(-6d, 1d, 0d), new Vector3(1d, 2d, 1d), new SolidMaterial(Color.Green)));

            this.Loaded += (sender, e) => RenderImage();
        }

        private void RenderImage()
        {
            this.rayMarcher.SetSize((int)this.MainGrid.ActualWidth - 4, (int)this.MainGrid.ActualHeight - 2);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            this.rayMarcher.RenderImageTo(this.MainImage);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Debug.Write($"Execution time {elapsedMs}");
        }
    }
}
