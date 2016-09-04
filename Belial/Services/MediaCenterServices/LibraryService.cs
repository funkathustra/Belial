using Belial.Messages;
using Belial.Models.Library;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Belial.Services.MediaCenterServices
{
    public class LibraryService : INotifyPropertyChanged
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

        public List<Track> NowPlaying { get; set; }

        public LibraryService()
        {
            Artists = new SortedDictionary<string, Artist>();
            Albums = new SortedDictionary<string, Album>();
            Tracks = new SortedDictionary<int, Track>();
            NowPlaying = new List<Track>();
        }

        DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public event PropertyChangedEventHandler PropertyChanged;

        public async void ParseNowPlayingStream(Stream XML)
        {
            var serializer = new XmlSerializer(typeof(MplResponse));
            MplResponse data = (MplResponse)serializer.Deserialize(XML);
            var tempList = new List<Track>();
            foreach (var responseItem in data.Items)
            {
                var track = FieldSetToTrack(responseItem.Fields);
                if (Tracks.ContainsKey(track.Key))
                    tempList.Add(Tracks[track.Key]);
            }

            if(!tempList.SequenceEqual(NowPlaying))
            {
                NowPlaying = tempList;
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    Messenger.Default.Send<NowPlayingChanged>(new NowPlayingChanged() { NowPlaying = this.NowPlaying });
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NowPlaying"));
                });
            }

            
        }

        public async void ParseLibraryStream(Stream XML)
        {
            Tracks.Clear();
            Albums.Clear();
            Artists.Clear();
            var serializer = new XmlSerializer(typeof(MplResponse));
            MplResponse data = (MplResponse)serializer.Deserialize(XML);
            foreach (var responseItem in data.Items)
            {
                var track = FieldSetToTrack(responseItem.Fields);
                if(!Tracks.ContainsKey(track.Key))
                    Tracks.Add(track.Key, track);
            }

            //// cache these queries by calling them
            //var templist1 = LibraryService.Instance.Albums.Values.OrderBy(x => x.Year).Reverse().ToList();
            //var templist2 = LibraryService.Instance.Albums.Values.OrderBy(x => x.DateImported).Reverse().ToList();

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tracks"));
                Messenger.Default.Send<Messages.LibraryLoaded>(new Messages.LibraryLoaded() { IsLoaded = true });
            });
        }

        public Track FieldSetToTrack(List<ItemField> FieldList)
        {
            Track newTrack = new Track();

            // Much faster than stuffing key/values in a dictionary.
            foreach (var item in FieldList)
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
                        newTrack.Artist = FindOrCreateArtist(item.Value);
                        break;
                    case "Album":
                        newTrack.Album = FindOrCreateAlbum(item.Value);
                        newTrack.Album.ImageSource = string.Format("http://{0}:{1}/MCWS/v1/File/GetImage?File={2}", McwsService.Instance.ServerIp, McwsService.Instance.ServerPort, newTrack.Key);
                        break;
                    case "Album Artist":
                        if(newTrack.Album != null)
                            newTrack.Album.AlbumArtist = FindOrCreateArtist(item.Value);
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

            return newTrack;

        }

        public Track FindOrCreateTrack(int key)
        {
            if (Tracks.ContainsKey(key))
                return Tracks[key];

            var newTrack = new Track() { Key = key };
            Tracks.Add(key, newTrack);

            return newTrack;
        }

        public Album FindOrCreateAlbum(string AlbumName)
        {
            if (Albums.ContainsKey(AlbumName))
                return Albums[AlbumName];

            var newAlbum = new Album() { Name = AlbumName };
            Albums.Add(AlbumName, newAlbum);

            return newAlbum;
        }

        public Artist FindOrCreateArtist(string Artist)
        {
            if (Artists.ContainsKey(Artist))
                return Artists[Artist];

            var newArtist = new Artist() { Name = Artist };
            Artists.Add(Artist, newArtist);

            return newArtist;
        }

        internal void Clear()
        {
            Artists.Clear();
            Albums.Clear();
            Tracks.Clear();
        }
    }
}
