using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using GalaSoft.MvvmLight.Views;
using ImageEditor_v3.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using Android.Views;
using Android.Content;
using Android.Provider;
using System.IO;

namespace ImageEditor_v3
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : ActivityBase
    {
        SeekBar exposure;
        SeekBar contrast;
        SeekBar saturation;
        ImageView imgView;
        //Bitmap bitmap;
        //Bitmap btm;
        Button selectImg;
        Button savetImg;


        //private static readonly int REQUEST_CAMERA = 0;
        private static readonly int SELECT_FILE = 1;

        MainViewModel vm;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            vm = new MainViewModel();

            exposure = FindViewById<SeekBar>(Resource.Id.exposure);
            exposure.ProgressChanged += ExposureChanged;

            contrast = FindViewById<SeekBar>(Resource.Id.contrast);
            contrast.ProgressChanged += ContrastChanged;

            saturation = FindViewById<SeekBar>(Resource.Id.saturation);
            saturation.ProgressChanged += SaturatinChanged;

            selectImg = FindViewById<Button>(Resource.Id.selectImage);

            savetImg = FindViewById<Button>(Resource.Id.saveImage);

            imgView = FindViewById<ImageView>(Resource.Id.imageViewer);

            imgView.BuildDrawingCache(true);

            //bitmap = imgView.GetDrawingCache(true);

            //textViewBinding = this.SetBinding(() => vm.Text, () => txtView.Text);

            selectImg.SetCommand("Click", vm.ImageCommand);
            selectImg.Click += SelectImage_btnClick;

            savetImg.SetCommand("Click", vm.SaveImageCommand);



        }

        private void ExposureChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            float value = (e.Progress -100);

            vm.ExposureChanged(value);
            /*imgView.BuildDrawingCache(true);
            bitmap = imgView.GetDrawingCache(true);

            btm = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, bitmap.GetConfig());

            ColorMatrix clrMatrix = new ColorMatrix(new float[] {
                1, 0, 0, 0, value,
                0, 1, 0, 0, value,
                0, 0, 1, 0, value,
                0, 0, 0, 1, 0 });

            ColorMatrixColorFilter imgMtrxFilter = new ColorMatrixColorFilter(clrMatrix);
            Paint paint = new Paint();
            paint.SetColorFilter(imgMtrxFilter);
            Canvas canvas = new Canvas(btm);

            canvas.DrawBitmap(bitmap, new Matrix(), paint);
            imgView.SetImageBitmap(btm);*/
        }

        private void ContrastChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            float value = (e.Progress - 100);

            vm.ContrastChanged(value);

            /*float input = value / 100;

            float scale = input + 1f;
            float contrast = (-0.5f * scale + 0.5f) * 255f;

            imgView.BuildDrawingCache(true);
            bitmap = imgView.GetDrawingCache(true);

            btm = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, bitmap.GetConfig());

            ColorMatrix clrMatrix = new ColorMatrix(new float[] {
                scale, 0, 0, 0, contrast,
                0, scale, 0, 0, contrast,
                0, 0, scale, 0, contrast,
                0, 0, 0, 1, 0 });

            ColorMatrixColorFilter imgMtrxFilter = new ColorMatrixColorFilter(clrMatrix);
            Paint paint = new Paint();
            paint.SetColorFilter(imgMtrxFilter);
            Canvas canvas = new Canvas(btm);

            canvas.DrawBitmap(bitmap, new Matrix(), paint);
            imgView.SetImageBitmap(btm);*/
        }

        private void SaturatinChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            float value = (e.Progress*0.01f);

            vm.SaturationChanged(value);
            /*imgView.BuildDrawingCache(true);
            bitmap = imgView.GetDrawingCache(true);

            btm = Bitmap.CreateBitmap(bitmap.Width, bitmap.Height, bitmap.GetConfig());

            ColorMatrix clrMatrix = new ColorMatrix();
            clrMatrix.SetSaturation(value);

            ColorMatrixColorFilter imgMtrxFilter = new ColorMatrixColorFilter(clrMatrix);
            Paint paint = new Paint();
            paint.SetColorFilter(imgMtrxFilter);
            Canvas canvas = new Canvas(btm);

            canvas.DrawBitmap(bitmap, new Matrix(), paint);
            imgView.SetImageBitmap(btm);*/
        }

        private void SelectImage_btnClick(object sender, EventArgs e)
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
            Android.App.AlertDialog alert = dialog.Create();
            alert.SetTitle("Select Image");
            alert.SetMessage("Select image from");
            /*alert.SetButton("Camera", (c, ev) =>
            {
                var intent = new Intent(MediaStore.ActionImageCapture);
                File file = new File(Android.App._dir, String.Format("image_{0}.jpg", Guid.NewGuid()));
                intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
                this.StartActivityForResult(intent, REQUEST_CAMERA);
            });*/
            alert.SetButton2("Gallery", (c, ev) => 
            {
                var intent = new Intent(Intent.ActionPick, MediaStore.Images.Media.ExternalContentUri);
                intent.SetType("image/*");
                this.StartActivityForResult(Intent.CreateChooser(intent, "Select Picture"), SELECT_FILE);
            });
            alert.SetButton3("CANCEL", (c, ev) => { });
            alert.Show();

        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                if (requestCode == SELECT_FILE)
                {
                     //
                }
                /*else if (requestCode == REQUEST_CAMERA)
                {
                    imgView.SetImageURI(data.Data);
                }*/
            }
        }









        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }


}