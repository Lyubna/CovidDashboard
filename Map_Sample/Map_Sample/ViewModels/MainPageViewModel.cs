using Map_Sample.API;
using Map_Sample.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Map_Sample
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand LoadPageData => new Command(async(item) => await LoadData(item));
        
        public ObservableCollection<Country> Countries { get; set; }

        private string _todayConfirmed;
        public string TodayConfirmed
        {
            get { return _todayConfirmed; }
            set
            {
                if (_todayConfirmed != value)
                {
                    _todayConfirmed = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TodayConfirmed"));
                    }
                }
            }
        }
        private string todayNewConfirmed;
        public string TodayNewConfirmed
        {
            get { return todayNewConfirmed; }
            set
            {
                if (todayNewConfirmed != value)
                {
                    todayNewConfirmed = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TodayNewConfirmed"));
                    }
                }
            }
        }

        private string todayNewDeaths;
        public string TodayNewDeaths
        {
            get { return todayNewDeaths; }
            set
            {
                if (todayNewDeaths != value)
                {
                    todayNewDeaths = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TodayNewDeaths"));
                    }
                }
            }
        }

        private string todayDeaths;
        public string TodayDeaths
        {
            get { return todayDeaths; }
            set
            {
                if (todayDeaths != value)
                {
                    todayDeaths = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("TodayDeaths"));
                    }
                }
            }
        }
        private async Task LoadData(object obj)
        {
            var list = new ObservableCollection<Country>();
            if (Countries == null)
                return;
            var dates = obj as Dates;
            var countriesList = await CovidDashboardAPI.GetTrackingData(dates.FromDate, dates.ToDate);
            if (countriesList != null)
            {
                var total = countriesList.total;
                TodayConfirmed = total.today_confirmed;
                TodayDeaths = total.today_deaths;
                TodayNewConfirmed = total.today_new_confirmed;
                TodayNewDeaths = total.today_new_deaths;

                var countriesFrom = (JObject)countriesList.dates.GetValue(dates.FromDate.Replace('/', '-')).countries;
                foreach (var x in Countries)
                {
                    var countryDataFrom = countriesFrom.TryGetValue<JObject>(x.CountryName);
                    
                    if (countryDataFrom != null)
                    {
                        list.Add(new Country() 
                        {
                            CountryName = x.CountryName,
                            TodayConfirmed = (double)countryDataFrom.TryGetValue<double>("today_confirmed"),
                            TodayDeaths = (string)countryDataFrom.TryGetValue<string>("today_deaths"),
                            TodayNewConfirmed = (string)countryDataFrom.TryGetValue<string>("today_new_confirmed"),
                            TodayNewDeaths = (string)countryDataFrom.TryGetValue<string>("today_new_deaths"),

                        });                       
                    }
                }

                var valuesTo = (dynamic)countriesList.dates.GetValue(dates.ToDate.Replace('/', '-'));
                if (valuesTo != null && !dates.FromDate.Equals(dates.ToDate))
                {
                    list.Clear();
                    var countriesTo = (JObject)valuesTo.countries;

                    foreach (var x in Countries)
                    {
                        var countryDataTo = countriesTo.TryGetValue<JObject>(x.CountryName);
                        var countryDataFrom = countriesFrom.TryGetValue<JObject>(x.CountryName);
                        if (countryDataFrom != null)
                        {
                            var todayConf1 = (int)countryDataFrom.TryGetValue<int>("today_confirmed");
                            var todayConf2 = (int)countryDataTo.TryGetValue<int>("today_confirmed");
                            var diff1 = todayConf2 - todayConf1;
                            var diff2 = (int)countryDataFrom.TryGetValue<int>("today_deaths") - (int)countryDataTo.TryGetValue<int>("today_deaths");
                            list.Add(new Country()
                            {
                                CountryName = x.CountryName,
                                TodayConfirmed = Convert.ToDouble(diff1),
                                TodayDeaths = diff2.ToString(),
                                TodayNewConfirmed = countryDataTo.TryGetValue<string>("today_new_confirmed"),
                                TodayNewDeaths = countryDataTo.TryGetValue<string>("today_new_deaths")
                            });

                        }

                    }
                }

                list.ToList().ForEach(x => x.IsMostCases = false);
                var itemMostCases = list.OrderByDescending(i => i.TodayConfirmed).FirstOrDefault();
                if (itemMostCases != null) itemMostCases.IsMostCases = true;
                list.ToList().ForEach(x => x.IsMostDeaths = false);
                var itemMostTodayDeaths = list.OrderByDescending(i => i.TodayDeaths).FirstOrDefault();
                if (itemMostTodayDeaths != null) itemMostCases.IsMostDeaths = true;

                Countries = list;
            }
        }
        public MainPageViewModel()
        {
            Countries = new ObservableCollection<Country>();
            Constants.countries.ToList().ForEach(x=>Countries.Add(new Country() { CountryName = x }));
        }

    }

    public class Dates
    {
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }
}