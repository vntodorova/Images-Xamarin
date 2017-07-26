using System;

using Android.App;
using Android.Content;
using Android.Appwidget;
using Android.Widget;

namespace Images
{
    [BroadcastReceiver(Label = "MyWidget")]
    [IntentFilter(new string[] { "android.appwidget.action.APPWIDGET_UPDATE" })]
    [MetaData("android.appwidget.provider", Resource = "@xml/widget_provider_info")]
    class MyWidget : AppWidgetProvider
    {
    }
}