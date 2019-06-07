using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using GalaSoft.MvvmLight.Views;
using ImageEditor_v3.ViewModels;
using GalaSoft.MvvmLight.Helpers;
using Android.Content;
using Android;
using Android.Support.V4.Content;
using Android.Support.V4.App;
using Android.Content.PM;

namespace ImageEditor_v3
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : ActivityBase
    {

        //Deklarace proměnných
        SeekBar exposure;
        SeekBar contrast;
        SeekBar saturation;

        ImageView imgView;

        Button selectImg;
        Button savetImg;

        private static readonly int SELECT_FILE = 1;

        MainViewModel vm;

        Android.App.AlertDialog.Builder dialog;
        Android.App.AlertDialog alert;
        //


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            vm = new MainViewModel(this);
            vm.IsSelected = false;

            //přidělení activity_main.axml tool-objektů do proměnných 
            exposure = FindViewById<SeekBar>(Resource.Id.exposure);
            exposure.ProgressChanged += ExposureChanged;

            contrast = FindViewById<SeekBar>(Resource.Id.contrast);
            contrast.ProgressChanged += ContrastChanged;

            saturation = FindViewById<SeekBar>(Resource.Id.saturation);
            saturation.ProgressChanged += SaturatinChanged;

            selectImg = FindViewById<Button>(Resource.Id.selectImage);

            savetImg = FindViewById<Button>(Resource.Id.saveImage);

            imgView = FindViewById<ImageView>(Resource.Id.imageViewer);

            //nastavení bindingu pro ImageView, která bude bindovat property EditImageBtm z MainViewModelu
            this.SetBinding(() => vm.EditImageBtm, () => imgView.Resources);

            //nastavení commandů
            selectImg.SetCommand("Click", vm.ImageCommand);
            savetImg.SetCommand("Click", vm.SaveImageCommand);

            //vytvoření dialogu, připraveného pro zobrazení, pokud není načtena fotka pro editaci
            dialog = new Android.App.AlertDialog.Builder(this);
            alert = dialog.Create();
            alert.SetTitle("Select Image");
            alert.SetMessage("image not selected!!");
            alert.SetButton3("OK", (c, ev) => { });



        }


        //Metody, které kontrolují správné bindování fotky z property EditImageBtm do ImageView.
        protected override void OnResume()
        {
            base.OnResume();
            this.vm.PropertyChanged += Vm_PropertyChanged;;

            if(this.vm.EditImageBtm != null)
            {
                imgView.SetImageBitmap(vm.EditImageBtm);
            }


        }
        void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == nameof(vm.EditImageBtm))
            {
                imgView.SetImageBitmap(vm.EditImageBtm);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();
            this.vm.PropertyChanged -= Vm_PropertyChanged;
        }




        //Metoda pro změnu světelnosti, pokud je načtena fotka pro editaci, zavolá se metoda v MainViewModel, do které se předá parametr hodnota seekbaru, pokud není vybrána fotka
        //zobrazí se alert a zresetuje se poloha seekbaru
        private void ExposureChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (vm.IsSelected)
            {
                float value = (e.Progress - 100);

                vm.ExposureChanged(value);
            }
            else
            {
                alert.Show();
                ResetSlider();
            }

        }

        //Metoda pro změnu kontrastu, pokud je načtena fotka pro editaci, zavolá se metoda v MainViewModel, do které se předá parametr hodnota seekbaru, pokud není vybrána fotka
        //zobrazí se alert a zresetuje se poloha seekbaru
        private void ContrastChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (vm.IsSelected)
            {
                float value = (e.Progress - 100);

                vm.ContrastChanged(value);
            }
            else
            {
                alert.Show();
                ResetSlider();

            }

        }

        //Metoda pro změnu saturace, pokud je načtena fotka pro editaci, zavolá se metoda v MainViewModel, do které se předá parametr hodnota seekbaru, pokud není vybrána fotka
        //zobrazí se alert a zresetuje se poloha seekbaru
        private void SaturatinChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            if (vm.IsSelected)
            {
                float value = (e.Progress * 0.01f);

                vm.SaturationChanged(value);
            }
            else
            {
                alert.Show();
                ResetSlider();

            }
        }

        //Metoda pro zresetování seekbarů do původní poozice (prostředek)
        private void ResetSlider()
        {
            exposure.Progress = 100;
            contrast.Progress = 100;
            saturation.Progress = 100;
        }

        //po dokončení intentu pro výber fotky z galerie se nastaví proměnná, která říká, že je fotka vybrán, z fotky se udělá bitmapa a uloží se do properties v MainViewModel.
        //Vyresetuje se poloha seekbarů.
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);


            if (resultCode == Result.Ok)
            {
                if (requestCode == SELECT_FILE)
                {
                    vm.IsSelected = true;
                    vm.EditImageBtm = Android.Provider.MediaStore.Images.Media.GetBitmap(this.ContentResolver, data.Data);
                    vm.ImageBtm = Android.Provider.MediaStore.Images.Media.GetBitmap(this.ContentResolver, data.Data);

                    ResetSlider();
                }
            }
        }











        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }


}