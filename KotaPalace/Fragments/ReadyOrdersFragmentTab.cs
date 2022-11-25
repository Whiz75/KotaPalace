﻿using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Facebook.Shimmer;
using KotaPalace.Adapters;
using KotaPalace.Models;
using KotaPalace_Api.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Context = Android.Content.Context;

namespace KotaPalace.Fragments
{
    public class ReadyOrdersFragmentTab : Fragment
    {
        private Context mContext;
        private ShimmerFrameLayout container;
        private RecyclerView ready_orders_recyclerView;

        List<Order> OrderList = new List<Order>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View view = inflater.Inflate(Resource.Layout.ready_orders_fragment_tab,container,false);
            mContext = view.Context;
            Init(view);
            LoadReadyOrdersAsync();

            return view;
        }

        private void Init(View view)
        {
            container = view.FindViewById<ShimmerFrameLayout>(Resource.Id.container);
            ready_orders_recyclerView = view.FindViewById<RecyclerView>(Resource.Id.ready_orders_recyclerView);
        }

        private async void LoadReadyOrdersAsync()
        {
            var Id = Preferences.Get("Id", null);
            try
            {
                container.StartLayoutAnimation();

                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"{API.Url}/orders/completed/{Id}"); 

                RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(mContext);
                ready_orders_recyclerView.SetLayoutManager(mLayoutManager);
                ReadyOrdersAdapter mAdapter = new ReadyOrdersAdapter(OrderList);


                ready_orders_recyclerView.HasFixedSize = true;
                ready_orders_recyclerView.SetAdapter(mAdapter);

                if (response.IsSuccessStatusCode)
                {
                    var str_results = await response.Content.ReadAsStringAsync();
                    //get driver info
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(str_results);

                    foreach (var item in results)
                    {
                        OrderList.Add(item);
                        mAdapter.NotifyDataSetChanged();
                    }
                }
                else
                {
                    var str_results = await response.Content.ReadAsStringAsync();
                    Message(str_results);
                }

                container.StopShimmer();

            }
            catch (Exception ex)
            {
                Message(ex.Message);
            }
        }

        private void Message(string message)
        {
            AndHUD.Shared.ShowError(mContext, message, MaskType.None, TimeSpan.FromSeconds(3));
        }
    }
}