using System;
using Android.App;
using Android.Graphics;

namespace ImageEditor_v3.Models
{
    public class Model
    {
        //deklarace proměnných
        ColorMatrix clrMatrix;
        ColorMatrixColorFilter imgMtrxFilter;
        Paint paint;
        Canvas canvas;
        Bitmap newBtm;

        public Model()
        {
        }

        //Metoda pro úpravu jasu, nejprve se vytvoří nová prázdná bitmapa podle výšky a šířky původní bitmapy, dále se vytvoří matice, reprezentující barvy a jejich scaling factor(1), poslední
        //položka každého řádku matice představuje jas obrázku, do kterého nahrajeme získanou hodnotu. Vytvoříme filter, do kterého nahrajem martici, barvu, do které nahrajem filtr, do canvasu uložíme
        //novou bitmapu a aplikuejem na ní novou barvu
        public Bitmap ExposureChanged(Bitmap oldBtm, float value)
        {
            newBtm = Bitmap.CreateBitmap(oldBtm.Width, oldBtm.Height, oldBtm.GetConfig());

            clrMatrix = new ColorMatrix(new float[] {
                1, 0, 0, 0, value, //red
                0, 1, 0, 0, value, //green
                0, 0, 1, 0, value, //blue
                0, 0, 0, 1, 0 }); //alpha
            imgMtrxFilter = new ColorMatrixColorFilter(clrMatrix);
            paint = new Paint();
            paint.SetColorFilter(imgMtrxFilter);
            canvas = new Canvas(newBtm);

            canvas.DrawBitmap(oldBtm, new Matrix(), paint);

            return newBtm;
        }

        //Metoda pro úpravu kontrastu, nejprve se vytvoří nová prázdná bitmapa podle výšky a šířky původní bitmapy, dále se vytvoří matice, reprezentující barvy a jejich scaling factor,
        //kontrast je kombinace jasu, vypočítaného podle vzorce a scale faktrou, vypočítaného podle vzorce.
        //Vytvoříme filter, do kterého nahrajem martici, barvu, do které nahrajem filtr, do canvasu uložíme
        //novou bitmapu a aplikuejem na ní novou barvu
        public Bitmap ContrastChanged(Bitmap oldBtm, float value)
        {
            newBtm = Bitmap.CreateBitmap(oldBtm.Width, oldBtm.Height, oldBtm.GetConfig());

            float input = value / 100;
            float scale = input + 1f;
            float contrast = (-0.5f * scale + 0.5f) * 255f;

            clrMatrix = new ColorMatrix(new float[] {
                scale, 0, 0, 0, contrast,
                0, scale, 0, 0, contrast,
                0, 0, scale, 0, contrast,
                0, 0, 0, 1, 0 });
            imgMtrxFilter = new ColorMatrixColorFilter(clrMatrix);
            paint = new Paint();
            paint.SetColorFilter(imgMtrxFilter);
            canvas = new Canvas(newBtm);

            canvas.DrawBitmap(oldBtm, new Matrix(), paint);
            return newBtm;
        }

        //Metoda pro změnu saturace. Zde nemusíme vytvářet matici, ale třída ColorMatrix má vlastní meotdu pro změnu saturace, do které se nahraje pouze hodnota.
        public Bitmap SaturationChanged(Bitmap oldBtm, float value)
        {
            newBtm = Bitmap.CreateBitmap(oldBtm.Width, oldBtm.Height, oldBtm.GetConfig());

            clrMatrix = new ColorMatrix();
            clrMatrix.SetSaturation(value);

            imgMtrxFilter = new ColorMatrixColorFilter(clrMatrix);
            paint = new Paint();
            paint.SetColorFilter(imgMtrxFilter);
            canvas = new Canvas(newBtm);

            canvas.DrawBitmap(oldBtm, new Matrix(), paint);
            return newBtm;
        }
        //Meotda pro uložení obrázku, upravený obrázek bude mít název času, ve kterém byl uložen a bude uložen do galerie/camera
        //Ukládá, ale v galerii neukazuje
        public bool SaveImage(Bitmap btm)
        {
            try
            {
                string myDate = DateTime.Now.TimeOfDay.ToString() + ".jpg";
                using (var os = new System.IO.FileStream(Android.OS.Environment.ExternalStorageDirectory + "/DCIM/Camera/" + myDate, System.IO.FileMode.CreateNew))
                {
                    btm.Compress(Bitmap.CompressFormat.Jpeg, 95, os);
                    Console.WriteLine("image saved");
                    os.Close();
                }

               
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

    }
}
