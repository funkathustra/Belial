using Belial.Models.Library;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Belial.Services.MediaCenterServices
{
    public class LibraryService
    {
        public static LibraryService Instance { get; }
        static LibraryService()
        {
            // implement singleton pattern
            Instance = Instance ?? new LibraryService();
        }

        public SortedDictionary<string, Artist> Artists { get; set; }
        public SortedDictionary<string, Album> Albums { get; set; }
        public SortedDictionary<int, Track> Tracks { get; set; }

        public LibraryService()
        {
            Artists = new SortedDictionary<string, Artist>();
            Albums = new SortedDictionary<string, Album>();
            Tracks = new SortedDictionary<int, Track>();
        }

        DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public void ParseXML(Stream XML)
        {
            var serializer = new XmlSerializer(typeof(Response));
            Response data = (Response)serializer.Deserialize(XML);
            foreach (var responseItem in data.Items)
            {
                Track newTrack = new Track();
                
                // Much faster than stuffing key/values in a dictionary.
                foreach (var item in responseItem.Fields)
                {
                    switch (item.Name)
                    {
                        case "Key":
                            newTrack.Key = Int32.Parse(item.Value);
                            break;
                        case "Track #":
                            newTrack.TrackNumber = Int32.Parse(item.Value);
                            break;
                        case "Name":
                            newTrack.Name = item.Value;
                            break;
                        case "Artist":
                            if (!Artists.ContainsKey(item.Value))
                                Artists.Add(item.Value, new Artist() { Name = item.Value });
                            newTrack.Artist = Artists[item.Value];
                            break;
                        case "Album":
                            if (!Albums.ContainsKey(item.Value))
                                Albums.Add(item.Value, new Album() {
                                    Name = item.Value,
                                    ImageSource =string.Format("http://{0}:{1}/MCWS/v1/File/GetImage?File={2}", McwsService.Instance.ServerIp, McwsService.Instance.ServerPort, newTrack.Key)
                                });
                            newTrack.Album = Albums[item.Value];
                            break;
                        case "Album Artist":
                            if (!Artists.ContainsKey(item.Value))
                                Artists.Add(item.Value, new Artist() { Name = item.Value });
                            if (newTrack.Album != null)
                                newTrack.Album.AlbumArtist = Artists[item.Value];
                            break;
                        case "Date":
                            if (newTrack.Album != null)
                                newTrack.Album.Year = (int)Math.Round(Double.Parse(item.Value) / 365.0 + 1899);
                            break;
                        case "Date Imported":
                            if (newTrack.Album != null)
                                newTrack.Album.DateImported = UnixEpoch.AddSeconds(double.Parse(item.Value)).ToLocalTime();
                            break;
                    }
                }
                Tracks.Add(newTrack.Key, newTrack);

            }

            Messenger.Default.Send<Messages.LibraryLoaded>(new Messages.LibraryLoaded());
        }
    }
}
