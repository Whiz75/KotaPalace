using Android.Content;
using Android.Gms.Extensions;
using Android.Gms.Location;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Gms.Tasks;
using Android.Locations;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
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

        private TextInputEditText txt_address;

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

        public static BusinessAddressDialogFragment NewInstance(Bundle bundle)
        {
            BusinessAddressDialogFragment fragment = new BusinessAddressDialogFragment();
            fragment.Arguments = bundle;
            return fragment;
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
            txt_address = view.FindViewById<TextInputEditText>(Resource.Id.txt_address);

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

            //GetLastLocation();
            CheckGps();
        }

        private async void GetLocation()
        {
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.High);
                var location = await Geolocation.GetLocationAsync(request);

                if (location != null)
                {
                    LatLng lang = new LatLng(location.Latitude, location.Longitude);
                    googleMap.AnimateCamera(CameraUpdateFactory.NewLatLngZoom(lang, 17));

                    MarkerOptions Options = new MarkerOptions();
                    Options.SetPosition(lang);
                    Options.SetTitle("My location");
                    Options.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.shop));

                    Options.Anchor((float)0.5, (float)0.5);
                    googleMap.AddMarker(Options);

                    //get address here
                    var addresses = await ReverseGeocodeCurrentLocation(location.Latitude, location.Longitude);
                    string address = addresses.GetAddressLine(0);
                    txt_address.Text = $"{address}";

                    //pass address to activity
                    GetAddress(address);
                }
            }
            catch (Exception ex)
            {

                DisplayMessage(ex.Message);
            }
        }

        async Task<Address> ReverseGeocodeCurrentLocation(double lat, double lon)
        {
            Geocoder geocoder = new Geocoder(context);
            IList<Address> addressList = await geocoder.GetFromLocationAsync(lat, lon, 5);

            Address address = addressList.FirstOrDefault();
            return address;
        }

        private void CheckGps()
        {
            LocationManager locationManager = (LocationManager)context.GetSystemService(Context.LocationService);

            //LocationManager mgr = (LocationManager)getActivity().getSystemService(Context.LOCATION_SERVICE);
            bool gps_enable = false;

            gps_enable = locationManager.IsProviderEnabled(LocationManager.GpsProvider);

            if (!gps_enable)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(context);
                builder.SetTitle("Confirm");
                builder.SetMessage("Please enable your location to continue");
                builder.SetNegativeButton("Cancel", delegate
                {
                    builder.Dispose();
                });
                builder.SetPositiveButton("Settings", delegate
                {
                    StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                });
                builder.Show();
            }
            else
            {
                GetLocation();
            }
        }

        private void GetAddress(string a)
        {
            ConfirmAddressBtn.Click += (s, e) =>
            {
                ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Context);
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutString("address", a);
                editor.Apply();

                BusinessAddressDialogFragment fragment = (BusinessAddressDialogFragment)FragmentManager.FindFragmentByTag("Address");
                if (fragment != null)
                {
                    fragment.Dismiss();
                }
            };
        }

        private void DisplayMessage(string m)
        {
            AndHUD.Shared.ShowError(context, m, MaskType.None, TimeSpan.FromSeconds(3));
        }

    }
}