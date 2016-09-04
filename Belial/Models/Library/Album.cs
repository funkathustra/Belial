using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belial.Models.Library
{
    public class Album
    {
        public Artist AlbumArtist { get; set; }
        public string Name { get; set; }
        public string ImageSource { get; set;  }
        public int Year { get; set; }
        public DateTime DateImported { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
