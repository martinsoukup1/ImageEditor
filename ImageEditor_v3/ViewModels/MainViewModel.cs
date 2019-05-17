using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using GalaSoft.MvvmLight.Command;
using ImageEditor_v3.Models;
using MvvmCross;
using MvvmCross.ViewModels;
using System.IO;

namespace ImageEditor_v3.ViewModels
{
    public class MainViewModel : MvxViewModel
    {

        //Data
        private Bitmap editImageBtm;
        private Bitmap imageBtm;
        private Android.Net.Uri imageUri;
        private Model md;



        //Commandy
        public RelayCommand ImageCommand { get; private set; }
        public RelayCommand SaveImageCommand { get; private set; }



        public MainViewModel()
        {
            ImageCommand = new RelayCommand(ImageSelect);
            SaveImageCommand = new RelayCommand(SaveImage);
            md = new Model();

            /*Bitmap test = Bitmap.CreateBitmap(100, 100, Bitmap.Config.Argb8888);
            Bitmap test2 = Bitmap.CreateBitmap(100, 100, Bitmap.Config.Argb8888);
            Paint p = new Paint();
            p.SetARGB(3000, 2000, 110, 20);
            Canvas canvas = new Canvas(test2);
            canvas.DrawBitmap(test, new Matrix(), p);

            EditImageBtm = test2;*/

            Bitmap bm = BitmapFactory.DecodeFile("donut.png");
            editImageBtm = bm;
        }



        //Property
        public Bitmap EditImageBtm
        {
            get { return editImageBtm; }
            set
            {
                editImageBtm = value;
                RaisePropertyChanged(() =>EditImageBtm);
            }
        }

        public Bitmap ImageBtm
        {
            get { return imageBtm; }
            set
            {
                imageBtm = value;
                RaisePropertyChanged(() => ImageBtm);
            }
        }

        public Android.Net.Uri ImageUri
        {
            get { return imageUri; }
            set
            {
                imageUri = value;
                RaisePropertyChanged(() => ImageUri);
            }
        }



        //Metody
        public void ImageSelect()
        {
            //ImageUri = uri;
            //editImageBtm = md.CreateBitmap(imageUri);
            Bitmap test = Bitmap.CreateBitmap(100, 100, Bitmap.Config.Argb8888);
            Paint p = new Paint();
            p.SetARGB(3000, 2000, 110,20);
            Canvas canvas = new Canvas(test);
            canvas.DrawBitmap(test, new Matrix(), p);

            EditImageBtm = test;
        }

        public void SaveImage()
        {
            string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string ImagesFolder = System.IO.Path.Combine(documentsFolder, "ImageEditor");
            string filename = System.IO.Path.Combine(ImagesFolder, "picture.jpg");

            FileStream fs = null;
            try
            {
                using (fs = new FileStream(filename, FileMode.Create))
                {
                    EditImageBtm.Compress(Bitmap.CompressFormat.Jpeg, 8, fs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("SaveImage exception: " + e.Message);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }

        }

        public void ExposureChanged(float value)
        {
            EditImageBtm = md.ExposureChanged(ImageBtm, value);
        }
        public void ContrastChanged(float value)
        {
            EditImageBtm = md.ContrastChanged(ImageBtm, value);
        }
        public void SaturationChanged(float value)
        {
            EditImageBtm = md.SaturationChanged(ImageBtm, value);
        }

    }
}
