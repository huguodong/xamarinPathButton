using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PathbuttonLib;
using System.Collections.Generic;
namespace PathButton
{
    [Activity(Label = "PathButton", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity,SatelliteMenu.SateliteClickedListener 
    {
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            SatelliteMenu menu = FindViewById<SatelliteMenu>(Resource.Id.menu);

            List<SatelliteMenuItem> items = new List<SatelliteMenuItem>();
            items.Add(new SatelliteMenuItem(1, Resource.Drawable.ic_1));
            items.Add(new SatelliteMenuItem(2, Resource.Drawable.ic_2));
            items.Add(new SatelliteMenuItem(3, Resource.Drawable.ic_3));
            items.Add(new SatelliteMenuItem(4, Resource.Drawable.ic_4));
            items.Add(new SatelliteMenuItem(5, Resource.Drawable.ic_5));
            items.Add(new SatelliteMenuItem(6, Resource.Drawable.ic_6));
            //        items.add(new SatelliteMenuItem(5, R.drawable.sat_item));
            menu.addItems(items);
            menu.setOnItemClickedListener(this);
         
        }
         

        public void eventOccured(int id)
        {
            Toast.MakeText(this, id+"点击", ToastLength.Short).Show();
        }
    }
}

