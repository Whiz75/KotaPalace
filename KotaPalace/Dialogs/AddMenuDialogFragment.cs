﻿using Android.Content;
using Android.OS;
using Android.Views;
using AndroidHUD;
using AndroidX.Fragment.App;
using Google.Android.Material.Button;
using Google.Android.Material.Chip;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.TextField;
using KotaPalace.Models;
using KotaPalace_Api.Models;
using Plugin.FirebaseStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Menu = KotaPalace_Api.Models.Menu;

namespace KotaPalace.Dialogs
{
    public class AddMenuDialogFragment : DialogFragment
    {
        private Context context;

        private TextInputEditText InputItemName;
        private TextInputEditText InputItemPrice;

        private FloatingActionButton FabMenuImg;

        private ChipGroup chipGroup;
        private  List<Extras> Items = new List<Extras>();

        private Uri img_url = null;

        private MaterialButton BtnOpenAddDlg;
        private MaterialButton BtnSubmitMenu;

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public AddMenuDialogFragment()
        {
        }

        private string key;
        public AddMenuDialogFragment(string key)
        {
            this.key = key;
        }

        public override void OnStart()
        {
            base.OnStart();
            Dialog.Window.SetLayout(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            Dialog.SetCanceledOnTouchOutside(false);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.fragment_add_menu, container, false);

            context = view.Context;

            Init(view);
            //SubmitMenu();
            //OpenAddExtrasDialog(view);

            return view;
        }

        private void Init(View view)
        {

            InputItemName = view.FindViewById<TextInputEditText>(Resource.Id.InputItemName);
            InputItemPrice = view.FindViewById<TextInputEditText>(Resource.Id.InputItemPrice);
            chipGroup = view.FindViewById<ChipGroup>(Resource.Id.chipAddOns);

            FabMenuImg = view.FindViewById<FloatingActionButton>(Resource.Id.FabMenuImg);

            BtnOpenAddDlg = view.FindViewById<MaterialButton>(Resource.Id.BtnOpenAddDlg);

            BtnSubmitMenu = view.FindViewById<MaterialButton>(Resource.Id.BtnSubmitMenu);

            FabMenuImg.Click += (s, e) =>
            {

            };

            BtnOpenAddDlg.Click += (s, e) =>
            {
                //OpenAddExtrasDialog(view);
                AddExtrasFragment add = new AddExtrasFragment();
                add.Show(ChildFragmentManager.BeginTransaction(), "");
                add.AddOnAdded += Add_AddOnAdded;
            };

            BtnSubmitMenu.Click += (s, e) =>
            {
                SubmitMenuAsync();
            };
        }

        private void Add_AddOnAdded(object sender, AddExtrasFragment.AddOnHandler e)
        {
            Chip chip = new Chip(context);
            //create chip drawable
            ChipDrawable drawable = ChipDrawable.CreateFromAttributes(context,
                    null, 0, Resource.Style.Widget_MaterialComponents_Chip_Entry);

            //set chip drawable
            chip.SetChipDrawable(drawable);
            chip.Text = e.Item;

            Items.Add( new Extras() { Title =  e.Item });
           
            //chip.SetOnCloseIconClickListener(v1->
            //        chipGroup.removeView(chip));

            chipGroup.AddView(chip);
        }

        private async void SubmitMenuAsync()
        {

            if (string.IsNullOrEmpty(InputItemName.Text) || string.IsNullOrWhiteSpace(InputItemName.Text))
            {
                InputItemName.Error = "Please the item name";

            }
            else if (string.IsNullOrEmpty(InputItemPrice.Text) || string.IsNullOrWhiteSpace(InputItemPrice.Text))
            {
                InputItemPrice.Error = "Please the item's price";
            }
            else
            {
                try
                {
                    var businessId = Preferences.Get("businessId", 0);

                    //var file = await PickAndShow();

                    //var memoryStream = new MemoryStream();
                    //var st = await file.OpenReadAsync();
                    //string filename = $"{businessId}_menu_image";

                    //var result = CrossFirebaseStorage.Current
                    //    .Instance
                    //    .RootReference
                    //    .Child("Menu Images")
                    //    .Child(filename);

                    //await result.PutStreamAsync(st);

                    //var url = await result.GetDownloadUrlAsync();

                    Menu menu = new Menu()
                    {
                        BusinessId = businessId,
                        Name = InputItemName.Text.Trim(),
                        Price = Convert.ToDouble(InputItemPrice.Text),
                        Extras = Items,
                        Status = true,
                        Url = null
                    };

                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(menu);
                    HttpContent data = new StringContent(json, Encoding.UTF8, "application/json");
                    HttpClient httpClient = new HttpClient();

                    var results = await httpClient.PostAsync($"{API.Url}/menus", data);

                    if (results.IsSuccessStatusCode)
                    {
                        string str_out = await results.Content.ReadAsStringAsync();
                        Message("Menu added successfully!!!");

                        InputItemName.Text = "";
                        InputItemPrice.Text = "";
                        chipGroup.RemoveAllViews();
                    }
                }
                catch (Exception ex)
                {
                    Message(ex.Message);
                }
            }
        }

        private async Task<FileResult> PickAndShow()
        {
            try
            {
                var file = await FilePicker.PickAsync(new PickOptions()
                {
                    FileTypes = FilePickerFileType.Images
                });

                if (file != null)
                {
                    return file;
                }
            }
            catch (Exception ex)
            {
                Message(ex.Message);
            }

            return null;
        }

        private void Message(string s)
        {
            AndHUD.Shared.ShowSuccess(context, s, MaskType.None, TimeSpan.FromSeconds(3));
        }
        
    }
}