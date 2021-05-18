using Map_Sample.API;
using Map_Sample.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            set { countries = value; }
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

                var countriesBasedOnDate = countriesList.dates;
                var list = JsonConvert.DeserializeObject<List<dynamic>>(countriesBasedOnDate);
                if (countriesBasedOnDate != null) 
                {
                    //var list = JsonConvert.DeserializeObject<dynamic>(countriesBasedOnDate);
                    //var firstDateValues = list[0];
                    //var lastDateValues = list[countriesBasedOnDate?.Count()-1];
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