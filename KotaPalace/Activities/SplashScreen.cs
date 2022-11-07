using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KotaPalace.Activities
{
    [Activity(Label = "SplashScreen", Theme = "@style/MyTheme.Splash", MainLauncher = true)]
    public class SplashScreen : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            // Create your application here
  
            Task startWork = new Task(() =>
            {
                Task.Delay(3000);
            });
            startWork.ContinueWith(t =>
            {
                try
                {
                    Intent intent = new Intent(Application.Context, typeof(SignIn));
                    StartActivity(intent);
                    OverridePendingTransition(Resource.Animation.Side_in_right, Resource.Animation.Side_out_left);
                }
                catch (Exception ex)
                {
                    
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            startWork.Start();

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}