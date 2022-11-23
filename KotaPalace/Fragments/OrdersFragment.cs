using Android.OS;
using Android.Views;
using Facebook.Shimmer;
using Google.Android.Material.Tabs;
using System.Collections.Generic;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using ViewPager = AndroidX.ViewPager.Widget.ViewPager;

namespace KotaPalace.Fragments
{
    public class OrdersFragment : Fragment
    {
        private ShimmerFrameLayout shimmer_container;

        private TabLayout tabHost;
        private ViewPager viewpager;

        private int[] tabIcons = {Resource.Drawable.ic_order,
            Resource.Drawable.ic_restaurant_menu};

        public OrdersFragment()
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
            View view = inflater.Inflate(Resource.Layout.fragment_orders, container, false);

            Init(view);
            SetViewrPager(viewpager);

            return view;
        }

        private void Init(View view)
        {
            shimmer_container = view.FindViewById<ShimmerFrameLayout>(Resource.Id.shimmer_container);
            //initialize variables
            tabHost = view.FindViewById<TabLayout>(Resource.Id.TabHost);
            viewpager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
        }

        private void SetViewrPager(ViewPager pager)
        {
            var adapter = new Adapter(ChildFragmentManager);
            adapter.AddFragment(new PrepareOrderFragmentTab(), "PREPARE");
            adapter.AddFragment(new ReadyOrdersFragmentTab(), "READY");

            tabHost.SetupWithViewPager(viewpager);
            viewpager.Adapter = adapter;
            viewpager.Adapter.NotifyDataSetChanged();

            //call method to set tab icons here
            setupTabIcons();
        }

        private void setupTabIcons()
        {
            tabHost.GetTabAt(0).SetIcon(tabIcons[0]);
            tabHost.GetTabAt(1).SetIcon(tabIcons[1]);
        }
    }
}

class Adapter : AndroidX.Fragment.App.FragmentPagerAdapter
{
    List<Fragment> fragments = new List<Fragment>();
    List<string> fragmentTitles = new List<string>();
    public Adapter(FragmentManager fm) : base(fm) { }
    public void AddFragment(Fragment fragment, string title)
    {
        fragments.Add(fragment);
        fragmentTitles.Add(title);
    }
    public override Fragment GetItem(int position)
    {
        return fragments[position];
    }
    public override int Count
    {
        get
        {
            return fragments.Count;
        }
    }
    public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
    {
        return new Java.Lang.String(fragmentTitles[position]);
    }
}