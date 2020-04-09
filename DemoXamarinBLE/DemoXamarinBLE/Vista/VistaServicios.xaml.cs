using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoXamarinBLE.Vista
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VistaServicios : ContentPage
    {
        public VistaServicios()
        {
            InitializeComponent();
        }

        public VistaServicios(VistaModelo.VistaModeloBLE vmBle)
        {
            InitializeComponent();
            BindingContext = vmBle;
        }
    }
}