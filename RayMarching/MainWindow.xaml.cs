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

            this.rayMarcher.AddCamera(new StandardCamera(new Vector3(0d, 4d, -20d), new Vector3(0d, 0d, 0d), 800d));
            this.rayMarcher.AddObject(new PlaneObject(0d, new SolidMaterial(Color.FromArgb(255,40,40,40))));
            this.rayMarcher.AddObject(new SphereObject(new Vector3(0d, 2d, 0d), 2d, new SolidMaterial(Color.Red)));
            this.rayMarcher.AddObject(new SphereObject(new Vector3(5d, 2d, 0d), 2d, new SolidMaterial(Color.Yellow)));
            this.rayMarcher.AddObject(new CubeObject(new Vector3(-6d, 1d, 0d), new Vector3(1d, 1d, 1d), new SolidMaterial(Color.Green)));

            this.SizeChanged += (sender, e) => RenderImage();
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
