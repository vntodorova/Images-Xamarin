using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;
using Square.Picasso;
using Android.Net;
using Java.IO;
using Android.Graphics;
using System.IO;
using Android.Provider;

namespace Images
{
    class ListViewAdapter : BaseAdapter<string>
    {
        private List<string> items;
        private Context context;
        public MainActivity.State state;

        public ListViewAdapter(Context context, List<string> items)
        {
            this.items = items;
            this.context = context;
        }
        public override int Count
        {
            get { return items.Count; }
        }   

        public override long GetItemId(int position)
        {
            return position;
        }

        public override string this[int position]
        {
            get { return items[position]; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View element = convertView;

            if(element == null)
            {
                element = LayoutInflater.From(context).Inflate(Resource.Layout.listview_row, null, false);
            }

            ImageView imageView = element.FindViewById<ImageView>(Resource.Id.imageView);
            if(state == MainActivity.State.LocalImages)
            {
                Bitmap imageBitmap = LocalImageManager.ResizeImage(System.IO.File.ReadAllBytes(items[position]), 200, 200);
                imageView.SetImageBitmap(imageBitmap);
            }
            else if(state == MainActivity.State.SearchImages)
            {
                Picasso.With(context)
               .Load(items[position])
               .Resize(300, 300)
               .Placeholder(Resource.Drawable.placeholder)
               .CenterCrop()
               .Into(imageView);
            }
            return element;
        }
    }
}