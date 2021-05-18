using Map_Sample.API;
using Newtonsoft.Json.Linq;
using Syncfusion.SfMaps.XForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace Map_Sample
{

    public partial class MainPage : ContentPage
    {
        private MainPageViewModel VM;
        public MainPage()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            BindingContext = VM = new MainPageViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            var fromDate = startDatePicker.Date.ToString("yyyy/MM/dd");
            var toDate = endDatePicker.Date.ToString("yyyy/MM/dd");
            await VM.ExcuteLoadCommand(fromDate, toDate);
        }


        private async void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            var fromDate = startDatePicker.Date.ToString("yyyy/MM/dd");
            var toDate = endDatePicker.Date.ToString("yyyy/MM/dd");

            //if (toDate < fromDate)
            //{
            //    await DisplayAlert("Alert", "To date can't be less than From date", "OK");
            //    return;

            //}

            await VM.ExcuteLoadCommand(fromDate, toDate);
        }

        private async void Layer_ShapeSelectionChanged(object sender, ShapeSelectedEventArgs e)
        {
            if (sender is Syncfusion.SfMaps.XForms.ShapeFileLayer && e?.Data != null && e.Data is Country country) 
            {
                var page = new DetailsPage(country.Name);
                await Navigation.PushAsync(page);

            }
                        
        }

    }

    

    
}
