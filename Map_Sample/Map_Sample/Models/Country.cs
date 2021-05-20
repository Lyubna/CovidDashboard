using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Map_Sample
{
    public class Country: INotifyPropertyChanged
    {        
        public string CountryName { get; set; }

        private double _todayConfirmed;
        public double TodayConfirmed
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
        public string TodayNewConfirmed { get; set; }
        public string TodayNewDeaths { get; set; }
        public string TodayDeaths { get; set; }
        public bool IsMostCases { get; set; }
        public bool IsLeastCases { get; set; }
        public bool IsMostDeaths { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        //public static Country FromDto(dynamic dto)
        //{
        //    var country = new Country()
        //    {
        //        CountryName = dto.name,
        //        TodayConfirmed = dto.today_confirmed,
        //        TodayDeaths = dto.today_deaths,
        //        TodayNewConfirmed = dto.today_new_confirmed,
        //        TodayNewDeaths = dto.today_new_deaths,
        //    };

        //    return country;
        //}
    }
}