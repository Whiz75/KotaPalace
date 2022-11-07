using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using AndroidX.AppCompat.App;
using Gauravk.BubbleNavigation;
using Gauravk.BubbleNavigation.Listeners;
using KotaPalace.Fragments;
using System;
using System.ComponentModel;

namespace KotaPalace
{
    [Activity(Label = null, Theme = "@style/AppTheme", MainLauncher = false)]
    public class MainActivity : AppCompatActivity, IBubbleNavigationChangeListener
    {
        private BubbleNavigationLinearView navigationView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            if(savedInstanceState == null)
            {
                SupportFragmentManager.BeginTransaction().Add(Resource.Id.fragHost,new OrdersFragment()).Commit();
            }

            //call methods here
            Init();
        }

        private void Init()
        {
            navigationView = FindViewById<BubbleNavigationLinearView>(Resource.Id.bottom_navigation_view_linear);

            navigationView.SetNavigationChangeListener(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnNavigationChanged(View view, int position)
        {
            switch (position)
            {
                case 0:
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragHost, new OrdersFragment()).Commit();
                    break;
                case 1:
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragHost, new MenuFragment()).Commit();
                    break;
                case 2:
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragHost, new ProfileFragment()).Commit();
                    break;
                case 3:
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragHost, new ReportFragment()).Commit();
                    break;
                default:
                    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragHost, new OrdersFragment()).Commit();
                    break;
            }
        }
    }
}