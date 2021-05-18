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
using Xamarin.Forms;

namespace Map_Sample
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Country> countries;
        public ObservableCollection<Country> Countries
        {
            get { return countries; }
            set
            {
                if (countries != value)
                {
                    countries = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("Countries"));
                    }
                }
            }
        }

        private ObservableCollection<Country> selectedItems = new ObservableCollection<Country>();
        public ObservableCollection<Country> SelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; }
        }
               

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
        public async Task ExcuteLoadCommand(string fromDate, string toDate)
        {
            var countriesList = await CovidDashboardAPI.GetTrackingData(fromDate, toDate);
            if (countriesList != null)
            {
                var total = countriesList.total;
                TodayConfirmed = total.today_confirmed;
                TodayDeaths = total.today_deaths;
                TodayNewConfirmed = total.today_new_confirmed;
                TodayNewDeaths = total.today_new_deaths;

                var countriesFrom = (dynamic)countriesList.dates.GetValue(fromDate.Replace('/','-')).countries;
                foreach (var x in Countries)
                {
                    var countryDataFrom = countriesFrom.GetValue(x.Name);
                    if (countryDataFrom != null)
                    {
                        x.TodayConfirmed = (string)countryDataFrom.today_confirmed;
                        x.TodayDeaths = countryDataFrom.today_deaths;
                        x.TodayNewConfirmed = countryDataFrom.today_new_confirmed;
                        x.TodayNewDeaths = countryDataFrom.today_new_deaths;
                    }

                }

                Countries.ToList().ForEach(x => x.IsMostCases = false);
                var itemMostCases = Countries.OrderByDescending(i => i.TodayConfirmed).FirstOrDefault();
                if (itemMostCases != null) itemMostCases.IsMostCases = true;
                Countries.ToList().ForEach(x => x.IsMostDeaths = false);
                var itemMostTodayDeaths = Countries.OrderByDescending(i => i.TodayDeaths).FirstOrDefault();
                if (itemMostTodayDeaths != null) itemMostCases.IsMostDeaths = true;

                var valuesTo = (dynamic)countriesList.dates.GetValue(toDate.Replace('/', '-'));
                if (valuesTo != null && !fromDate.Equals(toDate))
                {
                    var countriesTo = (dynamic)valuesTo.countries;

                    foreach (var x in Countries)
                    {
                        var countryDataTo = countriesTo.GetValue(x.Name);
                        var countryDataFrom = countriesFrom.GetValue(x.Name);
                        if (countryDataFrom != null)
                        {
                            var todayConf1 = (int)countryDataFrom.today_confirmed;
                            var todayConf2 = (int)countryDataTo.today_confirmed;
                            var diff1 = todayConf2 - todayConf1;
                            x.TodayConfirmed = diff1.ToString();
                            var diff2 = (int)countryDataFrom.today_deaths - (int)countryDataTo.today_deaths;
                            x.TodayDeaths = diff2.ToString();
                            x.TodayNewConfirmed = countryDataTo.today_new_confirmed;
                            x.TodayNewDeaths = countryDataTo.today_new_deaths;
                        }

                    }

                    Countries.ToList().ForEach(x => x.IsMostCases = false);
                    var itemMostCases1 = Countries.OrderByDescending(i => i.TodayConfirmed).FirstOrDefault();
                    if (itemMostCases1 != null) itemMostCases1.IsMostCases = true;

                    Countries.ToList().ForEach(x => x.IsMostDeaths = false);
                    var itemMostTodayDeaths1 = Countries.OrderByDescending(i => i.TodayDeaths).FirstOrDefault();
                    if (itemMostTodayDeaths != null) itemMostTodayDeaths1.IsMostDeaths = true;
                }
            }
        }
        public MainPageViewModel()
        {
            Countries = new ObservableCollection<Country>();
            Constants.countries.ToList().ForEach(x=> Countries.Add(new Country{Name = x}));
        }

    }
}