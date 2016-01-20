using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nihon_Mission_Maker
{
    /// <summary>
    /// Map markers
    /// </summary>
    class Marker
    {
        /// <summary>
        /// Constructor for Marker
        /// </summary>
        /// <param name="text">Text the marker will display. Must be fewer than 20 characters</param>
        /// <param name="color">Color of marker. Must be red, green, blue, yellow, orange, black, pink, or white</param>
        /// <param name="texture">Texture describing what type of unit the marker represents. Must be fireteam, unknown, hq, 
        /// support, supportAT, recon, mortar, maint, mech, armor, air, plane, or artillery
        /// </param>
        /// <param name="width">Width of the marker (unknown units)</param>
        /// <param name="height">Height of the marker (unknown units)</param>
        public Marker(string text = "", string color = "", string texture = "", int width = 16, int height = 16)
        {
            //TODO: determine the units of the width and height of the marker
            Text = text;
            Color = color;
            Texture = texture;
            Width = width;
            Height = height;
        }

        #region accessors
        /// <summary>
        /// Text the marker will display. Must be fewer than 20 characters
        /// </summary>
        public string Text
        {
            get
            {
                return this.text;
            }
            set
            {
                //TODO: validate text input
                this.text = value;
            }
        }

        /// <summary>
        /// Color of marker. Must be red, green, blue, yellow, orange, black, pink, or white
        /// </summary>
        public string Color
        {
            get
            {
                return this.color;
            }
            set
            {
                //Validate color input
                this.color = value;
            }
        }

        /// <summary>
        /// Texture describing what type of unit the marker represents. Must be fireteam, unknown, hq, 
        /// support, supportAT, recon, mortar, maint, mech, armor, air, plane, or artillery
        /// </summary>
        public string Texture
        {
            get
            {
                return this.texture;
            }
            set
            {
                //TODO: Validate texture input
                this.texture = value;
            }
        }

        /// <summary>
        /// >Width of the marker (unknown units)
        /// </summary>
        public int Width
        {
            //TODO: determine units of width
            get
            {
                return this.width;
            }

            set
            {
                //TODO: validate width
                this.width = value;
            }
        }

        /// <summary>
        /// Height of the marker (unknown units)
        /// </summary>
        public int Height
        {
            //TODO: determine units of height
            get
            {
                return this.height;
            }
            set
            {
                //TODO: validate height
                this.height = value;
            }
        }
        #endregion

        #region protected members
        protected string text;
        protected string color;
        protected string texture;
        protected int width;
        protected int height;
        #endregion
    }
}
