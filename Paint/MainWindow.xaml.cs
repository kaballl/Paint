using System;
using System.Collections.Generic;
using System.IO;
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
using Microsoft.Win32;

namespace Paint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        PropertiesOfShape ttv = new PropertiesOfShape();
        
        DrawAbstract Draw;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MyCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            
            ttv = new PropertiesOfShape() { };
            Draw = new DrawShape(MyCanvas, ttv, Tools.Line);
            Draw.ActionCreated += new DoActionHandler(OnActionCreated);
        }


      

        private void OnActionCreated(object sender, DoActionEventArgs e)
        {
            undoredoManager.Add(e.action);
        }

        private void cbbSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbbSize.SelectedIndex)
            {
                case 0:
                    ttv.currSize = 2;
                    break;
                case 1:
                    ttv.currSize = 5;
                    break;
                case 2:
                    ttv.currSize = 10;
                    break;
                case 3:
                    ttv.currSize =20;
                    break;
            }
        }

        

        private void cbbOutLine_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (cbbOutLine.SelectedIndex)
            {
                case 0:
                    ttv.OutLineType = new DoubleCollection() { 1, 0 };
                    break;
                case 1:
                    ttv.OutLineType = new DoubleCollection() { 1, 4 };
                    break;
                case 2:
                    ttv.OutLineType = new DoubleCollection() { 4, 1 };
                    break;
                case 3:
                    ttv.OutLineType = new DoubleCollection() { 4, 2, 1, 2 };
                    break;               
            }
        }


        //============================================================================================
        
        
        


        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Draw != null)
                Draw.MouseDown();
            
        }

        private void MyCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (Draw != null)
                Draw.MouseMove();
        }

        private void MyCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Draw != null)
                Draw.MouseUp();
        }

        
        private void btnColor_Click(object sender, RoutedEventArgs e)
        {
                btnColor.Background = (sender as Button).Background;
                ttv.ColorOutLineBrush = btnColor.Background;
        }
        
        private void btnMoreColor_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.ColorDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Color cl = new Color();
                cl.A = dialog.Color.A;
                cl.R = dialog.Color.R;
                cl.G = dialog.Color.G;
                cl.B = dialog.Color.B;

                Brush br = new SolidColorBrush(cl);
                    btnColor.Background = br;


 

                ttv.ColorOutLineBrush = btnColor.Background;

            }
        }


        




        //-----------------------------------------------
       
        

        
        //---------------------------------------------------------
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if ( MyCanvas == null) return;
            Image saveData = Croper.Crop(MyCanvas, 0, 0, MyCanvas.Width, MyCanvas.Height);

            SaveFileDialog saveFileDialogue = new SaveFileDialog()
            {
                FileName = "Untitled",
                Filter = "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf"
            };
            if (saveFileDialogue.ShowDialog() == true)
            {
                //Image saveData = paintLogic.Save();
                BitmapEncoder encoder;
                #region Encoding
                string extension = System.IO.Path.GetExtension(saveFileDialogue.FileName);
                switch (extension.ToLower())
                {
                    case ".jpeg":
                        encoder = new JpegBitmapEncoder();
                        break;
                    case ".png":
                        encoder = new PngBitmapEncoder();
                        break;
                    case ".gif":
                        encoder = new GifBitmapEncoder();
                        break;
                    case ".tiff":
                        encoder = new TiffBitmapEncoder();
                        break;
                    case ".wmf":
                        encoder = new WmpBitmapEncoder();
                        break;
                    case ".bmp":
                        encoder = new BmpBitmapEncoder();
                        break;
                    default:
                        return;
                }
                #endregion
                encoder.Frames.Add(saveData.Source as BitmapFrame);
                using (FileStream file = File.Create(saveFileDialogue.FileName))
                {
                    encoder.Save(file);
                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialogue = new OpenFileDialog()
            {
                Filter = "Bitmap Image (.bmp)|*.bmp|Gif Image (.gif)|*.gif|JPEG Image (.jpeg)|*.jpeg|Png Image (.png)|*.png|Tiff Image (.tiff)|*.tiff|Wmf Image (.wmf)|*.wmf"
            };

            if (openFileDialogue.ShowDialog() == true)
            {
                Stream imageStreamSource = new FileStream(openFileDialogue.FileName, FileMode.Open, FileAccess.Read, FileShare.Read);
                BitmapDecoder decoder;
                #region Decoding
                string extension = System.IO.Path.GetExtension(openFileDialogue.FileName);
                switch (extension.ToLower())
                {
                    case ".jpeg":
                        decoder = new JpegBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    case ".png":
                        decoder = new PngBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    case ".gif":
                        decoder = new GifBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    case ".tiff":
                        decoder = new TiffBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    case ".wmf":
                        decoder = new WmpBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    case ".bmp":
                        decoder = new BmpBitmapDecoder(imageStreamSource, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                        break;
                    default:
                        return;
                }
                #endregion
                BitmapSource LoadedBitmap = decoder.Frames[0];
                MyCanvas.Children.Clear();
                MyCanvas.Width = LoadedBitmap.Width;
                MyCanvas.Height = LoadedBitmap.Height;
                MyCanvas.Children.Add(
                            new Image() { Source = LoadedBitmap }
                        );

                
            }
        }





        //------------------------------------
        // undo redo

        UndoRedoManager undoredoManager = new UndoRedoManager();
        private void Undo_Click(object sender, RoutedEventArgs e)
        {
            undoredoManager.Undo(MyCanvas);
        }

        private void Redo_Click(object sender, RoutedEventArgs e)
        {
            undoredoManager.Redo(MyCanvas);
        }

        private void btnLine_Click(object sender, RoutedEventArgs e)
        {
            Draw = new DrawShape(MyCanvas, ttv, Tools.Line);
            Draw.ActionCreated += new DoActionHandler(OnActionCreated);
        }

        private void btnEllipse_Click(object sender, RoutedEventArgs e)
        {
            Draw = new DrawShape(MyCanvas, ttv, Tools.Ellipse);
            Draw.ActionCreated += new DoActionHandler(OnActionCreated);
        }

        private void btnRetangle_Click(object sender, RoutedEventArgs e)
        {
            Draw = new DrawShape(MyCanvas, ttv, Tools.Rectangle);
            Draw.ActionCreated += new DoActionHandler(OnActionCreated);
        }

        private void btnCircle_Click(object sender, RoutedEventArgs e)
        {
            Draw = new DrawShape(MyCanvas, ttv, Tools.Circle);
            Draw.ActionCreated += new DoActionHandler(OnActionCreated);
        }

        private void btnSquare_Click(object sender, RoutedEventArgs e)
        {
            Draw = new DrawShape(MyCanvas, ttv, Tools.Square);
            Draw.ActionCreated += new DoActionHandler(OnActionCreated);
        }
    }
}
