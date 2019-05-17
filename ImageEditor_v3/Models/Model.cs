﻿using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;

namespace ImageEditor_v3.Models
{
    public class Model
    {
        ColorMatrix clrMatrix;
        ColorMatrixColorFilter imgMtrxFilter;
        Paint paint;
        Canvas canvas;
        Bitmap newBtm;

        public Model()
        {
        }

        /*public Bitmap CreateBitmap(int requestCode, Result resultCode, Intent data)
        {
            var inputStream = ContentResolver.OpenInputStream(data.Data);
            Bitmap bitmap = BitmapFactory.DecodeStream(ContentResolver.OpenInputStream(data.Data));
            return bitmap;
        }*/

        public Bitmap ExposureChanged(Bitmap oldBtm, float value)
        {
            newBtm = Bitmap.CreateBitmap(oldBtm.Width, oldBtm.Height, oldBtm.GetConfig());

            clrMatrix = new ColorMatrix(new float[] {
                1, 0, 0, 0, value,
                0, 1, 0, 0, value,
                0, 0, 1, 0, value,
                0, 0, 0, 1, 0 });
            imgMtrxFilter = new ColorMatrixColorFilter(clrMatrix);
            paint = new Paint();
            paint.SetColorFilter(imgMtrxFilter);
            canvas = new Canvas(newBtm);

            canvas.DrawBitmap(oldBtm, new Matrix(), paint);

            return newBtm;
        }

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

    }
}
