
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microcharts.Droid;
using static Android.Provider.ContactsContract.RawContacts;
using Facebook.Shimmer;
using System.Threading.Tasks;

namespace KotaPalace.Fragments
{
    public class ReportFragment : Fragment
    {
        private Context context;
        private ShimmerFrameLayout container;
        private ChartView chartReport;

        public ReportFragment()
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
            View view = inflater.Inflate(Resource.Layout.fragment_report, container, false);
            context = view.Context;
            Init(view);
            LoadGraphs();

            return view;
        }

        private void Init(View view)
        {
            //var charts = Data.CreateQuickstart();
            container = view.FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_view_container);
            
            chartReport = view.FindViewById<ChartView>(Resource.Id.chartReport);
        }

        private void LoadGraphs()
        {
            container.StartShimmer(); // If auto-start is set to false

            Task startWork = new Task(() =>
            {
                Task.Delay(3000);
            });
            startWork.ContinueWith(t =>
            {
                try
                {
                    container.StopShimmer();
                    container.Visibility = ViewStates.Gone;
                }
                catch (Exception ex)
                {

                    Toast.MakeText(context, ex.Message, ToastLength.Long).Show();
                }
            }, TaskScheduler.FromCurrentSynchronizationContext());
            startWork.Start();
        }
    }
}