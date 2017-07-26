using System.Collections.Generic;
using Android.Content;
using Android.Widget;
using Android.Appwidget;

namespace Images
{
    [BroadcastReceiver]
    class RepeatingAlarm : BroadcastReceiver
    {
        private List<string> images;
        public override void OnReceive(Context context, Intent intent)
        {
            UpdateWidget(context);
        }

        private void UpdateWidget(Context context)
        {
            RemoteViews updateViews = CreateWidgetView(context);
            ComponentName statusWidget = new ComponentName(context, Java.Lang.Class.FromType(typeof(MyWidget)).Name);
            AppWidgetManager manager = AppWidgetManager.GetInstance(context);
            manager.UpdateAppWidget(statusWidget, updateViews);
        }

        private RemoteViews CreateWidgetView(Context context)
        {
            RemoteViews views = new RemoteViews(context.PackageName, Resource.Layout.widget);
            views.SetImageViewBitmap(Resource.Id.imageView, LocalImageManager.GetRandomLocalImage());
            return views;
        }

    }
}