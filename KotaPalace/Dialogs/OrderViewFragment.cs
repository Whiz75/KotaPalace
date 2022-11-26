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
using Org.W3c.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static Android.Content.ClipData;
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

        private RecyclerView orderItemsRecyclerView;

        private MaterialButton BtnProcess;

        private ChipGroup chipGroup;

        private int id;

        private int businessId = Preferences.Get("businessId", 0);
        private string Id = Preferences.Get("Id", null);

        private Order order;
        readonly List<string> ExtrasItemList = new List<string>();
        readonly List<OrderItems> OrderItemList = new List<OrderItems>();

        public OrderViewFragment()
        {
        }

        public OrderViewFragment(int id)
        {
            this.id = id;
        }

        public OrderViewFragment(Order order)
        {
            this.order = order;
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
            //CheckStatus();
            LoadOrdersAsync();
            //LoadOrderItemsAsync();

            return view;
        }

        private void Init(View view)
        {
            close_order_view = view.FindViewById<ImageView>(Resource.Id.close_order_view);

            business_name = view.FindViewById<TextView>(Resource.Id.business_name);
            business_order_price = view.FindViewById<TextView>(Resource.Id.business_order_price);
            business_Id = view.FindViewById<TextView>(Resource.Id.business_Id);
            business_status = view.FindViewById<TextView>(Resource.Id.business_status);

            business_status.Text = order.Status;

            orderItemsRecyclerView = view.FindViewById<RecyclerView>(Resource.Id.orderItemsRecyclerView);

            BtnProcess = view.FindViewById<MaterialButton>(Resource.Id.BtnProcess);

            chipGroup = view.FindViewById<ChipGroup>(Resource.Id.AddOnsChips);

            close_order_view.Click += (s, e) =>
            {
                Dismiss();
            };

            BtnProcess.Click += (s, e) =>
            {
                ProcessOrderAsync();
            };
        }

        private async void LoadOrdersAsync()
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync($"{API.Url}/orders/single/{order.Id}"); // car details

                if (response.IsSuccessStatusCode)
                {
                    var str_results = await response.Content.ReadAsStringAsync();
                    
                    var results = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(str_results);

                    RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(context);
                    orderItemsRecyclerView.SetLayoutManager(mLayoutManager);
                    OrderItemAdapter mAdapter = new OrderItemAdapter(OrderItemList);
                    orderItemsRecyclerView.SetAdapter(mAdapter);

                   
                    business_Id.Text = $"Business ID: {order.BusinessId}";
                    business_status.Text = $"Status: {order.Status}";

                    var extras = order.OrderItems;

                    foreach (var item in extras)
                    {
                        OrderItemList.Add(item);
                    }
                    mAdapter.NotifyDataSetChanged(); 
                }
                else
                {
                    var str_results = await response.Content.ReadAsStringAsync();
                    Message(str_results);
                }
            }
            catch (HttpRequestException e)
            {
                Message(e.Message);
            }
        }

        private async void CheckStatus()
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"{API.Url}/status/{id}");

            if(response.IsSuccessStatusCode)
            {
                var str = await response.Content.ReadAsStringAsync();
                var order = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(str);

                if(order.Status == "Pending")
                {
                    BtnProcess.Text = "PROCESS";
                }else if(order.Status == "Accepted")
                {
                    BtnProcess.Text = "DONE!";
                }
                else
                {
                    BtnProcess.Text = "READY";
                    BtnProcess.Enabled = false;
                }
            }
        }

        private async void ProcessOrderAsync()
        {
            Order order = new Order();
            HttpClient client = new HttpClient();

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(order);
            HttpContent data = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PutAsync($"{API.Url}/orders/process/{id}", data);

                if (response.IsSuccessStatusCode)
                {

                    string str_out = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var str_results = await response.Content.ReadAsStringAsync();
                    Message(str_results);
                }
            }
            catch (HttpRequestException ex)
            {
                Message(ex.Message);
            }
        }

        private void Message(string str_results)
        {
            AndHUD.Shared.ShowError(context, str_results, MaskType.None, TimeSpan.FromSeconds(3));
        }
    }
}