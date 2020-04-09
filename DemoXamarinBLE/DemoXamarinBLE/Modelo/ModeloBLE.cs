using Plugin.BLE.Abstractions.Contracts;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DemoXamarinBLE.Modelo
{
    public class ModeloBLE : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _EstatusBLE;
        public string EstatusBLE
        {
            get
            {
                return _EstatusBLE;
            }
            set
            {
                _EstatusBLE = value;
                OnPropertyChanged("EstatusBLE");
            }
        }

        private ObservableCollection<IDevice> _ListaDispositivos;
        public ObservableCollection<IDevice> ListaDispositivos
        {
            get
            {
                return _ListaDispositivos;
            }
            set
            {
                _ListaDispositivos = value;
                OnPropertyChanged("ListaDispositivos");
            }
        }

        private IDevice _DispositivoConectado;
        public IDevice DispositivoConectado
        {
            get
            {
                return _DispositivoConectado;
            }
            set
            {
                _DispositivoConectado = value;
                OnPropertyChanged("DispositivoConectado");
            }
        }

        private ObservableCollection<IService> _ListaServicios;
        public ObservableCollection<IService> ListaServicios
        {
            get
            {
                return _ListaServicios;
            }
            set
            {
                _ListaServicios = value;
                OnPropertyChanged("ListaServicios");
            }
        }

        private IService _ServicioSeleccionado;
        public IService ServicioSeleccionado
        {
            get
            {
                return _ServicioSeleccionado;
            }
            set
            {
                _ServicioSeleccionado = value;
                OnPropertyChanged("ServicioSeleccionado");
            }
        }

        private ObservableCollection<ICharacteristic> _ListaCaracteristicas;
        public ObservableCollection<ICharacteristic> ListaCaracteristicas
        {
            get
            {
                return _ListaCaracteristicas;
            }
            set
            {
                _ListaCaracteristicas = value;
                OnPropertyChanged("ListaCaracteristicas");
            }
        }

        private ICharacteristic _CaracteristicaSeleccionada;
        public ICharacteristic CaracteristicaSeleccionada
        {
            get
            {
                return _CaracteristicaSeleccionada;
            }
            set
            {
                _CaracteristicaSeleccionada = value;
                OnPropertyChanged("CaracteristicaSeleccionada");
            }
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
