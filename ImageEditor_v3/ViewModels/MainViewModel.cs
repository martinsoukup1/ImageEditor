using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using GalaSoft.MvvmLight.Command;
using ImageEditor_v3.Models;
using MvvmCross.ViewModels;
using System.IO;
using Android.Provider;

namespace ImageEditor_v3.ViewModels
{
    public class MainViewModel : MvxViewModel
    {

        //Data
        private Bitmap editImageBtm;
        private Bitmap imageBtm;
        private Model md;
        private Activity act;

        private static readonly int SELECT_FILE = 1;


        //Commandy
        public RelayCommand ImageCommand { get; private set; }
        public RelayCommand SaveImageCommand { get; private set; }



        public MainViewModel(Activity act)
        {
            this.act = act;

            ImageCommand = new RelayCommand(SelectImage);
            SaveImageCommand = new RelayCommand(SaveImage);

            md = new Model();
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



        //Metody
        public void SelectImage()
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this.act);
            Android.App.AlertDialog alert = dialog.Create();
            alert.SetTitle("Select Image");
            alert.SetMessage("Select image from");
            alert.SetButton2("Gallery", (c, ev) =>
            {
                var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                intent.SetType("image/*");
                this.act.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), SELECT_FILE);
            });
            alert.SetButton3("CANCEL", (c, ev) => { });
            alert.Show();

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

        //Ukládá, ale v galerii neukazuje
        public void SaveImage()
        {
            try
            {
                string myDate = DateTime.Now.TimeOfDay.ToString() + ".jpg";
                using (var os = new System.IO.FileStream(Android.OS.Environment.ExternalStorageDirectory + "/DCIM/Camera/" + myDate, System.IO.FileMode.CreateNew))
                {
                    EditImageBtm.Compress(Bitmap.CompressFormat.Jpeg, 95, os);
                    Console.WriteLine("image saved");
                    os.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

    }
}
