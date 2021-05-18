using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Map_Sample
{
    public class Country
    {        
        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string cases;
        public string Cases
        {
            get { return cases; }
            set { cases = value; }
        }

        public static Country FromDto(dynamic dto)
        {
            var country = new Country()
            {
                Name = dto.name,               
            };

            return country;

        }
    }
}