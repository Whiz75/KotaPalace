
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
using Microcharts;
using SkiaSharp;
using System.Net.Http;
using KotaPalace.Models;
using Xamarin.Essentials;
using KotaPalace_Api.Models;
using AndroidHUD;

namespace KotaPalace.Fragments
{
    public class ReportFragment : Fragment
    {
        private Context context;
        private ShimmerFrameLayout container;
        private ChartView chartReport;

        private readonly List<string> months = new List<string>();
        private readonly List<int> counter = new List<int>();

        private int businessId = Preferences.Get("businessId", 0);
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

        private async void GetADates()
        {
            string[] monthNames = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
            foreach (var m in monthNames)
            {
                months.Add(m);
                counter.Add(0);
            }

            HttpClient client = new HttpClient();
            try
            {
                var response = await client.GetAsync($"{API.Url}/orders/{businessId}");

                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var order = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(data);

                    if (order != null)
                    {
                        foreach (var item in order.OrderDate.ToString())
                        {
                            if (months.Contains(order.OrderDate.ToString("MMMM")))
                            {
                                int pos = months.IndexOf(order.OrderDate.ToString("MMMM"));
                                counter[pos] = counter[pos] + 1;
                            }
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Message(ex.Message);
            }
        }

       
        private void DrawCharts()
        {
            List<ChartEntry> DataEntry = new List<ChartEntry>();
            string[] colors = { "#157979", "#154779", "#5F1C80", "#801C59",
                            "#9CBDD6", "#75863D" , "#1E1011", "#48D53B",
                            "#48D5C7", "#6761F0", "#8A80A3", "#D3C6F4"
            };

            for (int i = 0; i < months.Count; i++)
            {
                DataEntry.Add(new ChartEntry(counter[i])
                {
                    Label = months[i],
                    Color = SKColor.Parse(colors[i]),
                    ValueLabel = counter[i].ToString(),
                    TextColor = SKColor.Parse(colors[i]),
                    ValueLabelColor = SKColor.Parse(colors[i])
                });
                if (months[i].Contains(DateTime.Now.ToString("MMMM")))
                {
                    break;
                }
            }

            var chart = new RadarChart()
            {
                Entries = DataEntry,
            };
            chartReport.Chart = chart;
        }

        private void Message(string message)
        {
            AndHUD.Shared.ShowError(context, message,MaskType.None,TimeSpan.FromSeconds(3));
        }

    }
}