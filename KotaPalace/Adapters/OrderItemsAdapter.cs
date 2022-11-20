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
    public class OrderItemAdapter : RecyclerView.Adapter
    {
        List<OrderItems> orders = new List<OrderItems>();

        public OrderItemAdapter(List<OrderItems> orders)
        {
            this.orders = orders;
        }

        public override int ItemCount => orders.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            OrderItemViewHolder vh = holder as OrderItemViewHolder;
            var orderItems = orders[position];

            vh.Name.Text = $"Name:{orderItems.ItemName}" ;
            vh.Price.Text = $"Price:R{orderItems.Price}";
            vh.Quantity.Text = $"Quantity:{orderItems.Quantity}";
        }
        public event EventHandler<OrderBtnClick> BtnClick;

        public class OrderBtnClick : EventArgs
        {
            public int Position { get; set; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemview = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.order_items_row, parent, false);
            OrderItemViewHolder vh = new OrderItemViewHolder(itemview);
            return vh;
        }
    }

    public class OrderItemViewHolder : RecyclerView.ViewHolder
    {
        public MaterialTextView Quantity { get; set;}
        public MaterialTextView Price { get; set;}
        public MaterialTextView Name { get; set; }

        public OrderItemViewHolder(View itemview) : base(itemview)
        {
            Name = itemview.FindViewById<MaterialTextView>(Resource.Id.order_item_name);
            Price = itemview.FindViewById<MaterialTextView>(Resource.Id.order_item_price);
            Quantity = itemview.FindViewById<MaterialTextView>(Resource.Id.order_item_quantity);

        }
    }
}