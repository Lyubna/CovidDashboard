using Map_Sample.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Map_Sample
{
    public partial class DetailsPage : ContentPage
    {
        DetailsViewModel VM;
        public DetailsPage(string countryName)
        {
            InitializeComponent();
            BindingContext = VM = new DetailsViewModel();
            VM.CountryName = countryName;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await VM.LoadPageData();
        }

    }
}