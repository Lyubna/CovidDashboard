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
        public bool IsRefresh { get; set; }
        public MainPage()
        {
            InitializeComponent();
            On<Xamarin.Forms.PlatformConfiguration.iOS>().SetUseSafeArea(true);
            BindingContext = VM = new MainPageViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (!IsRefresh) 
            {
                GetData();
                IsRefresh = true;
            }
        }


        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            GetData();
        }
        private void GetData() 
        {
            var fromDate = startDatePicker.Date.ToString("yyyy/MM/dd");
            var toDate = endDatePicker.Date.ToString("yyyy/MM/dd");

            var dates = new Dates
            {
                FromDate = fromDate,
                ToDate = toDate

            };

            VM.LoadPageData.Execute(dates);

        }
       
        private async void Layer_ShapeSelectionChanged(object sender, ShapeSelectedEventArgs e)
        {
            if (sender is Syncfusion.SfMaps.XForms.ShapeFileLayer && e?.Data != null && e.Data is Country country)
            {
                var page = new DetailsPage(country.CountryName);
                page.Title = country.CountryName;
                await Navigation.PushAsync(page);
            }

        }
    }
}
