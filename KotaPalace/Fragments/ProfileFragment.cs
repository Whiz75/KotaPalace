
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.Fragment.App;
using Google.Android.Material.Button;
using Google.Android.Material.MaterialSwitch;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using KotaPalace.Models;
using KotaPalace_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KotaPalace.Fragments
{
    public class ProfileFragment : Fragment
    {
        //private textin
        private MaterialSwitch OutputStatus;

        private TextInputEditText OutputName;
        private TextInputEditText OutputLastname;
        private TextInputEditText OutputEmail;
        private TextInputEditText OutputPhoneNumber;
        private TextInputEditText OutputUserType;

        private MaterialButton BtnUpdateProfile;

        string Id = Preferences.Get("Id", null);

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
            GetBusinessAsync();

            return rootView;
        }

        private void Init(View view)
        {

            OutputName = view.FindViewById<TextInputEditText>(Resource.Id.OutputName);
            OutputLastname = view.FindViewById<TextInputEditText>(Resource.Id.OutputLastname);
            OutputEmail = view.FindViewById<TextInputEditText>(Resource.Id.OutputEmail);
            OutputPhoneNumber = view.FindViewById<TextInputEditText>(Resource.Id.OutputPhoneNumber);
            OutputUserType = view.FindViewById<TextInputEditText>(Resource.Id.OutputUserType);

            BtnUpdateProfile = view.FindViewById<MaterialButton>(Resource.Id.BtnUpdateProfile);

            BtnUpdateProfile.Click += (s, e) =>
            {
                UpdateUserDetails();
            };
        }

        private async void GetUserDetails()
        {
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
                        OutputName.Text = user.Firstname;
                        OutputLastname.Text = user.Lastname;
                        OutputEmail.Text = user.Email;
                        OutputPhoneNumber.Text = user.PhoneNumber;
                        OutputUserType.Text = user.UserType;
                    }
                    else
                    {
                        Message("User records not found!!!");
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Message(ex.Message);
            }
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
                        
                    }
                   
                }
            }catch(HttpRequestException ex)
            {
                Message(ex.Message);
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