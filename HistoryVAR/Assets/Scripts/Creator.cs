using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace History_VAR.Classes
{
    class Creator
    {
        //Creator ID
        private int CreatorID;

        //Name, Gender, Bday, PlaceofBirth, Deathday, Deathplace
        private string CreatorName;
        private string CreatorGender;
        private DateTime CreatorBday;
        private string CreatorPlaceOfBirth;
        private DateTime CreatorDeathDay;
        private string CreatorPlaceOfDeath;

        //Number of objects made, most famous art, creatorgenre
        private int NumberOfArtObjects;
        private string CreatorMostFamousArt;
        private string CreatorGenre;
        
        //List of professions
        private List<string> ListofProfessions = new List<string>();

        //Description of the creator
        private string CreatorDescription;


        public Creator(int id, string name)
        {
            this.CreatorID = id;
            this.CreatorName = name;
        }

        public string GenderOfCreator
        {
            get
            {
                return this.CreatorGender;
            }
            set
            {
                this.CreatorGender = value;
            }
        }


        public DateTime BdayOfCreator
        {
            get
            {
                return this.CreatorBday;
            }
            set
            {
                this.CreatorBday = value;
            }
        }


        public string PlaceOfBirthCreator
        {
            get
            {
                return this.CreatorPlaceOfBirth;
            }
            set
            {
                this.CreatorPlaceOfBirth= value;
            }
        }

        public DateTime DeathOfCreator
        {
            get
            {
                return this.CreatorDeathDay;
            }
            set
            {
                this.CreatorDeathDay = value;
            }
        }


        public string PlaceOfDeathCreator
        {
            get
            {
                return this.CreatorPlaceOfDeath;
            }
            set
            {
                this.CreatorPlaceOfDeath = value;
            }
        }


        public int CreatorArtObjects
        {
            get
            {
                return this.NumberOfArtObjects;
            }
            set
            {
                this.NumberOfArtObjects = value;
            }
        }


        public string MostFamousArtCreator
        {
            get
            {
                return this.CreatorMostFamousArt;
            }
            set
            {
                this.CreatorMostFamousArt = value;
            }
        }

        public string GenreOfCreator
        {
            get
            {
                return this.CreatorGenre;
            }
            set
            {
                this.CreatorGenre = value;
            }
        }

        public string DescriptionOfCreator
        {
            get
            {
                return this.CreatorDescription;
            }
            set
            {
                this.CreatorDescription = value;
            }
        }

    }
}
