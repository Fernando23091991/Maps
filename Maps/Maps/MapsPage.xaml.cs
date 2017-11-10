using System.Collections.Generic;
using System.Threading.Tasks;
using Maps.CustomRenderer;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Maps
{
    public partial class MapsPage : ContentPage
    {
        public MapsPage()
        {
            InitializeComponent();
            PermissionAsync();
        }

        public async void PermissionAsync()
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
            if (status == PermissionStatus.Granted)
            {
                LoadMap();
            }
            else
            {
                if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Location))
                {
                    await DisplayAlert("Need location", "Gunna need that location", "OK");
                }

                var results = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Location);

                if (results.ContainsKey(Permission.Location))
                {
                    status = results[Permission.Location];
                }

                if (status == PermissionStatus.Granted)
                {
                    LoadMap();
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Location Denied", "Can not continue, try again.", "OK");
                }
            }          
        }

        public void LoadMap()
        {
            var map = new ExtendedMap();
            map.IsShowingUser = true;
            map.HeightRequest = 100;
            map.WidthRequest = 960;
            map.VerticalOptions = LayoutOptions.FillAndExpand;

            var stack = new StackLayout { Spacing = 0 };
            stack.Children.Add(map);
            Content = stack;

            var pin = new ExtendedPin();
            pin.Type = PinType.Place;
            pin.Position = new Position(37.79752, -122.40183);
            pin.Label = "Xamarin San Francisco Office";
            pin.Address = "394 Pacific Ave, San Francisco CA";
            pin.Id = "Xamarin";
            pin.Url = "http://xamarin.com/about/";

            map.CustomPins = new List<ExtendedPin> { pin };
            map.Pins.Add(pin);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(37.79752, -122.40183), Distance.FromMiles(1.0)));
        }
    }
}
