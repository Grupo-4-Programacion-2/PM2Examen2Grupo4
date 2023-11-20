using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Net.NetworkInformation;

namespace PM2Examen2Grupo4.Views;
public partial class PageMap : ContentPage
{
    string descripcion;
    Double lat, lgn;
	public PageMap( Double a, Double b, string c)
	{
		InitializeComponent();
        lat = a;
        lgn = b;
        descripcion = c;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var geolocationRequest = new GeolocationRequest(GeolocationAccuracy.High, TimeSpan.FromSeconds(5));
        var location = await Geolocation.GetLocationAsync(geolocationRequest);

        var lcal = new Location(lat, lgn);
        map.MoveToRegion(MapSpan.FromCenterAndRadius(lcal, Distance.FromKilometers(2.5)));
        LoadMap();
    }

	public void LoadMap()
	{

        var pin = new Pin
        {
            Label = "DIRECCION",
            Address = $"{descripcion}",
            Location = new Location(lat, lgn)
        };

        var circle = new Circle
        {
            Center = new Location(lat, lgn),
            Radius = new Distance(250),
            StrokeColor = Color.FromArgb("#88FF0000"),
            StrokeWidth = 8,
            FillColor = Color.FromArgb("#88FFC0CB")
        };
        map.MapElements.Add(circle);
        map.Pins.Add(pin);
        
    }
}