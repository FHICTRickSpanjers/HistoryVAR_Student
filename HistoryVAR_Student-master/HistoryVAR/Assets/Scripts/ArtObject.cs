using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace History_VAR.Classes
{
    class ArtObject
    {
        //ID
        private int ArtID;

        //Title / Type / Creatorname / Genre
        private string ObjectTitle;
        private string ObjectType;
        private string ObjectCreator;
        private string Genre;

        //Dimension
        private decimal Length;
        private decimal Height;
        private decimal Width;

        //Details
        private int Year;
        private string PeriodInTime;
        private string Material;
        private string Description;
        private string WhereIsOriginal;
        private string Country;
        private string City;

        public ArtObject()
        {

        }


        public ArtObject(int ID, string title, string type, string genre, int year, string period, string city, string country, decimal width, decimal height, decimal length, string material, string originallocation, string description)
        {
            this.ArtID = ID;
            this.ObjectTitle = title;
            this.ObjectType = type;
            this.Genre = genre;
            this.Year = year;
            this.PeriodInTime = period;
            this.City = city;
            this.Country = country;
            this.Width = width;
            this.Height = height;
            this.Length = length;
            this.Material = material;
            this.WhereIsOriginal = originallocation;
            this.Description = description;
        }


        public int ReturnArtID()
        {
            return this.ArtID;
        }

        public string ReturnArtTitle()
        {
            return this.ObjectTitle;
        }

        public string ReturnArtType()
        {
            return this.ObjectType;
        }

        public string ReturnArtGenre()
        {
            return this.Genre;
        }

        public int ReturnArtYear()
        {
            return this.Year;
        }

        public string ReturnPeriodInTime()
        {
            return this.PeriodInTime;
        }

        public string ReturnCity()
        {
            return this.City;
        }

        public string ReturnCountry()
        {
            return this.Country;
        }

        public decimal ReturnWidth()
        {
            return this.Width;
        }

        public decimal ReturnLength()
        {
            return this.Length;
        }

        public decimal ReturnHeight()
        {
            return this.Height;
        }

        public string ReturnMaterial()
        {
            return this.Material;
        }

        public string ReturnDescription()
        {
            return this.Description;
        }

        public string ReturnOriginalLocation()
        {
            return this.WhereIsOriginal;
        }


    }
}
