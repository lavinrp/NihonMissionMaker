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

        public Dictionary<string, Marker> GroupMarkerPairs;
        public Dictionary<string, Marker> UnitMarkerPairs;

        protected string unitsAndGroupsFullText;
        protected string markerStyleFullText;
    }
}
