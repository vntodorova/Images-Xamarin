using Android.Graphics;
using Android.Net;
using System;
using System.Collections.Generic;


namespace Images
{
    public class LocalImageManager
    {
        private static LocalImageManager instance;
        public static Java.IO.File currentImageTaken;

        private LocalImageManager() { }
        public static LocalImageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LocalImageManager();
                }
                return instance;
            }
        }

        internal List<string> GetAllImages()
        {
            Java.IO.File[] allFiles = GetImagesDirectory().ListFiles();
            string[] fileStrings = new string[allFiles.Length];

            for (int i = 0; i < allFiles.Length; i++)
            {
                fileStrings[i] = allFiles[i].Path;
            }

            return new List<string>(fileStrings);
        }
        public static Bitmap GetRandomLocalImage()
        {
            List<string> images = LocalImageManager.Instance.GetAllImages();
            Random random = new Random();
            int randomInt = random.Next(0, images.Count);
            Bitmap imageBitmap = ResizeImage(System.IO.File.ReadAllBytes(images[randomInt]), 500, 500);
            return imageBitmap;
        }

        public Java.IO.File GetImagesDirectory()
        {
            Java.IO.File directory = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "ImagesApp");
            if (!directory.Exists())
            {
                directory.Mkdirs();
            }
            return directory;
        }

        public Java.IO.File CreateNewImageFile()
        {
            currentImageTaken = new Java.IO.File(GetImagesDirectory(), string.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            return currentImageTaken;
        }

        public static Bitmap ResizeImage(byte[] imageData, float width, float height)
        {
            BitmapFactory.Options options = new BitmapFactory.Options();
            options.InPurgeable = true; // inPurgeable is used to free up memory while required
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length, options);

            float newHeight = 0;
            float newWidth = 0;

            var originalHeight = originalImage.Height;
            var originalWidth = originalImage.Width;

            if (originalHeight > originalWidth)
            {
                newHeight = height;
                float ratio = originalHeight / height;
                newWidth = originalWidth / ratio;
            }
            else
            {
                newWidth = width;
                float ratio = originalWidth / width;
                newHeight = originalHeight / ratio;
            }

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, true);
            originalImage.Recycle();
            return resizedImage;
        }
    }
}