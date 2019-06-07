using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using GalaSoft.MvvmLight.Command;
using ImageEditor_v3.Models;
using MvvmCross.ViewModels;
using System.IO;
using Android.Provider;
using Android.Widget;

namespace ImageEditor_v3.ViewModels
{
    public class MainViewModel : MvxViewModel
    {
        //deklarace proměnných
        //Data
        private Bitmap editImageBtm;
        private Bitmap imageBtm;
        private bool isSelected;
        private Model md;
        private Activity act;

        Android.App.AlertDialog.Builder dialog;
        Android.App.AlertDialog alert;

        private static readonly int SELECT_FILE = 1;


        //Commandy
        public RelayCommand ImageCommand { get; private set; }
        public RelayCommand SaveImageCommand { get; private set; }


        //v konstruktoru se přiřadí hodnoty proměnným a předá se sem aktuální activita, která nám umožňuje z této třídy zobrazovat alerty do aplikace uživateli.
        public MainViewModel(Activity act)
        {
            this.act = act;

            isSelected = false;

            ImageCommand = new RelayCommand(SelectImage);
            SaveImageCommand = new RelayCommand(SaveImage);

            md = new Model();
        }



        //Property pokud je fotka vybrána
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                RaisePropertyChanged(() => IsSelected);
            }
        }

        //property pro upravenou bitmapu
        public Bitmap EditImageBtm
        {
            get { return editImageBtm; }
            set
            {
                editImageBtm = value;
                RaisePropertyChanged(() =>EditImageBtm);
            }
        }

        //property pro původní bitmapu, která se předává pokaždé je zavolána metoda pro úoravu, aby se předešlo stackování úprav na sebe
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
        //Po stisknutí tlačítka se uživateli zobrazí alert, odkud chce fotku vybrat (budoucí možnost pro výběr i z foťáku, nepodařilo se mi rozchodit), pokud je vybraná galerie,
        //spustí se intent pro výběr obrázku z galerie, ta se otevře a uživatel má možnost vybrat fotku.
        public void SelectImage()
        {
            dialog = new Android.App.AlertDialog.Builder(this.act);
            alert = dialog.Create();
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


        //metody, které volají metody na úpravu fotek z modelu
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

        //metoda, která porovná zda je fotka vybraná nebo ne, pokud ne vyhodí alert, pokud ano, zavolá metodu v modelu pro úoravu obrázku, která pokud vrátí uspěšné uložení, tak se zobrazí v aplikaci
        //toast, že fotka byla uložena
        public void SaveImage()
        {
            if (IsSelected)
            {
                bool saved = md.SaveImage(EditImageBtm);
                if (saved)
                {
                    var toast = Toast.MakeText(act, "Image saved", ToastLength.Short);
                    toast.Show();
                }
            }
            else
            {
                dialog = new Android.App.AlertDialog.Builder(this.act);
                alert = dialog.Create();
                alert.SetTitle("Select Image");
                alert.SetMessage("image not selected!!");
                alert.SetButton3("OK", (c, ev) => { });
                alert.Show();
            }

        }

    }
}
