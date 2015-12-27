using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupMakerNoGui
{
    class Unit
    {
        ///////////////
        //constructors
        //////////////
 
        //default constructor
        public Unit()
        {
            id = 0;
            side = "WEST";
            vehicleName = "B_Soldier_TL_F";
            UnitName = "A1 FTL";
            rank = 0;
            skill = 0;
            leader = false;
            tagedUnit = false;
            tag = "";
        }
        public Unit(string side)
        {
            id = 0;
            this.side = side;
            vehicleName = "B_Soldier_TL_F";
            UnitName = "A1 FTL";
            rank = 0;
            skill = 0;
            leader = false;
            tagedUnit = false;
            tag = "";
        }


        ///////////////////
        //member functions
        //////////////////

        //TODO(Ryan Lavin): fill stub for units GetOutput
        public string GetOutput()
        {
            return "";
        }

        //////////////////////
        //getters and setters
        /////////////////////

        //getter and setter for id
        public int ID 
        {
            get 
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        //getter and setter for side
        public string Side
        {
            get
            {
                return side;
            }

            set
            {
                side = value;
            }
        }

        //getter and setter for vehicleName
        public string VehicleName
        {
            get
            {
                return vehicleName;
            }

            set
            {
                vehicleName = value;
            }
        }

        //getter and setter for unitName
        public string UnitName
        {
            get
            {
                return unitName;
            }
            set
            {
                unitName = value;
            }
        }

        //getter and setter for skill
        public double Skill
        {
            get
            {
                return skill;
            }

            set
            {
                skill = value;
            }
        }

        //getter and setter for the rank of the unit
        public int Rank
        {
            get
            {
                return rank;
            }

            set
            {
                rank = value;
            }
        }

        //getter and setter for leader
        public bool Leader
        {
            get
            {
                return leader;
            }
            set
            {
                leader = value;
            }
        }

        //getter and setter for tagedUnit
        public bool TagedUnit
        {
            get
            {
                return tagedUnit;
            }

            set
            {
                tagedUnit = value;
            }
        }

        //getter and setter for tag
        public string Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }


        ///////////////////
        //member variables
        //////////////////

        //position of unit in group
        int id;

        //faction the unit belongs to
        string side;

        //in engine unit name
        string vehicleName;

        //makes unit playable
        const string player = "player=PLAY CDG";

        //name of the unit seen when sloting
        string unitName;

        //skill of the unit
        double skill;

        //rank of the unit
        int rank;
        
        //determines if the unit is the leader of its group
        bool leader;

        //determines if the unit gets a map marker
        bool tagedUnit;

        //string of the units map marker
        string tag;


    }
}
