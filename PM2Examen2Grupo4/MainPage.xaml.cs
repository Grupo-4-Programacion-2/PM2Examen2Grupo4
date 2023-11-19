using CommunityToolkit.Maui.Views;
using Plugin.Maui.Audio;
using PM2Examen2Grupo4.Models;
using System;

namespace PM2Examen2Grupo4
{
    public partial class MainPage : ContentPage
    {
        Sitios sitios;

        private readonly IAudioRecorder _audioRecorder;
        private bool isRecording = false;
        private MediaElement mediaElement;

        public string pathaudio, filename;
        public MainPage()
        {
            InitializeComponent();

            _audioRecorder = AudioManager.Current.CreateRecorder();
        }

        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
            byte[] imagenBytes = await getSignatureToImage();

            sitios = new Sitios
            {
                descripcionAudio = _des.Text,
                lat = Convert.ToDouble(_lat.Text),
                lgn = Convert.ToDouble(_lgn.Text),
                audio = null,
                imageSignature = null
            };

            Console.WriteLine(sitios.lat);
            Console.WriteLine(sitios.lgn);
            Console.WriteLine(sitios.descripcionAudio);
            Console.WriteLine(sitios.audio);
            Console.WriteLine(sitios.imageSignature);

            Models.Msg msg = await Controllers.SitiosController.CreateEmple(sitios);

            if (msg != null)
            {
                await DisplayAlert("Aviso", msg.message.ToString(), "OK");
            }


        }

        private async void btnLista_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.PageList());
        }

        private async void recording_Clicked(object sender, EventArgs e)
        {

            if (!isRecording)
            {
                var permiss1 = await Permissions.RequestAsync<Permissions.Microphone>();
                var permiss2 = await Permissions.RequestAsync<Permissions.StorageRead>();
                var permiss3 = await Permissions.RequestAsync<Permissions.StorageWrite>();

                if (permiss1 != PermissionStatus.Granted || permiss2 != PermissionStatus.Granted || permiss3 != PermissionStatus.Granted)
                {
                    return;
                }

                if (string.IsNullOrEmpty(_des.Text))
                {
                    await DisplayAlert("Message", "Descripcion Vacia", "Ok");
                    return;
                }

                await _audioRecorder.StartAsync();
                isRecording = true;
                Console.WriteLine("Iniciando grabación...");
            }
            else
            {
                var recordedAudio = await _audioRecorder.StopAsync();

                if (recordedAudio != null)
                {
                    try
                    {
                        filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), DateTime.Now.ToString("ddMMyyyymmss") + "_VoiceLocation.wav");

                        using (var fileStorage = new FileStream(filename, FileMode.Create, FileAccess.Write))
                        {
                            recordedAudio.GetAudioStream().CopyTo(fileStorage);
                        }

                        pathaudio = filename;

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                        await DisplayAlert("Error", "Ocurrió un error al procesar la grabación.", "Ok");
                    }
                }
                else
                {
                    await DisplayAlert("Error", "La grabación de audio ha fallado.", "Ok");
                }

                isRecording = false;
                Console.WriteLine("Deteniendo grabación y guardando el audio...");
            }
        }


        private async Task<byte[]> getSignatureToImage()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                Stream imagenStream = await ((DrawingView)this.FindByName<DrawingView>("drawingView")).GetImageStream(200, 200);
                await imagenStream.CopyToAsync(stream);
                return stream.ToArray();
            }
        }


        private void detener_Clicked(object sender, EventArgs e)
        {
            if (mediaElement != null)
            {
                mediaElement.Pause();
            }
        }

        private void clear()
        {
            //_lat.Text = ""; 
            //_lgn.Text = "";
            drawingView.Clear();
        }

    }
}