
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.Fragment.App;
using Google.Android.Material.TextView;
using KotaPalace.Models;
using KotaPalace_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;

namespace KotaPalace.Fragments
{
    public class ProfileFragment : Fragment
    {
        private MaterialTextView testTv;

        public ProfileFragment()
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

            View rootView = inflater.Inflate(Resource.Layout.fragment_profile, container, false);

            Init(rootView);
            GetUserDetails();

            return rootView;
        }

        private void Init(View view)
        {
            testTv = view.FindViewById<MaterialTextView>(Resource.Id.testTv);
        }

        private async void GetUserDetails()
        {
            var Id = Preferences.Get("id", null);

            try
            {
                HttpClient httpClient = new HttpClient();
                var response = await httpClient.GetAsync($"{API.Url}/account/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    string str_out = await response.Content.ReadAsStringAsync();
                    var user = Newtonsoft.Json.JsonConvert.DeserializeObject<AppUsers>(str_out);

                    if (user != null)
                    {
                        //DriverNameTextView.Text = $"{user.Name} {user.Surname}";
                        //InputNamesTextView.Text = user.Name;
                        //InputSurnameTextView.Text = user.Surname;
                        //InputPhoneTextView.Text = user.PhoneNumber;
                        //InputEmailTextView.Text = user.Email;
                        //Message($"{user.Firstname}-{user.Lastname}-{user.Email}-{user.PhoneNumber}");

                        testTv.Text = $"{user.Firstname}-{user.Lastname}-{user.Email}-{user.PhoneNumber}";
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Message(ex.Message);
            }
        }

        private void Message(string message)
        {
            AndHUD.Shared.ShowError(Context, message, MaskType.None, TimeSpan.FromSeconds(3));
        }
    }
}