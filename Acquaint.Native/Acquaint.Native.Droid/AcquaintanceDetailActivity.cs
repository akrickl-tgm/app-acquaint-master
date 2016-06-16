using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acquaint.Data;
using Acquaint.Util;
using Android.App;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Locations;
using Android.OS;
using Android.Support.V7.App;
using Android.Transitions;
using Android.Views;
using Android.Widget;
using FFImageLoading;
using FFImageLoading.Transformations;
using FFImageLoading.Views;
using Plugin.ExternalMaps;
using Plugin.ExternalMaps.Abstractions;
using Plugin.Messaging;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace Acquaint.Native.Droid
{
	/// <summary>
	/// Acquaintance detail activity.
	/// </summary>
	[Activity]			
	public class AcquaintanceDetailActivity : AppCompatActivity, IOnMapReadyCallback
	{

        Animal _Animal = new Animal();

        View _ContentLayout;
        /*
		readonly IDataSource<Acquaintance> _AcquaintanceDataSource;
		Acquaintance _Acquaintance;
		
		//ImageView _GetDirectionsActionImageView;
		//LatLng _GeocodedLocation;

		public AcquaintanceDetailActivity()
		{
			_AcquaintanceDataSource = new AcquaintanceDataSource();
		}

        */
        protected override async void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			var acquaintanceDetailLayout = LayoutInflater.Inflate(Resource.Layout.AcquaintanceDetail, null);

			SetContentView(acquaintanceDetailLayout);

			SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));

			// ensure that the system bar color gets drawn
			Window.AddFlags(WindowManagerFlags.DrawsSystemBarBackgrounds);

			// enable the back button in the action bar
			//SupportActionBar.SetDisplayHomeAsUpEnabled(true);
			//SupportActionBar.SetHomeButtonEnabled(true);

			// extract the acquaintance id fomr the intent
			var acquaintanceId = Intent.GetStringExtra(GetString(Resource.String.acquaintanceDetailIntentKey));

            // fetch the acquaintance based on the id

            //Database auslesen 
           // DatabaseConnect dbcon = new DatabaseConnect();
            //dbcon.openConnection();
            //int id = Int32.Parse(acquaintanceId);
            // _Animal = dbcon.getItem(id);
            
           
            foreach (Animal item in AnimalCollectionAdapter.animals)
            {
                if (item.id == Int32.Parse(acquaintanceId))
                    _Animal = item;
            }

            //dbcon.closeConnection(); 

			// set the activity title and action bar title
			Title = SupportActionBar.Title = _Animal.name;

			SetupViews(acquaintanceDetailLayout, savedInstanceState);

			SetupAnimations();
		}

		void SetupViews(View layout, Bundle savedInstanceState)
		{
            //TODO bindings
			// inflate the content layout
			_ContentLayout = layout.FindViewById<LinearLayout>(Resource.Id.contentLayout);

			// inflate and set the profile image view
			var profilePhotoImageView = _ContentLayout.FindViewById<ImageViewAsync>(Resource.Id.profilePhotoImageView);

			if (profilePhotoImageView != null)
			{
				// use FFImageLoading library to load an android asset image into the imageview
				ImageService.LoadFileFromApplicationBundle(_Animal.PhotoURL).Transform(new CircleTransformation()).Into(profilePhotoImageView);

				// use FFImageLoading library to asynchonously load the image into the imageview
				// ImageService.LoadUrl(_Acquaintance.PhotoUrl).Transform(new CircleTransformation()).Into(profilePhotoImageView);
			}

			// infliate and set the name text view
			_ContentLayout.InflateAndBindTextView(Resource.Id.nameTextView, _Animal.name);

			// inflate and set the company name text view
			_ContentLayout.InflateAndBindTextView(Resource.Id.companyTextView, "Description: " +_Animal.description);

			// inflate and set the job title text view
			_ContentLayout.InflateAndBindTextView(Resource.Id.jobTitleTextView, "Kingdom: " +_Animal.kingdom);

			_ContentLayout.InflateAndBindTextView(Resource.Id.streetAddressTextView, "Origin: "+ _Animal.origin);

			_ContentLayout.InflateAndBindTextView(Resource.Id.cityTextView, "Family: "+_Animal.family);

			_ContentLayout.InflateAndBindTextView(Resource.Id.statePostalTextView, "Genus: "+_Animal.genus);

			_ContentLayout.InflateAndBindTextView(Resource.Id.phoneTextView, "Class: "+_Animal.classe);

			_ContentLayout.InflateAndBindTextView(Resource.Id.emailTextView, "Order: "+_Animal.order);

            //google maps ? 
			//_GetDirectionsActionImageView = _ContentLayout.InflateAndBindLocalImageViewByResource(Resource.Id.getDirectionsActionImageView, Resource.Mipmap.directions);
			//_GetDirectionsActionImageView.Visibility = ViewStates.Invisible;
			//_GetDirectionsActionImageView.Click += async (sender, e) => {
			//	if (_GeocodedLocation != null)
					// we're using the External Maps plugin from James Montemagno here (included as a NuGet)
			//		await CrossExternalMaps.Current.NavigateTo(_Acquaintance.DisplayName, _GeocodedLocation.Latitude, _GeocodedLocation.Longitude, NavigationType.Driving);
			//};

            /*
			var messageActionImageView = _ContentLayout.InflateAndBindLocalImageViewByResource(Resource.Id.messageActionImageView, Resource.Mipmap.message);
			messageActionImageView.Click += (sender, e) => {
				// we're using the Messaging plugin from Carel Lotz here (included as a NuGet)
				var smsTask = MessagingPlugin.SmsMessenger;
				if (smsTask.CanSendSms)
					smsTask.SendSms(_Acquaintance.Phone.SanitizePhoneNumber(), "");
			};

			var phoneActionImageView = _ContentLayout.InflateAndBindLocalImageViewByResource(Resource.Id.phoneActionImageView, Resource.Mipmap.phone);
			phoneActionImageView.Click += (sender, e) => {
				// we're using the Messaging plugin from Carel Lotz here (included as a NuGet)
				var phoneCallTask = MessagingPlugin.PhoneDialer;
				if (phoneCallTask.CanMakePhoneCall)
					phoneCallTask.MakePhoneCall(_Acquaintance.Phone.SanitizePhoneNumber());
			};

    */

            /*
			var emailActionImageView = _ContentLayout.InflateAndBindLocalImageViewByResource(Resource.Id.emailActionImageView, Resource.Mipmap.email);
			emailActionImageView.Click += (sender, e) => {
				// we're using the Messaging plugin from Carel Lotz here (included as a NuGet)
				var emailTask = MessagingPlugin.EmailMessenger;
				if (emailTask.CanSendEmail)
					emailTask.SendEmail(_Acquaintance.Email, "");
			};

			// inflate the map view
			var mapview = FindViewById<MapView>(Resource.Id.map);

			// create the map view with the context
			mapview.OnCreate(savedInstanceState);

			// get the map, which calls the OnMapReady() method below (by virtue of the IOnMapReadyCallback interface that this class implements)
			mapview.GetMapAsync(this);
            */
		}

		void SetupAnimations()
		{
			
			if (Build.VERSION.SdkInt >= BuildVersionCodes.Lollipop)
			{
				var enterTransition = TransitionInflater.From(this).InflateTransition(Resource.Transition.acquaintanceDetailActivityEnter);

				Window.SharedElementEnterTransition = enterTransition;
			}
		}

		#region Implementations IOnMapReadyCallback

		public async void OnMapReady(GoogleMap googleMap)
		{
            /*
			// disable the compass on the map
			googleMap.UiSettings.CompassEnabled = false;

			// disable the my location button
			googleMap.UiSettings.MyLocationButtonEnabled = false;

			// disable the map toolbar
			googleMap.UiSettings.MapToolbarEnabled = false;

			// prevent tap gestures (this will automatically open the external map application, which we don't want in this case)
			googleMap.MapClick += (sender, e) => {
				// an empty delegate, to prevent click events
			};

			// attempt to get the lat and lon for the address
			_GeocodedLocation = await GetPositionAsync();

			if (_GeocodedLocation != null)
			{
				// because we now have coordinates, show the get directions action image view, and wire up its click handler
				_GetDirectionsActionImageView.Visibility = ViewStates.Visible;

				// initialze the map
				MapsInitializer.Initialize(this);

				// display the map region that contains the point. (the zoom level has been defined on the map layout in AcquaintanceDetail.axml)
				googleMap.MoveCamera(CameraUpdateFactory.NewLatLng(_GeocodedLocation));

				// create a new pin
				var marker = new MarkerOptions();

				// set the pin's position
				marker.SetPosition(new LatLng(_GeocodedLocation.Latitude, _GeocodedLocation.Longitude));

				// add the pin to the map
				googleMap.AddMarker(marker);
                */
			}
		}

		#endregion


}

