using DemoXamarinBLE.Modelo;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace DemoXamarinBLE.VistaModelo
{
    public class VistaModeloBLE : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        // modelo
        private ModeloBLE _modelo;
        public ModeloBLE modelo
        {
            get
            {
                return _modelo;
            }
            set
            {
                _modelo = value;
                OnPropertyChanged("modelo");
            }
        }

        // gestor de BLE
        private IAdapter bleAdapter;
        private IBluetoothLE bleHandler;

        public VistaModeloBLE()
        {
            // este constructor lo hace todo :)

            modelo = new ModeloBLE
            {
                EstatusBLE = "",
                ListaCaracteristicas = new System.Collections.ObjectModel.ObservableCollection<Plugin.BLE.Abstractions.Contracts.ICharacteristic>(),
                ListaDispositivos = new System.Collections.ObjectModel.ObservableCollection<Plugin.BLE.Abstractions.Contracts.IDevice>(),
                ListaServicios = new System.Collections.ObjectModel.ObservableCollection<Plugin.BLE.Abstractions.Contracts.IService>()
            };

            // obteniendo las instancias del hardware ble
            bleHandler = CrossBluetoothLE.Current;
            bleAdapter = CrossBluetoothLE.Current.Adapter;

            // configurando evento inicial del proceso de escaneo de dispositivos 
            // y cambios de estado
            bleHandler.StateChanged += (sender, args) =>
            {
                modelo.EstatusBLE = $"Estado del bluetooth: {args.NewState}";
            };

            bleAdapter.ScanMode = ScanMode.LowPower; // se establece que el escaneo de advertising se optimiza para bajo consumo 
            bleAdapter.ScanTimeout = 10000; // tiempo de busqueda de dispositivos en advertising
            bleAdapter.ScanTimeoutElapsed += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("El escaneo se ha terminado");
                modelo.EstatusBLE = $"Estado del bluetooth: {bleHandler.State}";
            };

            // cuando se 'descubre' un dispositivo
            // se ejecuta cuando BLE encuentra un dispositivo que esta en advertising
            bleAdapter.DeviceDiscovered += (sender, args) =>
            {
                System.Diagnostics.Debug.WriteLine("Se ha descubierto un dispositivo");
                IDevice dispositivoDescubierto = args.Device;



                // buscando en la lista de dispositivos en memoria si ya existe
                //List<IDevice> lstDispositivoRepetido = (from disps in modelo.ListaDispositivos
                //                                        where disps.Name == dispositivoDescubierto.Name
                //                                        select disps).ToList();

                //// no hay repetidos
                //if (!lstDispositivoRepetido.Any())
                //{
                modelo.ListaDispositivos.Add(dispositivoDescubierto);
                //}
            };

            modelo.EstatusBLE = "Listo...";
        }

        private Command _CmdIniciaEscaneo;
        public Command CmdIniciaEscaneo
        {
            get
            {
                if (_CmdIniciaEscaneo == null)
                {
                    _CmdIniciaEscaneo = new Command(async () =>
                    {
                        try
                        {
                            if (!bleAdapter.IsScanning)
                            {
                                System.Diagnostics.Debug.WriteLine("Comienza el escaneo");
                                modelo.EstatusBLE = "Escanenado por dispositivos BLE";
                                modelo.ListaDispositivos.Clear();
                                await bleAdapter.StartScanningForDevicesAsync();
                            }
                        }
                        catch (System.Exception ex)
                        {
                            await App.Current.MainPage.DisplayAlert("Demo BLE", ex.Message, "Ok");
                        }
                    });
                }

                return _CmdIniciaEscaneo;
            }
        }

        // conecta el dispositivo
        private Command _CmdConectaDispositivo;
        public Command CmdConectaDispositivo
        {
            get
            {
                if (_CmdConectaDispositivo == null)
                {
                    _CmdConectaDispositivo = new Command(async () =>
                    {
                        if (bleAdapter.IsScanning)
                        {
                            await bleAdapter.StopScanningForDevicesAsync();
                        }
                        modelo.EstatusBLE = "Conectando con el periférico...";

                        await bleAdapter.ConnectToDeviceAsync(modelo.DispositivoConectado);

                        modelo.ListaServicios.Clear();

                        foreach (IService servicio in await modelo.DispositivoConectado.GetServicesAsync())
                        {
                            modelo.ListaServicios.Add(servicio);
                        }

                        // pasar a la siguiente pagina
                        modelo.EstatusBLE = $"Estado del bluetooth: {bleHandler.State}";

                        await ((NavigationPage)App.Current.MainPage).PushAsync(new Vista.VistaServicios(App.vmBle));
                    });
                }

                return _CmdConectaDispositivo;
            }
        }

        private Command _CmdSeleccionaServicio;
        public Command CmdSeleccionaServicio
        {
            get
            {
                if (_CmdSeleccionaServicio == null)
                {
                    _CmdSeleccionaServicio = new Command(async () =>
                    {
                        modelo.ListaCaracteristicas.Clear();
                        foreach (ICharacteristic characteristic in await modelo.ServicioSeleccionado.GetCharacteristicsAsync())
                        {
                            modelo.ListaCaracteristicas.Add(characteristic);
                        }

                        // cambiar pagina
                        await ((NavigationPage)App.Current.MainPage).PushAsync(new Vista.VistaCaracteristicas(App.vmBle));
                    });
                }

                return _CmdSeleccionaServicio;
            }
        }

        private Command _CmdInteractuarConCaracteristica;
        public Command CmdInteractuarConCaracteristica
        {
            get
            {
                if (_CmdInteractuarConCaracteristica == null)
                {
                    _CmdInteractuarConCaracteristica = new Command(async () =>
                    {
                        // imprimir las capacidades de la caracteristica
                        System.Diagnostics.Debug.WriteLine($"Capcidades de la caracteristica: Lectura {modelo.CaracteristicaSeleccionada.CanRead}, Escritura {modelo.CaracteristicaSeleccionada.CanWrite}, Actualizacion {modelo.CaracteristicaSeleccionada.CanWrite}");

                        if (modelo.CaracteristicaSeleccionada.CanRead)
                        {
                            byte[] datos = await modelo.CaracteristicaSeleccionada.ReadAsync();

                            System.Diagnostics.Debug.WriteLine($"Datos: {Encoding.UTF8.GetString(datos)}");
                        }
                    });
                }

                return _CmdInteractuarConCaracteristica;
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}