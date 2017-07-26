using Android.App;
using Android.OS;
using Android.Provider;
using Android.Widget;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Content;
using Android.Runtime;
using System.Json;
using System;
using Android.Net;


namespace Images
{
    [Activity(Label = "Images", MainLauncher = true, Icon = "@drawable/images")]
    public class MainActivity : Activity
    {
        public enum State
        {
            LocalImages = 0,
            SearchImages = 1
        }

        private List<string> images = new List<string>();
        private string searchedString;
        private ListViewAdapter adapter;
        private EditText editText;
        private GridView gridView;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);
            editText = FindViewById<EditText>(Resource.Id.searchEditText);
            FindViewById<Button>(Resource.Id.searchButton).Click += SearchButtonClick;
            FindViewById<Button>(Resource.Id.cameraButton).Click += TakeAPicture;
            gridView = FindViewById<GridView>(Resource.Id.gridView);

            StartWidgetBroadcastWithInterval(60000);
            SetupGridView();
        }

        protected override void OnRestoreInstanceState(Bundle savedInstanceState)
        {
            base.OnRestoreInstanceState(savedInstanceState);
            if (savedInstanceState != null)
            {
                if (savedInstanceState.GetInt("State") == 0)
                {
                    adapter.state = State.LocalImages;
                }
                else
                {
                    adapter.state = State.SearchImages;
                    searchedString = savedInstanceState.GetString("SearchedString");
                    LoadSearchResults();
                }
                adapter.NotifyDataSetChanged();
            }
        }

        private void SetupGridView()
        {
            adapter = new ListViewAdapter(this, images);
            if (adapter.state == State.LocalImages)
            {
                SetLocalImagesAsSource();
            } else
            {
                LoadSearchResults();
            }
            gridView.Adapter = adapter;
        }

        private void SetLocalImagesAsSource()
        {
            images.Clear();
            images.AddRange(LocalImageManager.Instance.GetAllImages());
        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            if (adapter.state == State.LocalImages)
            {
                outState.PutInt("State", 0);
            }
            else
            {
                outState.PutInt("State", 1);
                outState.PutString("SearchedString", editText.Text);
            }
        }

        private void StartWidgetBroadcastWithInterval(int millis)
        {
            var intent = new Intent(this, typeof(RepeatingAlarm));
            var source = PendingIntent.GetBroadcast(this, 0, intent, 0);
            var am = (AlarmManager)GetSystemService(AlarmService);
            am.SetRepeating(AlarmType.ElapsedRealtimeWakeup, 0, millis, source);
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            if (!IsCameraAvailable())
            {
                return;
            }
            adapter.state = State.LocalImages;
            images.Clear();
            images.AddRange(LocalImageManager.Instance.GetAllImages());
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            Java.IO.File file = LocalImageManager.Instance.CreateNewImageFile();
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(file));
            StartActivityForResult(intent, 0);
        }

        private void SearchButtonClick(object sender, EventArgs eventArgs)
        {
            searchedString = editText.Text;
            LoadSearchResults();
        }

        private async void LoadSearchResults()
        {
            if(searchedString == null)
            {
                return;
            }
            adapter.state = State.SearchImages;
            images.Clear();
            JsonValue json = await APIManager.Instance.FetchImagesAsyn(searchedString);
            images.AddRange(APIManager.Instance.ParseJson(json));
            adapter.NotifyDataSetChanged();
        }

        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            images.Add(LocalImageManager.currentImageTaken.Path);
            adapter.NotifyDataSetChanged();
        }

        private bool IsCameraAvailable()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }


    }
}

