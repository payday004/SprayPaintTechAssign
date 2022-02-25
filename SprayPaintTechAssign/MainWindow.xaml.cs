using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.IO;
using System.Windows.Ink;



namespace SprayPaintTechAssign
{
    public static class penEdit
    {
        public static Color colorSelector = Colors.Black;
        public static int Radius = 5;
        public static double Opacity = .10;


        public static void setColor(Color inColor)
        {
            colorSelector = inColor; 
        }


        public static int getRadius()
        {
            return Radius; 
        }
        public static void incRadius()
        {
            if(Radius < 20)
            {
                Radius = Radius + 1;
            }
        }
        public static void decRadius()
        {
            if(Radius > 5)
            {
                Radius = Radius - 1;
            }
        }


        public static void incOpacity()
        {
            if (Opacity < 1)
            {
                Opacity = Opacity + .1;
            }
        }
        public static void decOpacity()
        {
            if (Opacity > 0)
            {
                Opacity = Opacity - .1;
            }
        }
    }



    public partial class MainWindow : Window
    {
        Image globalImage;


        public MainWindow()
        {
            InitializeComponent();
        }


        private void MenuItem_Open(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";
            dlg.RestoreDirectory = true;

            if (dlg.ShowDialog() == true)
            {
                inkCanvas.Children.Clear();

                string selectedFileName = dlg.FileName;

                //try to open and display image
                try
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(selectedFileName);
                    bitmap.EndInit();


                    Image image = new Image
                    {
                        Width = 500,
                        Source = bitmap,
                    };

                    inkCanvas.Children.Add(image);

                    globalImage = image;
                }

                // catch not an image file 
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
 
            }
        }
        private void MenuItem_Save(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Image files (*.jpg)|*.jpg|All Files (*.*)|*.*";

            if (saveFileDialog1.ShowDialog() == true)
            {
                /// IMPLEMENT SAVE IMAGE WITH INK ON TOP HERE as bitmap

                // render InkCanvas' visual tree to the RenderTargetBitmap

                RenderTargetBitmap targetBitmap =

                new RenderTargetBitmap((int)globalImage.ActualWidth,

                (int)globalImage.ActualHeight, 

                96d, 96d,

                PixelFormats.Default);



                targetBitmap.Render(inkCanvas);


                // add the RenderTargetBitmap to a Bitmapencoder

                BmpBitmapEncoder encoder = new BmpBitmapEncoder();

                encoder.Frames.Add(BitmapFrame.Create(targetBitmap));

                // save file to disk

                FileStream fs = File.Open(saveFileDialog1.FileName, FileMode.OpenOrCreate);

                encoder.Save(fs);

            }
        }
        private void LoadInk_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "isf files (*.isf)|*.isf";

            if (openFileDialog1.ShowDialog() == true)
            {
                FileStream fs = new FileStream(openFileDialog1.FileName,
                                               FileMode.Open);
                inkCanvas.Strokes = new StrokeCollection(fs);
                fs.Close();
            }
        }
        private void SaveInk_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "isf files (*.isf)|*.isf";

            if (saveFileDialog1.ShowDialog() == true)
            {
                FileStream fs = new FileStream(saveFileDialog1.FileName,
                                               FileMode.Create);
                inkCanvas.Strokes.Save(fs);
                fs.Close();
            }
        }



        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Strokes.Clear();
        }
        private void Eraser_Click(object sender, RoutedEventArgs e)
        {

            //MessageBox.Show(inkCanvas.EditingMode.ToString()); 
            if (inkCanvas.EditingMode == InkCanvasEditingMode.EraseByPoint)
            {
                inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            }
            else
            {
                inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
            }

        }



        private void Color_Click(object sender, RoutedEventArgs e)
        {

            inkCanvas.EditingMode = InkCanvasEditingMode.Ink; 

            string[] stringTuple = sender.ToString().Split(' ');

            switch (stringTuple[1])
            {
                case "Red":
                    penEdit.setColor(Colors.Red);
                    break;
                case "Green":
                    penEdit.setColor(Colors.Green);
                    break;
                case "Blue":
                    penEdit.setColor(Colors.Blue);
                    break;
                case "Black":
                    penEdit.setColor(Colors.Black);
                    break;
                case "White":
                    penEdit.setColor(Colors.White);
                    break;
            }
        }



        private void WidthPlus_Click(object sender, RoutedEventArgs e)
        {
            penEdit.incRadius();
            wText.Text = "Width: " + penEdit.Radius;

        }
        private void WidthMinus_Click(object sender, RoutedEventArgs e)
        {
            penEdit.decRadius();
            wText.Text = "Width: " + penEdit.Radius;
        }



        private void OpacityPlus_Click(object sender, RoutedEventArgs e)
        {
            penEdit.incOpacity();
            oText.Text = "Opacity: " + (int)(penEdit.Opacity * 10);
        }
        private void OpacityMinus_Click(object sender, RoutedEventArgs e)
        {
            penEdit.decOpacity();
            oText.Text = "Opacity: " + (int)(penEdit.Opacity * 10);
        }

    }
}
