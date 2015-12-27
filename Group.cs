using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroupMakerNoGui
{
    class Group
    {

        #region constructors
        public Group()
        {
            //assign default values to the group
            side = "WEST";
            tag = "FAIL";

            units = new List<Unit>();
        }

        public Group(string side, string tag, List<Unit> units)
        {
            //assign specific values to the group
            this.side = side;
            this.tag = tag;
            this.units = units;
        }


        /// <summary>
        /// Creates a group with a given side, unitCount and tag but with default units for its faction
        /// </summary>
        /// <param name="side">Faction the group belongs to</param>
        /// <param name="unitCount">Number of units in the group</param>
        /// <param name="tag">Name on the groups map marker</param>
        /// <returns>Group with default units</returns>
        public Group(string side, int unitCount, string tag)
        {

            //create a list to store units
            List<Unit> units = new List<Unit>();

            //fill unit list with units
            for (int i = 0; i < unitCount; ++i)
            {
                Unit tempUnit = new Unit(side);
                units.Add(tempUnit);
            }

            //assign specific values to the group
            this.side = side;
            this.tag = tag;
        }

        #endregion

        #region Getters and setters 

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
        //getter and setter for unitCount
        public int UnitCount
        {
            get
            {
                return units.Count();
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

        #region methods

        /// <summary>
        /// adds a unit to the group
        /// </summary>
        /// <param name="unit"> the unit to add </param>
        public void AddUnit(Unit unit)
        {
            units.Add(unit);
        }

        /// <summary>
        /// removes the unit at the specified location from the list of units
        /// </summary>
        /// <param name="location"> the location of the unit to remove </param>
        public void RemoveUnit(int location)
        {
            units.RemoveAt(location);
        }

        //TODO(Ryan Lavin): Fill stub for Groups output
        public string GetOutput()
        {
            return "";
        }

        #endregion

        #region private member variables
        private string side;
        private int unitCount;
        private string tag;
        private List<Unit> units;
        #endregion
    }
}
