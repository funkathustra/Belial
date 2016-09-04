using Belial.Models.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belial.Messages
{
    public class NowPlayingChanged
    {
        public List<Track> NowPlaying { get; set; }
    }
}
