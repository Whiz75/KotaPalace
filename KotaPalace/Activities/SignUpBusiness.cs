using Android.Content;
using Android.OS;
using Android.Text.Format;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Google.Android.Material.Button;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.TextField;
using KotaPalace.Dialogs;
using KotaPalace.Models;
using Org.Apache.Http;
using Plugin.FirebaseStorage;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace KotaPalace.Activities
{
    [Android.App.Activity(Label = "SignUpBusiness")]
    public class SignUpBusiness : AppCompatActivity
    {
        private FloatingActionButton fab_edit_img;

        private TextInputEditText InputBusinessName;
        private TextInputEditText InputBusinessPhone;
        private TextInputEditText InputBusinessDescription;
        private TextInputEditText InputBusinessEmail;
        private TextInputEditText InputBusinessMFOpen;
        private TextInputEditText InputBusinessMFClose;
        private TextInputEditText InputBusinessSatOpen;
        private TextInputEditText InputBusinessSatClose;
        private TextInputEditText InputBusinessSunOpen;
        private TextInputEditText InputBusinessSunClose;

        private MaterialButton InputBusinessAddress;
        private MaterialButton BtnUpdateBusinessProfile;

        private FileResult file;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.activity_sign_up_business);

            //call methods here 
            Init();
            GetOpenAndCloseTime();
            //GetBusinessAddress();
        }

        private void Init()
        {
            fab_edit_img = FindViewById<FloatingActionButton>(Resource.Id.fab_edit_img);

            InputBusinessName = FindViewById<TextInputEditText>(Resource.Id.InputBusinessName);
            InputBusinessPhone = FindViewById<TextInputEditText>(Resource.Id.InputBusinessPhone);
            InputBusinessDescription = FindViewById<TextInputEditText>(Resource.Id.InputBusinessDescription);
            InputBusinessEmail = FindViewById<TextInputEditText>(Resource.Id.InputBusinessEmail);
            InputBusinessMFOpen = FindViewById<TextInputEditText>(Resource.Id.InputBusinessMFOpen);
            InputBusinessMFClose = FindViewById<TextInputEditText>(Resource.Id.InputBusinessMFClose);
            InputBusinessSatOpen = FindViewById<TextInputEditText>(Resource.Id.InputBusinessSatOpen);
            InputBusinessSatClose = FindViewById<TextInputEditText>(Resource.Id.InputBusinessSatClose);
            InputBusinessSunOpen = FindViewById<TextInputEditText>(Resource.Id.InputBusinessSatClose);
            InputBusinessSunClose = FindViewById<TextInputEditText>(Resource.Id.InputBusinessSatClose);

            InputBusinessAddress = FindViewById<MaterialButton>(Resource.Id.InputBusinessAddress);
            BtnUpdateBusinessProfile = FindViewById<MaterialButton>(Resource.Id.BtnUpdateBusinessProfile);

            InputBusinessAddress.Click += InputBusinessAddress_Click;

            fab_edit_img.Click += async (s, e) =>
            {
                file = await PickAndShow();
            };

            BtnUpdateBusinessProfile.Click += (s, e) =>
            {
                RegisterBusinessProfile();
            };
        }
        string address;
        string coordinates;
        private void InputBusinessAddress_Click(object sender, EventArgs e)
        {
            var dlg = new BusinessAddressDialogFragment();
            dlg.Show(SupportFragmentManager.BeginTransaction(), "Address");
            dlg.CoordinatesHandler += (ss, ee) =>
            {
                address = ee.Address;
                coordinates = ee.Coordinates;

                InputBusinessAddress.Text = address;
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

            if (file == null)
            {
                ErrorMessage("Please upload business logo");
            }
        }

        private async void RegisterBusinessProfile()
        {
            Inputvalidation();
            string Ownerid = Preferences.Get("Id", null);

            var memoryStream = new MemoryStream();
            var st = await file.OpenReadAsync();
            string filename = $"{file.FileName}";
            var result = CrossFirebaseStorage.Current
                .Instance
                .RootReference
                .Child("Business logo")
                .Child(filename);

            await result.PutStreamAsync(st);

            var url = await result.GetDownloadUrlAsync();

            Business business = new Business()
            {
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
                BusinessAddress = address,
                OnlineStatus = "Online",
                ImgUrl = url.ToString(),
                Coordinates = coordinates,
                OwnerId = Ownerid
            };


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(business);
            HttpContent data = new StringContent(json, Encoding.UTF8, "application/json");
            HttpClient httpClient = new HttpClient();
            
            try
            {
                var results = await httpClient.PostAsync($"{API.Url}/businesses/register", data);

                if (results.IsSuccessStatusCode)
                {
                    var str = await results.Content.ReadAsStringAsync();
                    var response = Newtonsoft.Json.JsonConvert.DeserializeObject<Business>(str);

                    if (response != null)
                    {
                        SuccessMessage("Your account has been successfully created");

                        //Preferences.Set("businessId", response.Id);

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

            InputBusinessMFOpen.Touch += (s, e) =>
            {
                Timepicker(InputBusinessMFOpen);
            };

            InputBusinessMFClose.Touch += (s, e) =>
            {
                Timepicker(InputBusinessMFClose);
            };

            InputBusinessSatOpen.Touch += (s, e) =>
            {
                Timepicker(InputBusinessSatOpen);
            };

            InputBusinessSatClose.Touch += (s, e) =>
            {
                Timepicker(InputBusinessSatClose);
            };

            InputBusinessSunOpen.Touch += (s, e) =>
            {
                Timepicker(InputBusinessSunOpen);
            };

            InputBusinessSunClose.Touch += (s, e) =>
            {
                Timepicker(InputBusinessSunClose);
            };
        }

        private async Task<FileResult> PickAndShow()
        {
            try
            {
                var file = await FilePicker.PickAsync(new PickOptions()
                {
                    FileTypes = FilePickerFileType.Images
                });

                if (file != null)
                {
                    return file;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage(ex.Message);
            }

            return null;
        }

        private void Timepicker(TextInputEditText tv)
        {
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