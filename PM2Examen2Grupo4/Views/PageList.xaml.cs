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

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        List<Models.Sitios> listSitios = new List<Models.Sitios>();
        listSitios = await Controllers.SitiosController.GetSitios();
        _list.ItemsSource = listSitios;

    }

    private async void _list_ItemSelected(object sender, SelectionChangedEventArgs e)
    {
        var selectedItem = e.CurrentSelection[0] as Sitios; //Para cuando se seleccione un elemento de la lista.


        if (selectedItem != null)
        {
            string action = await DisplayActionSheet("¿Que Quieres Hacer?", "CANCELAR", "", "Actualizar Informacion", "Reproducir Audio", "Ir Mapa", "Eliminar");

            switch (action)
            {
                case "Ir Mapa":
                    await Navigation.PushAsync(new Views.PageMap()); ///Se pueden mandar los parametros mediante el constructor de clase y almacenarlo en variables globales.
                    break;

                case "Eliminar":
                    Models.Msg msg = await Controllers.SitiosController.DeleteEmple(selectedItem.id);
                    if (msg != null)
                    {
                        await DisplayAlert("Aviso", msg.message.ToString(), "CONTINUAR");
                    }
                    OnAppearing(); 
                    break;

                case "Actualizar Informacion":
                    await Navigation.PushAsync(new Views.PageUpdate()); //Se pueden mandar los parametros de actualizacion mediante el constructor
                    break;

                case "Reproducir Audio":
                    await DisplayAlert("INFORMACION", "REPRODUCIR AUDIO ESTA INACTIVO POR AHORA", "CANCEL");
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