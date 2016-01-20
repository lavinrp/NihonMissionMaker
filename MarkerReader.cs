using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nihon_Mission_Maker
{
    /// <summary>
    /// Reads and manipulates Marker data stored in fn_getGroupMarkerStyle and defines_unitsAndGroups
    /// </summary>
    class MarkerReader
    {
        #region constructors
        /// <summary>
        /// Constructs a marker reader and automatically gathers all available information
        /// from provided marker files 
        /// </summary>
        /// <param name="groupDefineFilePath">file path to defines_unitsAndGroups.sqf</param>
        /// <param name="markerStyleFilePath">file path to fn_getGroupMarkerStyle.sqf</param>
        public MarkerReader(string groupDefineFilePath, string markerStyleFilePath)
        {
            //TODO: store full marker file text in member variables
        }

        /// <summary>
        /// Default constructor for MarkerReader
        /// Gathers no marker information.
        /// </summary>
        public MarkerReader()
        {
            //TODO: Initialize variables
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadGroupsFile (string filePath)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        public void ReadStyleFile (string filePath)
        {

        }

        protected string unitsAndGroupsFullText;
        protected string markerStyleFullText;
    }
}
