using Android.App;
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
    public class MenuAdapter : RecyclerView.Adapter
    {
        Context context;

        public ObservableCollection<Menu> MenuList;

        public MenuAdapter(ObservableCollection<Menu> menuList)
        {
            MenuList = menuList;
        }

        public override int ItemCount => MenuList.Count;

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MenuViewHolder vh = holder as MenuViewHolder;
            var menu = MenuList[position];
            context = vh.ItemView.Context;

            vh.Name.Text = $"{menu.Name}" ;
            vh.Price.Text = $"R{menu.Price}";
            vh.MenuId.Text = $"{menu.Id}";

            if(menu.Url != null)
            {
                ImageService
                    .Instance
                    .LoadUrl(menu.Url)
                    .Into(vh.row_menuIcon);
            }

            vh.chipGroup.RemoveAllViews();

            foreach (var i in menu.Extras)
            {
                Chip chip = new Chip(vh.ItemView.Context);

                chip.Text = i.Title;
                vh.chipGroup.AddView(chip);
            }
            

            vh.BtnUpdate.Click += (s, e) =>
            {
                BtnClick.Invoke(vh.ItemView.Context, new MenuBtnClick { pos = position});
            };

            vh.BtnDelete.Click += (s, e) =>
            {
                RemoveItem(menu.Id);
            };   
        }

        public event EventHandler<MenuBtnClick> BtnClick;

        public class MenuBtnClick: EventArgs
        {
            public int pos { get; set; }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.menu_row, parent, false);
            MenuViewHolder vh = new MenuViewHolder(itemView);
            return vh;
        }


        private void RemoveItem(int id)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(context);
            builder.SetTitle("Remove Item");
            builder.SetMessage("Are you sure want to remove item?");
            builder.SetNegativeButton("No", delegate
            {
                builder.Dispose();
            });
            builder.SetPositiveButton("Yes", async delegate
            {
                HttpClient httpClient = new HttpClient();

                var result = await httpClient.DeleteAsync($"{API.Url}/menus/{id}");

                if (result.IsSuccessStatusCode)
                {
                    string str_out = await result.Content.ReadAsStringAsync();
                    AndHUD.Shared.ShowSuccess(context, str_out, MaskType.None, TimeSpan.FromSeconds(3));
                }
                else
                {
                    string str_out = await result.Content.ReadAsStringAsync();
                    AndHUD.Shared.ShowError(context, str_out, MaskType.None, TimeSpan.FromSeconds(3));
                }
                builder.Dispose();
            });
            builder.Show();
        }
    }

    public class MenuViewHolder : RecyclerView.ViewHolder
    {
        public AppCompatTextView Name { get; set; }
        public AppCompatTextView Price { get; set; }

        public ChipGroup chipGroup { get; set; }

        public AppCompatTextView MenuId { get;set; }

        public AppCompatImageView row_menuIcon { get; set; }

        public MaterialButton BtnUpdate { get; set; }
        public MaterialButton BtnDelete { get; set; }

        public MenuViewHolder(View itemview) : base(itemview)
        {
            Name = itemview.FindViewById<AppCompatTextView>(Resource.Id.row_name);
            Price = itemview.FindViewById<AppCompatTextView>(Resource.Id.row_price);
            MenuId = itemview.FindViewById<AppCompatTextView>(Resource.Id.row_menu_id);
            row_menuIcon = itemview.FindViewById<AppCompatImageView>(Resource.Id.row_menuIcon);

            chipGroup = itemview.FindViewById<ChipGroup>(Resource.Id.AddOnsChips);

            BtnUpdate = itemview.FindViewById<MaterialButton>(Resource.Id.row_btn_update);
            BtnDelete = itemview.FindViewById<MaterialButton>(Resource.Id.row_btn_delete);
        }
    }
}