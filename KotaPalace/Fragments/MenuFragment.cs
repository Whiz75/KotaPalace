using Android.Content;
using Android.Icu.Util;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidHUD;
using AndroidX.Fragment.App;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Button;
using KotaPalace.Adapters;
using KotaPalace.Dialogs;
using KotaPalace.Models;
using KotaPalace_Api.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using static AndroidX.RecyclerView.Widget.RecyclerView;
using Menu = KotaPalace_Api.Models.Menu;

namespace KotaPalace.Fragments
{
    public class MenuFragment : Fragment
    {
        private Context context;
        private MaterialButton BtnAddMenu;

        private RecyclerView menu_rv;

        public MenuFragment()
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
            View rootView = inflater.Inflate(Resource.Layout.fragment_menu, container, false);

            context = rootView.Context;

            Init(rootView);
            AddMenu();
            LoadMenusAsync();

            return rootView;
        }

        private void Init(View view)
        {
            BtnAddMenu = view.FindViewById<MaterialButton>(Resource.Id.BtnAddMenu);
            menu_rv = view.FindViewById<RecyclerView>(Resource.Id.menu_rv);
        }

        private void AddMenu()
        {
            BtnAddMenu.Click += (s, e) =>
            {
                new AddMenuDialogFragment()
                .Show(ChildFragmentManager.BeginTransaction(), "");
            };
        }

        private async void LoadMenusAsync()
        {
            var businessId = Preferences.Get("businessId", 0);

            HttpClient client = new HttpClient();
            var response = await client.GetAsync($"{API.Url}/menus/all/{businessId}"); // car details

            if (response.IsSuccessStatusCode)
            {
                var str_results = await response.Content.ReadAsStringAsync();
                //get driver info
                var results = Newtonsoft.Json.JsonConvert.DeserializeObject<ObservableCollection<Menu>>(str_results);

                ObservableCollection<Menu> MenuList = new ObservableCollection<Menu>();

                foreach (var item in results)
                {
                    MenuList.Add(item);
                    //is it okay like this?yezs. thanks you saw it?

                }

                


                RecyclerView.LayoutManager mLayoutManager = new LinearLayoutManager(context);
                menu_rv.SetLayoutManager(mLayoutManager);
                MenuAdapter mAdapter = new MenuAdapter(MenuList);
                menu_rv.HasFixedSize = true;
                menu_rv.SetAdapter(mAdapter);
            }
            else
            {
                var str_results = await response.Content.ReadAsStringAsync();
                Message(str_results);
            }

        }

        private void Message(string t)
        {
            AndHUD.Shared.ShowError(context, t,MaskType.None,TimeSpan.FromSeconds(3));
        }
    }
}