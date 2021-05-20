using Map_Sample.API;
using Map_Sample.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Map_Sample.ViewModels
{
    public class DetailsViewModel: INotifyPropertyChanged
    {        
        private string _totalResults;
        public string TotalResults 
        {
            get { return _totalResults; }
            set 
            {
                if (_totalResults != value)
                {
                    _totalResults = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TotalResults"));
                    }
                }
            }
        }

        private ObservableCollection<Article> _articles;
        public ObservableCollection<Article> Articles
        {
            get { return _articles; }
            set { _articles = value; }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
        public string CountryName { get; set; }
        public DetailsViewModel() 
        {
            Articles = new ObservableCollection<Article>();
        }

        public async Task LoadPageData() 
        {
            Articles.Clear();

            var countrycodeResult = await CovidDashboardAPI.GetCountryCode(CountryName);
            var countryCode = countrycodeResult != null? (string)countrycodeResult.alpha2Code : null;

            if (countryCode != null)
            {
                var countryData = await CovidDashboardAPI.GetCountryData(countryCode);
                if (countryData != null && (string)countryData.status == "ok")
                {
                    TotalResults = (string)countryData.totalResults;
                    var articles = countryData.articles.ToObject<List<JObject>>();
                    foreach (var item in articles)
                    {
                        Articles.Add(Article.FromDto(item));
                    }
                }
                else 
                    TotalResults = "No results";
            }
            else
                TotalResults = "No country code for this item";

        }
    }
}
