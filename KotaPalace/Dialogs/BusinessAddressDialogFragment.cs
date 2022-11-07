using Android.Content;
using Android.Gms.Extensions;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Tasks;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.Fragment.App;
using Google.Android.Material.Button;
using Java.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static Android.Icu.Text.Transliterator;
using Context = Android.Content.Context;
using Task = Android.Gms.Tasks.Task;

namespace KotaPalace.Dialogs
{
    public class BusinessAddressDialogFragment : DialogFragment, IOnMapReadyCallback
    {
        private Context context;

        private ImageView cancel_iv;

        private MaterialButton ConfirmAddressBtn;
        private SupportMapFragment mapFrag;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        private LatLng Lng;

        public BusinessAddressDialogFragment(LatLng lng)
        {
            Lng = lng;
        }

        public BusinessAddressDialogFragment()
        {
        }

        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            Dialog.SetCanceledOnTouchOutside(false);
        }

        

        public override  View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.business_address_dialog, container, false);

            Init(view);
           // CheckAndRequestLocationPermission();

            return view;
        }

        private void Init(View view)
        {
            context = view.Context;

            cancel_iv = view.FindViewById<ImageView>(Resource.Id.cancel_iv);
            ConfirmAddressBtn = view.FindViewById<MaterialButton>(Resource.Id.ConfirmAddressBtn);

            mapFrag = ChildFragmentManager.FindFragmentById(Resource.Id.fragMap).JavaCast<SupportMapFragment>();
            mapFrag.GetMapAsync(this);

            cancel_iv.Click += (s, e) =>
            {
                Dismiss();
            };
        }

        private GoogleMap googleMap;
        public void OnMapReady(GoogleMap googleMap)
        {
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("en-US");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;

            this.googleMap = googleMap;
            
            this.googleMap.UiSettings.ZoomControlsEnabled = true;

            //map styling 
            var stream = Resources.Assets.Open("map_style.json");
            string file;
            using (var reader = new System.IO.StreamReader(stream))
            {
                file = reader.ReadToEnd();
            }
            MapStyleOptions mapStyleOptions = new MapStyleOptions(file);

            this.googleMap.SetMapStyle(mapStyleOptions);
            this.googleMap.CameraChange += GoogleMap_CameraChange;

            //GetLastLocation();
            GetLocation();
        }

        private void GoogleMap_CameraChange(object sender, GoogleMap.CameraChangeEventArgs e)
        {
            var lat = e.Position.Target.Latitude;
            var lon = e.Position.Target.Longitude;
            //Console.WriteLine(lat.ToString() + "U+002C" + lon.ToString());
            //AndHUD.Shared.ShowSuccess(context,$"{lat}-{lon}", MaskType.Clear, TimeSpan.FromSeconds(3));

        }

        private async void GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    DisplayMessage($"{location.Latitude} {location.Longitude}");
                }
            }
            catch (Exception ex)
            {

                
            }
        }

        private void DisplayMessage(string m)
        {
            AndHUD.Shared.ShowError(context, m, MaskType.None, TimeSpan.FromSeconds(3));
        }

    }
}