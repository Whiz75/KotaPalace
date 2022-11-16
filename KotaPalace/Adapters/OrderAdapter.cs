﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Chip;
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
        ObservableCollection<OrderItems> orders = new ObservableCollection<OrderItems>();

        public OrderAdapter(ObservableCollection<OrderItems> orders)
        {
            this.orders = orders;
        }

        public override int ItemCount => orders.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            OrderViewHolder vh = holder as OrderViewHolder;
            var order = orders[position];

            vh.Name.Text = $"Name :{order.Id}";
            vh.Price.Text = $"Price :{order.Price}";
           
            vh.Quantity.Text = $"Available :{order.Quantity}";
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
        public AppCompatTextView Name { get; set;}
        public AppCompatTextView Price {get; set;}
        public AppCompatTextView Quantity { get; set; }

        public OrderViewHolder(View itemview) : base(itemview)
        {
            Name = itemview.FindViewById<AppCompatTextView>(Resource.Id.row_name);
            Price = itemview.FindViewById<AppCompatTextView>(Resource.Id.row_price);
            //MenuId = itemview.FindViewById<AppCompatTextView>(Resource.Id.row_menu_id);
            Quantity = itemview.FindViewById<AppCompatTextView>(Resource.Id.row_quantity);

            //chipGroup = itemview.FindViewById<ChipGroup>(Resource.Id.AddOnsChips);
        }
    }
}