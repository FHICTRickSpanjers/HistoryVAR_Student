using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace History_VAR.Classes
{
    class Creator
    {
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


        public Creator()
        {

        }
    }
}
