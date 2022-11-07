﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using Google.Android.Material.TextView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KotaPalace.Models;
using KotaPalace_Api.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KotaPalace.Activities
{
    [Activity(Label = "SignUp")]
    public class SignUp : Activity
    {
        private string conn = "https://kota-palace-api.herokuapp.com/api";

        private MaterialTextView back_to_signin_text;
        private TextInputEditText InputFirstname;
        private TextInputEditText InputLastname;
        private TextInputEditText InputEmail;
        private TextInputEditText InputPhoneNumber;
        private TextInputEditText InputPassword;
        private TextInputEditText InputConfirmPassword;

        private MaterialButton btn_proceed_signup;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_sign_up);

            //call methods here
            Init();
            GoToSignIn();
            GoToBusinessSignUp();
            
        }

        

        private void Init()
        {
            back_to_signin_text = FindViewById<MaterialTextView>(Resource.Id.back_to_signin_text);

            InputFirstname = FindViewById<TextInputEditText>(Resource.Id.InputRegFirstname);
            InputLastname = FindViewById<TextInputEditText>(Resource.Id.InputRegLastname);
            InputEmail = FindViewById<TextInputEditText>(Resource.Id.InputRegEmail);
            InputPhoneNumber = FindViewById<TextInputEditText>(Resource.Id.InputRegPhoneNumber);
            InputPassword = FindViewById<TextInputEditText>(Resource.Id.InputRegPassword);
            InputConfirmPassword = FindViewById<TextInputEditText>(Resource.Id.InputRegConfirmPassword);

            btn_proceed_signup = FindViewById<MaterialButton>(Resource.Id.btn_proceed_signup);
        }

        private void GoToSignIn()
        {
            back_to_signin_text.Click += (s, e) =>
            {
                StartActivity(new Intent(this, typeof(SignIn)));
                
            };
        }

        private void GoToBusinessSignUp()
        {
            btn_proceed_signup.Click += (s, e) =>
            {
                SignUpUserAsync();

                //StartActivity(new Intent(this, typeof(SignUpBusiness)));
                //OverridePendingTransition(Resource.Animation.Side_in_left, Resource.Animation.Side_out_right);
            };
        }

        private void InputValidations()
        {
            if (string.IsNullOrEmpty(InputFirstname.Text) || string.IsNullOrWhiteSpace(InputFirstname.Text))
            {
                InputFirstname.RequestFocus();
                InputFirstname.Error = "Provide your firstname";
                return;
            }

            if (string.IsNullOrEmpty(InputLastname.Text) || string.IsNullOrWhiteSpace(InputLastname.Text))
            {
                InputLastname.RequestFocus();
                InputLastname.Error = "Provide your lastname";
                return;
            }

            if (string.IsNullOrEmpty(InputEmail.Text) || string.IsNullOrWhiteSpace(InputEmail.Text))
            {
                InputEmail.RequestFocus();
                InputEmail.Error = "Provide your email";
                return;
            }
            
            if (string.IsNullOrEmpty(InputPhoneNumber.Text) || string.IsNullOrWhiteSpace(InputPhoneNumber.Text))
            {
                InputPhoneNumber.RequestFocus();
                InputPhoneNumber.Error = "Provide your email";
                return;
            }

            if (string.IsNullOrEmpty(InputPassword.Text) || string.IsNullOrWhiteSpace(InputPassword.Text))
            {
                InputPassword.RequestFocus();
                InputPassword.Error = "Provide your password";
                return;
            }

            if (string.IsNullOrEmpty(InputConfirmPassword.Text) || string.IsNullOrWhiteSpace(InputConfirmPassword.Text))
            {
                InputConfirmPassword.RequestFocus();
                InputConfirmPassword.Error = "Provide email confirmation";
                return;
            }
        }

        private async void SignUpUserAsync()
        {
            InputValidations();

            UserSignUp user = new UserSignUp()
            {
                Firstname = InputFirstname.Text.Trim(),
                UserType = "Admin",
                Lastname = InputLastname.Text.Trim(),
                Email = InputEmail.Text.Trim(),
                Password = InputPassword.Text.Trim(),
                PhoneNumber = InputPhoneNumber.Text.Trim()
            };


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            HttpContent data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient httpClient = new HttpClient();

            var results = await httpClient.PostAsync($"{API.Url}/account/signup", data);

            if (results.IsSuccessStatusCode)
            {
                var str = await results.Content.ReadAsStringAsync();
                //var response = Newtonsoft.Json.JsonConvert.DeserializeObject<AppUsers>(str);

                if (str != null)
                {
                    SuccessMessage("Your account has been successfully created");

                    //Preferences.Set("id", str);

                    //open sign up business activity
                    StartActivity(new Intent(this, typeof(SignUpBusiness)));
                    OverridePendingTransition(Resource.Animation.Side_in_left, Resource.Animation.Side_out_right);
                }
            }
            else
            {
                var str_r = await results.Content.ReadAsStringAsync();

                ErrorMessage(str_r);
            }
        }

        private void SuccessMessage(string t)
        {
            AndHUD.Shared.ShowSuccess(this, t, MaskType.None, TimeSpan.FromSeconds(3));
        }

        private void ErrorMessage(string t)
        {
            AndHUD.Shared.ShowError(this, t, MaskType.None, TimeSpan.FromSeconds(3));
        }
    }
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