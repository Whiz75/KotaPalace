﻿using Android.Content;
using Android.OS;
using Android.Runtime;
using AndroidHUD;
using AndroidX.AppCompat.App;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using KotaPalace.Dialogs;
using KotaPalace.Models;
using KotaPalace_Api.Models;
using System;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;

namespace KotaPalace.Activities
{
    [Android.App.Activity(Label = "SignIn", Theme = "@style/AppTheme", MainLauncher = false)]
    public class SignIn : AppCompatActivity
    {
        private string conn = "https://kota-palace-api.herokuapp.com/api";

        private TextInputEditText InputLoginEmail;
        private TextInputEditText InputLoginPassword;

        private MaterialTextView TxtForgotPassword;

        private MaterialButton btn_signin;
        private MaterialTextView go_to_signup_text;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_sign_in);

            //call methods here
            Init();
            GoToSignuUp();
            GoToMainActivity();
            ResetPassword();
        }

        private void Init()
        {
            InputLoginEmail = FindViewById<TextInputEditText>(Resource.Id.InputLoginEmail);
            InputLoginPassword = FindViewById<TextInputEditText>(Resource.Id.InputLoginPassword);

            TxtForgotPassword = FindViewById<MaterialTextView>(Resource.Id.TxtForgotPassword);

            btn_signin = FindViewById<MaterialButton>(Resource.Id.btn_signin);
            go_to_signup_text = FindViewById<MaterialTextView>(Resource.Id.go_to_signup_text);
        }

        private void GoToSignuUp()
        {
            go_to_signup_text.Click += (s, e) =>
            {
                StartActivity(new Intent(this,typeof(SignUp)));
            };
        }

        private void ResetPassword()
        {
            TxtForgotPassword.Click += (s, e) =>
            {
                ForgotPasswordDialogFragment fragment = new ForgotPasswordDialogFragment();
                fragment.Show(SupportFragmentManager.BeginTransaction(), "");
            };
        }

        private void GoToMainActivity()
        {
            btn_signin.Click += (s, e) =>
            {
                Login();
            };
        }

        private void inputValidations()
        {
            InputLoginEmail.Text = "y@yahoo.com";
            InputLoginPassword.Text = "1234567";

            if (string.IsNullOrEmpty(InputLoginEmail.Text) && string.IsNullOrWhiteSpace(InputLoginEmail.Text))
            {
                InputLoginEmail.RequestFocus();
                InputLoginEmail.Error = "provide your email";
                return;
            }
            else if (string.IsNullOrEmpty(InputLoginPassword.Text) && string.IsNullOrWhiteSpace(InputLoginPassword.Text))
            {
                InputLoginPassword.RequestFocus();
                InputLoginPassword.Error = "provide a password";
                return;
            }
        }

        private async void Login()
        {
            inputValidations();
           
            UserLogin login = new UserLogin()
            {
                //Email = InputLoginEmail.Text.Trim(),
                //Password = InputLoginPassword.Text.Trim()
                Email = "y@yahoo.com",
                Password = "1234567"

            };

            try
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(login);
                HttpContent data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpClient httpClient = new HttpClient();

                var results = await httpClient.PostAsync($"{API.Url}/account/login", data);

                if (results.IsSuccessStatusCode)
                {
                    string str_out = await results.Content.ReadAsStringAsync();
                    var user = Newtonsoft.Json.JsonConvert.DeserializeObject<AppUsers>(str_out);
                    Preferences.Set("Id", user.Id);

                    StartActivity(new Intent(this, typeof(MainActivity)));
                    OverridePendingTransition(Resource.Animation.Side_in_left, Resource.Animation.Side_out_right);
                }
                else
                {
                    string str_out = await results.Content.ReadAsStringAsync();

                    Message(str_out);
                }
            }
            catch (HttpRequestException ex)
            {
                Message(ex.Message);
            }
        }

        private void Message(string t)
        {
            AndHUD.Shared.ShowError(this, t,MaskType.None,TimeSpan.FromSeconds(3));
        }

        
    }

    public class UserLogin
    {
        public UserLogin()
        {
        }

        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserSignUp
    {
        public UserSignUp()
        {
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string PhoneNumber { get; set; }
        public string UserType { get; set; }
    }
}