using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        /// Read in the full text from the passed file. 
        /// If the file cannot be found return empty string and display error.
        /// </summary>
        /// <param name="filePath">Path to the file to be read.</param>
        /// <returns>All text contained in the passed file.</returns>
        public string ReadTextFromFile(string filePath)
        {
            try
            {
                if (File.Exists(filePath))
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string fileText = sr.ReadToEnd();
                        return fileText;
                    }
                }
                else
                {
                    throw new System.IO.IOException("File not found.");
                }
            }
            catch
            {
                MessageBox.Show(("Error reading file at: " + filePath), "Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return "";
            }
        }

        /// <summary>
        /// Populates the values of this instance of MarkerReader with values found in 
        /// the unitsAndGroupsFullText and  markerStyleFullText variables.
        /// </summary>
        public void FillValuesFromText()
        {
            //TODO: fill stub
            throw new NotImplementedException();
        }

        /// <summary>
        /// Populates the values of this instance of MarkerReader with values from the 
        /// passed UnitsAndGroupsText and markerStyleText strings.
        /// </summary>
        /// <param name="UnitsAndGroupsText">Text from a defines_unitsAndGroups.sqf file. </param>
        /// <param name="markerStyleText">Text from a fn_getGroupMarkerStyle.sqf file.</param>
        public void FillValuesFromText(ref string UnitsAndGroupsText, ref string markerStyleText)
        {
            unitsAndGroupsFullText = UnitsAndGroupsText;
            markerStyleFullText = markerStyleText;
            FillValuesFromText();
        }

        private void GetGroupsWithMarkers()
        {
            //TODO: fill stub
            throw new NotImplementedException();
        }
        private void GetUnitsWithMarkers()
        {
            //TODO: fill stub
            throw new NotImplementedException();
        }

        private void GetMarkerStyles()
        {
            //TODO: fill stub
            throw new NotImplementedException();
        }



        public Dictionary<string, Marker> GroupMarkerPairs;
        public Dictionary<string, Marker> UnitMarkerPairs;

        protected string unitsAndGroupsFullText;
        protected string markerStyleFullText;
    }
}
