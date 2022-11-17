using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Button;
using Google.Android.Material.Chip;
using KotaPalace.Adapters;
using KotaPalace.Models;
using KotaPalace_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Text;
using Xamarin.Essentials;
using Context = Android.Content.Context;

namespace KotaPalace.Dialogs
{
    public class OrderViewFragment : DialogFragment
    {
        private Context context;

        private ImageView close_order_view;

        private TextView business_name;
        private TextView business_order_price;
        private TextView order_quantity;
        private TextView business_order_description;
        private TextView business_Id;
        private TextView business_status;

        private MaterialButton BtnBuyNow;

        private ChipGroup chipGroup;

        private int id;

        public OrderViewFragment()
        {
        }

        public OrderViewFragment(int id)
        {
            this.id = id;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.order_row_view, container, false);
            context = view.Context;
            Init(view);
            LoadOrdersAsync();

            return view;
        }

        private void Init(View view)
        {
            close_order_view = view.FindViewById<ImageView>(Resource.Id.close_order_view);

            business_name = view.FindViewById<TextView>(Resource.Id.business_name);
            business_order_price = view.FindViewById<TextView>(Resource.Id.business_order_price);
            order_quantity = view.FindViewById<TextView>(Resource.Id.order_quantity);
            business_order_description = view.FindViewById<TextView>(Resource.Id.business_order_description);
            business_Id = view.FindViewById<TextView>(Resource.Id.business_Id);
            business_status = view.FindViewById<TextView>(Resource.Id.business_status);

            BtnBuyNow = view.FindViewById<MaterialButton>(Resource.Id.BtnBuyNow);

            chipGroup = view.FindViewById<ChipGroup>(Resource.Id.AddOnsChips);

            close_order_view.Click += (s, e) =>
            {
                Dismiss();
            };

            BtnBuyNow.Click += (s, e) =>
            {
                //LoadOrdersAsync();
            };
        }

        private async void LoadOrdersAsync()
        {
            var businessId = Preferences.Get("businessId", 0);

            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"{API.Url}/orders/{businessId}"); // car details

            if (response.IsSuccessStatusCode)
            {
                var str_results = await response.Content.ReadAsStringAsync();
                var results = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(str_results);

                Chip chip = new Chip(context);

                foreach (var item in results)
                {
                    if(item.BusinessId == businessId)
                    {
                        business_Id.Text = $"Business ID: {item.BusinessId}";
                        business_status.Text = $"Status: {item.Status}";

                        var extras = item.OrderItems.ToList<OrderItems>();

                        foreach (var item2 in extras)
                        {
                            business_order_price.Text = $"R{item2.Price}";
                            order_quantity.Text = $"Quantity: {item2.Quantity}";

                            string i = item2.Extras;
                            string[] itemList = i.Split('#', StringSplitOptions.RemoveEmptyEntries);

                            foreach (string str2 in itemList)
                            {
                                Message(str2);

                            }


                        }
                    }
                }
            }
            else
            {
                var str_results = await response.Content.ReadAsStringAsync();
                Message(str_results);
            }
        }

        private void Message(string str_results)
        {
            AndHUD.Shared.ShowError(context, str_results, MaskType.None, TimeSpan.FromSeconds(3));
        }
    }
}