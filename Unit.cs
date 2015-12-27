using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupMakerNoGui
{
    class Unit
    {

        #region Constructors

        //default constructor
        public Unit()
        {
            id = 0;
            side = "WEST";
            vehicleName = DefaultVehicleName;
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
            vehicleName = DefaultVehicleName;
            UnitName = "A1 FTL";
            rank = 0;
            skill = 0;
            leader = false;
            tagedUnit = false;
            tag = "";
        }

        #endregion

        #region Methods


        //TODO(Ryan Lavin): fill stub for units GetOutput
        public string GetOutput()
        {
            return "";
        }

        #endregion

        #region Getters and Setters

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
        #endregion

        #region private member variables

        //position of unit in group
        private int id;

        //faction the unit belongs to
        private string side;

        //in engine unit name
        private string vehicleName;

        //makes unit playable
        private const string player = "player=PLAY CDG";

        //name of the unit seen when slotting
        private string unitName;

        //skill of the unit
        private double skill;

        //rank of the unit
        private int rank;

        //determines if the unit is the leader of its group
        private bool leader;

        //determines if the unit gets a map marker
        private bool tagedUnit;

        //string of the units map marker
        private string tag;

        /// <summary>
        /// Return the default vehicle name depending on side.
        /// Default to Blue if no side.
        /// </summary>
        private string DefaultVehicleName
        {
            get
            {
                switch (side)
                {
                    case "WEST":
                        return "B_Soldier_F";
                    case "GUER":
                        return "I_Soldier_F";
                    case "EAST":
                        return "O_Soldier_F";
                    case "CIV":
                        return "C_man_1";
                    default:
                        return "B_Soldier_F";
                }
            }
        }
       
        #endregion

    }
}
