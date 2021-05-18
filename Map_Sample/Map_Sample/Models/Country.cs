using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Map_Sample
{
    public class Country : INotifyPropertyChanged
    {        
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string _todayConfirmed;
        public string TodayConfirmed
        {
            get { return _todayConfirmed; }
            set {
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
            set { todayNewConfirmed = value; }
        }
        private string todayNewDeaths;
        public string TodayNewDeaths
        {
            get { return todayNewDeaths; }
            set { todayNewDeaths = value; }
        }
        private string todayDeaths;
        public string TodayDeaths
        {
            get { return todayDeaths; }
            set { todayDeaths = value; }
        }

        private bool _isMostCases;
        public bool IsMostCases
        {
            get { return _isMostCases; }
            set {
                if (_isMostCases != value)
                {
                    _isMostCases = value;
                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("IsMostCases"));
                    }
                }
            }
        }

        private bool _isLeastCases;
        public bool IsLeastCases
        {
            get { return _isLeastCases; }
            set { _isLeastCases = value; }
        }

        private bool _isMostDeaths;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsMostDeaths
        {
            get { return _isMostDeaths; }
            set { _isMostDeaths = value; }
        }

        public static Country FromDto(dynamic dto)
        {
            var country = new Country()
            {
                Name = dto.name,
                TodayConfirmed = dto.today_confirmed,
                TodayDeaths = dto.today_deaths,
                TodayNewConfirmed = dto.today_new_confirmed,
                TodayNewDeaths = dto.today_new_deaths,
            };

            return country;
        }
    }
}