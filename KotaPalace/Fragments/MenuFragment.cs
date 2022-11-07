using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidX.Fragment.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KotaPalace.Fragments
{
    public class MenuFragment : Fragment
    {
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

            return rootView;
        }
    }
}