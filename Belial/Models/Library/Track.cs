using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belial.Models.Library
{
    public class Track
    {
        public int Key { get; set; }
        public int TrackNumber { get; set; }
        public string Name { get; set; }
        public Artist Artist { get; set; }
        public Album Album { get; set; }
        public string ImageSrc { get; set; }
        public bool IsPlaying { get; set; }
        public override string ToString()
        {
            string retVal = Name;
            if (Artist != null)
                retVal += " (" + Artist.Name + ")";
            return retVal;
        }
    }
}
