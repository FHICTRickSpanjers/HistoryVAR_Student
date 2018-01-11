using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace History_VAR.Classes
{
    class ArtImage
    {
        //ID of Image
        int Image_ID;
        //Image Filename
        private string ImageFileName;
        //Image byte array (varbinary)
        private byte[] ImageData;


        /// <summary>
        /// Constructor that gets the ID, Name and Varbinary
        /// </summary>
        /// <param name="ID">Image ID</param>
        /// <param name="filename">Image Filename</param>
        /// <param name="data">Byte array (varbinary data)</param>
        public ArtImage(int ID, string filename, byte[] data)
        {
            //setting the data
            this.ImageFileName = filename;
            this.ImageData = data;
            this.Image_ID = ID;
        }

        /// <summary>
        /// Return the name of the file
        /// </summary>
        /// <returns>String filename</returns>
        public string ReturnFileName()
        {
            //Returns string
            return this.ImageFileName;
        }

        /// <summary>
        /// Return the data of the image
        /// </summary>
        /// <returns>Returns an array of bytes</returns>
        public byte[] ReturnImageData()
        {
            //Return byte array
            return this.ImageData;
        }

        /// <summary>
        /// Return ID of the imagge
        /// </summary>
        /// <returns>int ID</returns>
        public int ReturnImageID()
        {
            //return ID
            return this.Image_ID;
        }
    }
}
