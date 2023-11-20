using CommunityToolkit.Maui.Views;
using Plugin.Maui.Audio;
using PM2Examen2Grupo4.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace PM2Examen2Grupo4
{

    public partial class MainPage : ContentPage
    {
        Sitios sitios;
        Contactos contactos;

        private readonly IAudioRecorder _audioRecorder;
        private bool isRecording = false;
        private bool isPaused = false;
        private MediaElement mediaElement;

        public string pathaudio, filename;
        public MainPage()
        {
            InitializeComponent();

            _audioRecorder = AudioManager.Current.CreateRecorder();
            this.Appearing += OnPageAppearing;
        }

        protected void OnAppearing()
        {
            base.OnAppearing();
            VerificarUbicacion();
        }

        private async void OnPageAppearing(object sender, EventArgs e)
        {
            var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

            //Validar para colocar la geolocalizacion 
            if (status == PermissionStatus.Granted)
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Default);
                var location = await Geolocation.GetLocationAsync(request);

                try
                {
                    if (location != null)
                    {
                        double latitude = location.Latitude;
                        double longitude = location.Longitude;

                        // Asignar los valores a los campos Entry
                        _lat.Text = latitude.ToString();
                        _lgn.Text = longitude.ToString();
                    }
                    else
                    {
                        // Manejar el caso en el que no se pudo obtener la ubicación
                        await DisplayAlert("Aviso", "No se pudo obtener la ubicación actual.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    // Manejar excepciones, por ejemplo, permisos denegados
                    await DisplayAlert($"Aviso", "Error al obtener la ubicación.", "OK");
                }
            }

            //Validar que tenga conexión a internet
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                // Hay conexión a Internet
                await DisplayAlert($"Aviso", "Hay conexión a Internet.", "OK");
            }
            else
            {
                // No hay conexión a Internet
                await DisplayAlert($"Error", "No hay conexión a Internet.", "OK");
            }
        }

        private void CheckInternetConnection()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
            {
                // Hay conexión a Internet
                DisplayAlert("Conexión", "Hay conexión a Internet.", "OK");
            }
            else
            {
                // No hay conexión a Internet
                DisplayAlert("Conexión", "No hay conexión a Internet.", "OK");
            }
        }

        private async void VerificarUbicacion()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                {
                    // Permiso de ubicación no concedido, solicitar permiso
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                    if (status == PermissionStatus.Granted)
                    {
                        // Permiso concedido, pero el servicio de ubicación podría estar desactivado
                        var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(1)));

                        if (location == null || (location.Latitude == 0 && location.Longitude == 0))
                        {
                            // El servicio de ubicación no está activado o la ubicación no es válida, mostrar mensaje de validación
                            await DisplayAlert("Aviso", "El servicio de ubicación está desactivado o no se pudo obtener una ubicación válida. Actívalo en la configuración del dispositivo.", "OK");
                        }
                    }
                    else
                    {
                        // Permiso de ubicación no concedido, mostrar mensaje de validación
                        await DisplayAlert("Aviso", "El permiso de ubicación es necesario para acceder a la ubicación.", "OK");
                    }
                }
                else
                {
                    // Permiso ya concedido, verificar si el servicio de ubicación está activado
                    var location = await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(1)));

                    if (location == null)
                    {
                        // El servicio de ubicación no está activado o la ubicación no está disponible, mostrar mensaje de validación
                        await DisplayAlert("Aviso", "El servicio de ubicación está desactivado o no se pudo obtener una ubicación válida. Actívalo en la configuración del dispositivo.", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar la ubicación: {ex.Message}");
                // Manejar el error según tus necesidades
            }
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
            if (string.IsNullOrWhiteSpace(_lat.Text) || string.IsNullOrWhiteSpace(_lgn.Text) || string.IsNullOrWhiteSpace(_des.Text))
            {
                await DisplayAlert("Aviso", "Los campos de ubicación y descripción no pueden estar vacíos. Por favor, completa la información antes de grabar.", "OK");
                return;
            }

            sitios = new Sitios
            {
                descripcion = _des.Text,
                latitud = Convert.ToDouble(_lat.Text),
                longitud = Convert.ToDouble(_lgn.Text),
                audioFile = pathaudio,
                firmaDigital = imagenBytes
            };


            Console.WriteLine(sitios.descripcion);
            Console.WriteLine(sitios.longitud);
            Console.WriteLine(sitios.latitud);
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

                if (permiss1 != PermissionStatus.Granted)
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

        private async void detener_Clicked(object sender, EventArgs e)
        {
            if (isRecording)
            {
                var recordedAudio = await _audioRecorder.StopAsync();

                if (recordedAudio != null)
                {
                    try
                    {
                        filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{DateTime.Now:ddMMyyyymmss}_VoiceLocation.wav");

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

        private void clear()
        {
            drawingView.Clear();
        }

    }
}