using CommunityToolkit.Maui.Views;
using Plugin.Maui.Audio;
using PM2Examen2Grupo4.Models;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

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
                    await Navigation.PushAsync(new Views.PageMap(selectedItem.latitud, selectedItem.longitud, selectedItem.descripcion)); //Se pueden mandar los parametros mediante el constructor de clase y almacenarlo en variables globales.
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
                    await Navigation.PushAsync(new Views.PageUpdate(selectedItem.latitud, selectedItem.longitud, selectedItem.descripcion, selectedItem.audioFile, selectedItem.id)); //Se pueden mandar los parametros de actualizacion mediante el constructor
                    break;

                case "Reproducir Audio":
                    StartAudio(selectedItem.audioFile);
                    await DisplayAlert("INFORMACION", "REPRODUCIR AUDIO ESTA INACTIVO POR AHORA", "CANCEL");
                    break;
            }
        }
    }

    private void StartAudio(string filePath)
    {

        mediaElement = new MediaElement
        {
            Source = filePath
        };
        mediaElement.Play();
    }
}