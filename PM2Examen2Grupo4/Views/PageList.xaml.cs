using CommunityToolkit.Maui.Views;
using PM2Examen2Grupo4.Models;
using System.Collections.ObjectModel;

namespace PM2Examen2Grupo4.Views;

public partial class PageList : ContentPage
{

    //private ObservableCollection<Sitios> _sitios;
    private MediaElement mediaElement;
    public PageList()
    {
        InitializeComponent();
    }

    private async void _list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        var selectedItem = e.SelectedItem as Sitios; //Para cuando se seleccione un elemento de la lista.


        if (selectedItem != null)
        {
            string action = await DisplayActionSheet("¿Que Quieres Hacer?", "Actualizar Informacion", "Ir Mapa", "Eliminar");

            switch (action)
            {
                case "Ir Mapa":
                    await Navigation.PushAsync(new Views.PageMap()); ///Se pueden mandar los parametros mediante el constructor de clase y almacenarlo en variables globales.
                    break;

                case "Eliminar":
                    await DisplayAlert("INFORMACION", "ELIMINAR INACTIVO POR AHORA", "CANCEL");
                    break;

                case "Actualizar Informacion":
                    await Navigation.PushAsync(new Views.PageMap()); //Se pueden mandar los parametros de actualizacion mediante el constructor
                    break;
            }
        }
    }

    private void StartAudio(string filePath)
    {
        if (mediaElement != null)
        {
            mediaElement.Stop();
            (Content as StackLayout)?.Children.Remove(mediaElement);
        }

        mediaElement = new MediaElement
        {
            Source = filePath,
            ShouldAutoPlay = true
        };

        (Content as StackLayout)?.Children.Add(mediaElement);
    }
}