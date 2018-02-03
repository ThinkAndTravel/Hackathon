using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Plugin.Connectivity.Abstractions;

namespace AppTandT.BLL
{
    public class ConectPage : ContentPage
    {
        Label connectionStateLbl;
        Label connectionDetailsLbl;
        public ConectPage()
        {

            connectionStateLbl = new Label
            {
                Text = "Подключение отсутствует",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            connectionDetailsLbl = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            Content = new StackLayout
            {
                Children = { connectionStateLbl, connectionDetailsLbl }
            };

            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged; connectionStateLbl = new Label
            {
                Text = "Подключение отсутствует",
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            connectionDetailsLbl = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, typeof(Label)),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            };
            Content = new StackLayout
            {
                Children = { connectionStateLbl, connectionDetailsLbl }
            };

            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
        }

        // обробка зміни стану підключення
        private void Current_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckConnection();
        }

        // отримуємо стан підключення
        private void CheckConnection()
        {
            connectionStateLbl.IsVisible = !CrossConnectivity.Current.IsConnected;
            connectionDetailsLbl.IsVisible = CrossConnectivity.Current.IsConnected;

            if (CrossConnectivity.Current != null &&
                CrossConnectivity.Current.ConnectionTypes != null &&
                CrossConnectivity.Current.IsConnected == true)
            {
                var connectionType = CrossConnectivity.Current.ConnectionTypes.FirstOrDefault();
                connectionDetailsLbl.Text = connectionType.ToString();
            }
        }
    }
}
