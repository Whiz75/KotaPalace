﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.AppCompat.Widget;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using FFImageLoading;
using Google.Android.Material.Button;
using Google.Android.Material.Chip;
using Java.Util.Zip;
using KotaPalace.Dialogs;
using KotaPalace.Models;
using KotaPalace_Api.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Contexts;
using System.Text;
using static Android.Resource;
using static Android.Views.View;
using Context = Android.Content.Context;
using Menu = KotaPalace_Api.Models.Menu;

namespace KotaPalace.Adapters
{
    public class UsersAdapter : RecyclerView.Adapter
    {
        Context context;

        public ObservableCollection<AppUsers> UserList;

        public UsersAdapter(ObservableCollection<AppUsers> userList)
        {
            UserList = userList;
        }

        public override int ItemCount => UserList.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            UserViewHolder vh = holder as UserViewHolder;
            var user = UserList[position];
            context = vh.ItemView.Context;

            vh.Name.Text = $"{user.Firstname}" ;
            vh.Email.Text = $"{user.Email}";
            vh.PhoneNumber.Text = $"{user.PhoneNumber}";

            //if(user.Url != null)
            //{
            //    ImageService
            //        .Instance
            //        .LoadUrl(user.Url)
            //        .Into(vh.Image);
            //}

            vh.BtnUpdate.Click += (s, e) =>
            {
                BtnClick.Invoke(vh.ItemView.Context, new UserBtnClick { Position = position});
            };

            vh.BtnDelete.Click += (s, e) =>
            {
                RemoveItem(user.Id);
            };   
        }

        public event EventHandler<UserBtnClick> BtnClick;

        public class UserBtnClick: EventArgs
        {
            public int Position { get; set; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.user_row, parent, false);
            UserViewHolder vh = new UserViewHolder(itemView);
            return vh;
        }


        private void RemoveItem(string id)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle("DELETE USER");
            builder.SetMessage("Are you sure want to delete user?");
            builder.SetNegativeButton("No", delegate
            {
                builder.Dispose();
            });
            builder.SetPositiveButton("Yes", async delegate
            {
                //HttpClient httpClient = new HttpClient();

                //var result = await httpClient.DeleteAsync($"{API.Url}/menus/{id}");

                //if (result.IsSuccessStatusCode)
                //{
                //    string str_out = await result.Content.ReadAsStringAsync();
                //    AndHUD.Shared.ShowSuccess(context, str_out, MaskType.None, TimeSpan.FromSeconds(3));
                //}
                //else
                //{
                //    string str_out = await result.Content.ReadAsStringAsync();
                //    AndHUD.Shared.ShowError(context, str_out, MaskType.None, TimeSpan.FromSeconds(3));
                //}
                AndHUD.Shared.ShowError(context, $"{id}", MaskType.None, TimeSpan.FromSeconds(3));
                builder.Dispose();
            });
            builder.Show();
        }
    }

    public class UserViewHolder : RecyclerView.ViewHolder
    {
        public AppCompatTextView Name { get; set; }
        public AppCompatTextView Email { get; set; }
        public AppCompatTextView PhoneNumber { get; set; }

        public AppCompatImageView Image { get; set; }

        public MaterialButton BtnUpdate { get; set; }
        public MaterialButton BtnDelete { get; set; }

        public UserViewHolder(View itemview) : base(itemview)
        {
            Name = itemview.FindViewById<AppCompatTextView>(Resource.Id.row_user_name);
            Email = itemview.FindViewById<AppCompatTextView>(Resource.Id.row_user_email);
            PhoneNumber = itemview.FindViewById<AppCompatTextView>(Resource.Id.row_phone_number);

            Image = itemview.FindViewById<AppCompatImageView>(Resource.Id.row_user_image);

            BtnUpdate = itemview.FindViewById<MaterialButton>(Resource.Id.btn_update_user);
            BtnDelete = itemview.FindViewById<MaterialButton>(Resource.Id.btn_delete_user);
        }
    }
}