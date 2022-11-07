using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text.Format;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Google.Android.Material.Button;
using Google.Android.Material.TextField;
using KotaPalace.Dialogs;
using KotaPalace.Models;
using KotaPalace_Api.Models;
using Org.Apache.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KotaPalace.Activities
{
    [Android.App.Activity(Label = "SignUpBusiness")]
    public class SignUpBusiness : AppCompatActivity
    {
        private string conn = "https://kota-palace-api.herokuapp.com/api";

        private TextInputEditText InputBusinessName;
        private TextInputEditText InputBusinessPhone;
        private TextInputEditText InputBusinessDescription;
        private TextInputEditText InputBusinessEmail;
        private MaterialAutoCompleteTextView InputBusinessMFOpen;
        private MaterialAutoCompleteTextView InputBusinessMFClose;
        private MaterialAutoCompleteTextView InputBusinessSatOpen;
        private MaterialAutoCompleteTextView InputBusinessSatClose;
        private MaterialAutoCompleteTextView InputBusinessSunOpen;
        private MaterialAutoCompleteTextView InputBusinessSunClose;

        private MaterialButton InputBusinessAddress;
        private MaterialButton BtnUpdateBusinessProfile;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_sign_up_business);

            //call methods here 
            Init();
            GoToHome();
            GetOpenAndCloseTime();
            GetBusinessAddress();
        }

        private void Init()
        {
            InputBusinessName = FindViewById<TextInputEditText>(Resource.Id.InputBusinessName);
            InputBusinessPhone = FindViewById<TextInputEditText>(Resource.Id.InputBusinessPhone);
            InputBusinessDescription = FindViewById<TextInputEditText>(Resource.Id.InputBusinessDescription);
            InputBusinessEmail = FindViewById<TextInputEditText>(Resource.Id.InputBusinessEmail);
            InputBusinessMFOpen = FindViewById<MaterialAutoCompleteTextView>(Resource.Id.InputBusinessMFOpen);
            InputBusinessMFClose = FindViewById<MaterialAutoCompleteTextView>(Resource.Id.InputBusinessMFClose);
            InputBusinessSatOpen = FindViewById<MaterialAutoCompleteTextView>(Resource.Id.InputBusinessSatOpen);
            InputBusinessSatClose = FindViewById<MaterialAutoCompleteTextView>(Resource.Id.InputBusinessSatClose);
            InputBusinessSunOpen = FindViewById<MaterialAutoCompleteTextView>(Resource.Id.InputBusinessSatClose);
            InputBusinessSunClose = FindViewById<MaterialAutoCompleteTextView>(Resource.Id.InputBusinessSatClose);

            InputBusinessAddress = FindViewById<MaterialButton>(Resource.Id.InputBusinessAddress);
            BtnUpdateBusinessProfile = FindViewById<MaterialButton>(Resource.Id.BtnUpdateBusinessProfile);
        }

        private void GoToHome()
        {
            BtnUpdateBusinessProfile.Click += (s, e) =>
            {
                //StartActivity(new Intent(this, typeof(MainActivity)));
                RegisterBusinessProfile();
            };
        }

        private void GetBusinessAddress()
        {
            InputBusinessAddress.Click += (s, e) =>
            {
                new BusinessAddressDialogFragment()
                .Show(SupportFragmentManager.BeginTransaction(), "");
            };
        }

        private void Inputvalidation()
        {
            if (string.IsNullOrEmpty(InputBusinessName.Text) || string.IsNullOrWhiteSpace(InputBusinessName.Text))
            {
                InputBusinessName.RequestFocus();
                InputBusinessName.Error = "Provide business name";
                return;
            }

            if (string.IsNullOrEmpty(InputBusinessPhone.Text) || string.IsNullOrWhiteSpace(InputBusinessPhone.Text))
            {
                InputBusinessPhone.RequestFocus();
                InputBusinessPhone.Error = "Provide business phone number";
                return;
            }

            if (string.IsNullOrEmpty(InputBusinessDescription.Text) || string.IsNullOrWhiteSpace(InputBusinessDescription.Text))
            {
                InputBusinessDescription.RequestFocus();
                InputBusinessDescription.Error = "Provide business description";
                return;
            }

            if (string.IsNullOrEmpty(InputBusinessMFOpen.Text) || string.IsNullOrWhiteSpace(InputBusinessMFOpen.Text))
            {
                InputBusinessMFOpen.RequestFocus();
                InputBusinessMFOpen.Error = "Provide business Mon-Fri open time";
                return;
            }

            if (string.IsNullOrEmpty(InputBusinessMFClose.Text) || string.IsNullOrWhiteSpace(InputBusinessMFClose.Text))
            {
                InputBusinessMFClose.RequestFocus();
                InputBusinessMFClose.Error = "Provide business Mon-Fri close time";
                return;
            }

            if (string.IsNullOrEmpty(InputBusinessSatOpen.Text) || string.IsNullOrWhiteSpace(InputBusinessSatOpen.Text))
            {
                InputBusinessSatOpen.RequestFocus();
                InputBusinessSatOpen.Error = "Provide business Sat open time";
                return;
            }

            if (string.IsNullOrEmpty(InputBusinessSatClose.Text) || string.IsNullOrWhiteSpace(InputBusinessSatClose.Text))
            {
                InputBusinessSatClose.RequestFocus();
                InputBusinessSatClose.Error = "Provide business Sat close time";
                return;
            }

            if (string.IsNullOrEmpty(InputBusinessSunOpen.Text) || string.IsNullOrWhiteSpace(InputBusinessSunOpen.Text))
            {
                InputBusinessSunOpen.RequestFocus();
                InputBusinessSunOpen.Error = "Provide business Sun open time";
                return;
            }

            if (string.IsNullOrEmpty(InputBusinessSunClose.Text) || string.IsNullOrWhiteSpace(InputBusinessSunClose.Text))
            {
                InputBusinessSunClose.RequestFocus();
                InputBusinessSunClose.Error = "Provide business Sun close time";
                return;
            }
        }

        private async void RegisterBusinessProfile()
        {
            Inputvalidation();

            Business user = new Business()
            {
                Id = 0,
                //OwnerId = Preferences..Get("id")
                BusinessName = InputBusinessName.Text.Trim(),
                BusinessPhoneNumber = InputBusinessPhone.Text.Trim(),
                BusinessDescription = InputBusinessDescription.Text.Trim(),
                BusinessEmail = InputBusinessEmail.Text.Trim(),
                BusinessMFOpen = InputBusinessMFOpen.Text.Trim(),
                BusinessMFClose = InputBusinessMFClose.Text.Trim(),
                BusinessSatOpen = InputBusinessSatOpen.Text.Trim(),
                BusinessSatClose = InputBusinessSatClose.Text.Trim(),
                BusinessSunOpen = InputBusinessSunOpen.Text.Trim(),
                BusinessSunClose = InputBusinessSunClose.Text.Trim(),
                BusinessAddress = null,
                OnlineStatus = "Online",
                ImgUrl = "",
                Coordinates = "0 / 0"
            };


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(user);
            HttpContent data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient httpClient = new HttpClient();
            
            try
            {
                var results = await httpClient.PostAsync($"{API.Url}/businesses/register", data);

                if (results.IsSuccessStatusCode)
                {
                    var str = await results.Content.ReadAsStringAsync();
                    var response = Newtonsoft.Json.JsonConvert.DeserializeObject<AppUsers>(str);

                    if (response != null)
                    {
                        SuccessMessage("Your account has been successfully created");

                        //open main activity
                        StartActivity(new Intent(this, typeof(SignIn)));
                        OverridePendingTransition(Resource.Animation.Side_in_left, Resource.Animation.Side_out_right);
                    }
                }
                else
                {
                    var str_r = await results.Content.ReadAsStringAsync();

                    ErrorMessage(str_r);
                }

            }
            catch(HttpException ex)
            {
                ErrorMessage(ex.Message);
            }
        }

        private void GetOpenAndCloseTime()
        {
            InputBusinessMFOpen.Click += (s, e) =>
            {
                Timepicker(InputBusinessMFOpen);
            };

            InputBusinessMFClose.Click += (s, e) =>
            {
                Timepicker(InputBusinessMFClose);
            };

            InputBusinessSatOpen.Click += (s, e) =>
            {
                Timepicker(InputBusinessSatOpen);
            };

            InputBusinessSatClose.Click += (s, e) =>
            {
                Timepicker(InputBusinessSatClose);
            };

            InputBusinessSunOpen.Click += (s, e) =>
            {
                Timepicker(InputBusinessSunOpen);
            };

            InputBusinessSunClose.Click += (s, e) =>
            {
                Timepicker(InputBusinessSunClose);
            };
        }

        private void Timepicker(MaterialAutoCompleteTextView tv)
        {
            //tv.Text = 
            TimePickerDlg frag = TimePickerDlg.NewInstance(
            delegate (DateTime time)
            {
                tv.Text = time.ToShortTimeString();
            });

            frag.Show(SupportFragmentManager, "TimePick");
        }

        private void ErrorMessage(string str_r)
        {
            AndHUD.Shared.ShowError(this, str_r, MaskType.None, TimeSpan.FromSeconds(3));
        }

        private void SuccessMessage(string v)
        {
            AndHUD.Shared.ShowSuccess(this, v, MaskType.None, TimeSpan.FromSeconds(3));
        }
    }
}
public class TimePickerDlg : DialogFragment, Android.App.TimePickerDialog.IOnTimeSetListener
{
    public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
    {
        DateTime currentTime = DateTime.Now;
        DateTime selectedTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hourOfDay, minute, 0);
        //Log.Debug(TAG, selectedTime.ToLongTimeString());
        timeSelectedHandler(selectedTime);
    }
    public Action<DateTime> timeSelectedHandler = delegate { };
 
    public override Android.App.Dialog OnCreateDialog(Bundle savedInstanceState)
    {
        DateTime currentTime = DateTime.Now;
        bool is24HourFormat = DateFormat.Is24HourFormat(Activity);
        Android.App.TimePickerDialog dialog = new Android.App.TimePickerDialog
            (Activity, this, currentTime.Hour, currentTime.Minute, is24HourFormat);

        return dialog;
    }
    public static TimePickerDlg NewInstance(Action<DateTime> onTimeSelected)
    {
        TimePickerDlg frag = new TimePickerDlg();
        frag.timeSelectedHandler = onTimeSelected;
        return frag;
    }
}