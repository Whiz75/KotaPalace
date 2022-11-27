
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using FFImageLoading;
using Google.Android.Material.Button;
using Google.Android.Material.MaterialSwitch;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using KotaPalace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KotaPalace.Fragments
{
    public class BusinessProfileFragment : Fragment
    {
        //private textin
        private AppCompatImageView ImgBusinessLogo;

        private AppCompatTextView OutputStatus;
        private AppCompatTextView OutputBusinessOwner;

        private TextInputEditText OutputBusinessName;
        private TextInputEditText OutputBusinessDesc;
        private TextInputEditText OutputBusinessAddress;
        private TextInputEditText OutputBusinessPhoneNumber;
        private TextInputEditText OutputBusinessEmail;

        private MaterialButton btn_mf_open;
        private MaterialButton btn_mf_close;
        private MaterialButton btn_sat_open;
        private MaterialButton btn_sat_close;
        private MaterialButton btn_sun_open;
        private MaterialButton btn_sun_close;

        private MaterialButton BtnUpdateProfile;

        string Id = Preferences.Get("Id", null);

        public BusinessProfileFragment()
        {
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View rootView = inflater.Inflate(Resource.Layout.fragment_business_profile, container, false);

            Init(rootView);
            GetBusinessAsync();

            return rootView;
        }

        private void Init(View view)
        {
            ImgBusinessLogo = view.FindViewById<AppCompatImageView>(Resource.Id.ImgBusinessLogo);

            OutputBusinessOwner = view.FindViewById<AppCompatTextView>(Resource.Id.OutputBusinessOwner);
            OutputStatus = view.FindViewById<AppCompatTextView>(Resource.Id.OutputBusinessStatus);
            OutputBusinessName = view.FindViewById<TextInputEditText>(Resource.Id.OutputBusinessName);
            OutputBusinessDesc = view.FindViewById<TextInputEditText>(Resource.Id.OutputBusinessDesc);
            OutputBusinessAddress = view.FindViewById<TextInputEditText>(Resource.Id.OutputBusinessAddress);
            OutputBusinessEmail = view.FindViewById<TextInputEditText>(Resource.Id.OutputBusinessEmail);
            OutputBusinessPhoneNumber = view.FindViewById<TextInputEditText>(Resource.Id.OutputBusinessPhoneNumber);

            btn_mf_open = view.FindViewById<MaterialButton>(Resource.Id.btn_mf_open);
            btn_mf_close = view.FindViewById<MaterialButton>(Resource.Id.btn_mf_close);
            btn_sat_open = view.FindViewById<MaterialButton>(Resource.Id.btn_sat_open);
            btn_sat_close = view.FindViewById<MaterialButton>(Resource.Id.btn_sat_close);
            btn_sun_open = view.FindViewById<MaterialButton>(Resource.Id.btn_sun_open);
            btn_sun_close = view.FindViewById<MaterialButton>(Resource.Id.btn_sun_close);

            //BtnUpdateProfile = view.FindViewById<MaterialButton>(Resource.Id.BtnUpdateProfile);

            //BtnUpdateProfile.Click += (s, e) =>
            //{
            //    UpdateUserDetails();
            //};
        }

        private async void GetBusinessAsync()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{API.Url}/businesses/specific/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    string str_out = await response.Content.ReadAsStringAsync();
                    var business = Newtonsoft.Json.JsonConvert.DeserializeObject<Business>(str_out);

                    if(business != null)
                    {
                        if(business.OnlineStatus == "Online")
                        {
                            OutputStatus.Text = business.OnlineStatus;
                            OutputStatus.SetTextColor(Android.Graphics.Color.ParseColor("#4CBB17"));
                        }
                        else
                        {
                            OutputStatus.Text = business.OnlineStatus;
                            OutputStatus.SetTextColor(Android.Graphics.Color.ParseColor("#e6d9534f"));
                        }

                        OutputBusinessName.Text = business.BusinessName;
                        OutputBusinessAddress.Text = business.BusinessAddress;
                        OutputBusinessDesc.Text = business.BusinessDescription;
                        OutputBusinessEmail.Text = business.BusinessEmail;
                        OutputBusinessPhoneNumber.Text = business.BusinessPhoneNumber;

                        btn_mf_open.Text = business.BusinessMFOpen;
                        btn_mf_close.Text = business.BusinessMFClose;
                        btn_sat_open.Text = business.BusinessSatOpen;
                        btn_sat_close.Text = business.BusinessSatClose;
                        btn_sun_open.Text= business.BusinessSunOpen;
                        btn_sun_close.Text = business.BusinessSunClose;

                        try
                        {
                            if (business.ImgUrl != null)
                            {
                                ImageService
                                    .Instance
                                    .LoadUrl(business.ImgUrl)
                                    .Into(ImgBusinessLogo);
                            }
                        }
                        catch (Exception ex)
                        {
                            Message(ex.Message);
                        }

                        //get owner details
                        GetOwnerName(business.OwnerId, OutputBusinessOwner);
                    }
                   
                }
            }catch(HttpRequestException ex)
            {
                Message(ex.Message);
            }
        }

        private async void GetOwnerName(string id, AppCompatTextView name)
        {
            HttpClient httpClient = new HttpClient();
            var response = await httpClient.GetAsync($"{API.Url}/account/{id}");

            if (response.IsSuccessStatusCode)
            {
                string str_out = await response.Content.ReadAsStringAsync();
                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<AppUsers>(str_out);

                if (user != null)
                {
                    name.Text = $"{user.Firstname} {user.Lastname}";
                }
            }
        }

        private void UpdateUserDetails()
        {
            
        }

        private void Message(string message)
        {
            AndHUD.Shared.ShowError(Context, message, MaskType.None, TimeSpan.FromSeconds(3));
        }
    }
}