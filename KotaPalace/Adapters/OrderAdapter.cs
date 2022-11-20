using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Button;
using Google.Android.Material.Chip;
using Google.Android.Material.TextView;
using KotaPalace_Api.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace KotaPalace.Adapters
{
    public class OrderAdapter : RecyclerView.Adapter
    {
        List<Order> orders = new List<Order>();

        public OrderAdapter(List<Order> orders)
        {
            this.orders = orders;
        }

        public override int ItemCount => orders.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            OrderViewHolder vh = holder as OrderViewHolder;
            var order = orders[position];

            vh.OrderId.Text = $"Id :{order.Id}";
            vh.Status.Text = $"Status :{order.Status}";

            //vh.OrderId.Text = $"Available :{order.Id}";
            vh.view_btn.Click += (s, e) => { BtnClick.Invoke(vh.ItemView.Context, new OrderBtnClick() { Position = position }); };
        }
        public event EventHandler<OrderBtnClick> BtnClick;

        public class OrderBtnClick : EventArgs
        {
            public int Position { get; set; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemview = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.order_row, parent, false);
            OrderViewHolder vh = new OrderViewHolder(itemview);
            return vh;
        }
    }

    public class OrderViewHolder : RecyclerView.ViewHolder
    {
        public MaterialTextView CustomerId { get; set;}
        public MaterialTextView Status {get; set;}
        public MaterialTextView OrderId { get; set; }
        public MaterialButton view_btn { get; set; }

        public OrderViewHolder(View itemview) : base(itemview)
        {
            OrderId = itemview.FindViewById<MaterialTextView>(Resource.Id.customer_id);
            Status = itemview.FindViewById<MaterialTextView>(Resource.Id.order_status);
            //MenuId = itemview.FindViewById<AppCompatTextView>(Resource.Id.row_menu_id);
            //OrderId = itemview.FindViewById<MaterialTextView>(Resource.Id.row_quantity);

            //chipGroup = itemview.FindViewById<ChipGroup>(Resource.Id.AddOnsChips);

            view_btn = itemview.FindViewById<MaterialButton>(Resource.Id.view_btn);
        }
    }
}