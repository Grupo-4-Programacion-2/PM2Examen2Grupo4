using CommunityToolkit.Maui.Views;
using PM2Examen2Grupo4.Models;

namespace PM2Examen2Grupo4.Views;
public partial class PageUpdate : ContentPage
{

    Double  latitude, longitude;
    string descripcion, audio;
    int id;

    public PageUpdate(Double la, Double lg, string dcp, string aud, int idR)
	{
		InitializeComponent();
        _lat2.Text = la.ToString();
        _lgn2.Text = lg.ToString();
        _des2.Text = dcp.ToString();

        latitude = la;
        longitude = lg;
        descripcion = dcp;
        id = idR;
        audio = aud;
	}

    private async void btnGuardar_Clicked(object sender, EventArgs e)
    {
        byte[] imagenBytes = await getSignatureToImage();

        // Validar que la firma no esté vacía
        if (imagenBytes == null || imagenBytes.Length <= 0)
        {
            await DisplayAlert("Aviso", "El área de firma no puede estar vacía. Realiza una firma antes de enviar.", "OK");
            return;
        }

        // Validar que los campos de ubicación y descripción no estén vacíos
        if (string.IsNullOrWhiteSpace(_lat2.Text) || string.IsNullOrWhiteSpace(_lgn2.Text) || string.IsNullOrWhiteSpace(_des2.Text))
        {
            await DisplayAlert("Aviso", "Los campos de ubicación y descripción no pueden estar vacíos. Por favor, completa la información antes de grabar.", "OK");
            return;
        }

        Sitios sitios = new Sitios
        {
            id = id,
            descripcion = _des2.Text,
            latitud = latitude,
            longitud = longitude,
            firmaDigital = imagenBytes,
            audioFile = audio,
        };

        Models.Msg msg = await Controllers.SitiosController.UpdateEmple(sitios);
        
        if (msg != null)
        {
            await DisplayAlert("Aviso", msg.message.ToString(), "CONTINUAR");
        }
        await Navigation.PushAsync(new Views.PageList());
    }

    private async Task<byte[]> getSignatureToImage()
    {
        using (MemoryStream stream = new MemoryStream())
        {
            DrawingView drawingView = this.FindByName<DrawingView>("drawingView");

            if (drawingView.Lines.Count > 0)
            {
                Stream imagenStream = await ((DrawingView)this.FindByName<DrawingView>("drawingView")).GetImageStream(200, 200);
                await imagenStream.CopyToAsync(stream);
                return stream.ToArray();
            }
            else
            {
                return null;
            }

        }
    }

}