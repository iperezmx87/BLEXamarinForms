using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DemoXamarinBLE
{
    public partial class App : Application
    {
        public static VistaModelo.VistaModeloBLE vmBle;

        public App()
        {
            InitializeComponent();

            vmBle = new VistaModelo.VistaModeloBLE();

            MainPage = new NavigationPage(new Vista.MainPage(vmBle));
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
