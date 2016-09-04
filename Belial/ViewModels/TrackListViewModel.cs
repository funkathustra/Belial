using Belial.Models.Library;
using Belial.Services.MediaCenterServices;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Belial.ViewModels
{
    public class TrackListViewModel : Belial.Mvvm.ViewModelBase
    {

        private ObservableCollection<Track> tracks;

        public ObservableCollection<Track> Tracks
        {
            get
            {
                if (tracks == null)
                    tracks = new ObservableCollection<Track>();
                return tracks;
            }
            set
            {
                tracks = value;
                RaisePropertyChanged("Tracks");
            }
        }

        private ObservableCollection<Track> selectedTracks;

        public ObservableCollection<Track> SelectedTracks
        {
            get
            {
                if (selectedTracks == null)
                    selectedTracks = new ObservableCollection<Track>();
                return selectedTracks;
            }
            set
            {
                selectedTracks = value;
                RaisePropertyChanged("SelectedTracks");
            }
        }

        private RelayCommand playSelected;

        /// <summary>
        /// Gets the PlaySelected.
        /// </summary>
        public RelayCommand PlaySelected
        {
            get
            {
                return playSelected
                    ?? (playSelected = new RelayCommand(
                    () =>
                    {
                        McwsService.Instance.Play(SelectedTracks.ToList());
                    }));
            }
        }

        private RelayCommand addSelectedNext;

        /// <summary>
        /// Gets the AddSelectedNext.
        /// </summary>
        public RelayCommand AddSelectedNext
        {
            get
            {
                return addSelectedNext
                    ?? (addSelectedNext = new RelayCommand(
                    () =>
                    {
                        McwsService.Instance.AddNext(SelectedTracks.ToList());
                    }));
            }
        }

        private RelayCommand addSelectedEnd;

        /// <summary>
        /// Gets the AddSelectedEnd.
        /// </summary>
        public RelayCommand AddSelectedEnd
        {
            get
            {
                return addSelectedEnd
                    ?? (addSelectedEnd = new RelayCommand(
                    () =>
                    {
                        McwsService.Instance.AddToEnd(SelectedTracks.ToList());
                    }));
            }
        }



        private RelayCommand addAllNext;

        /// <summary>
        /// Gets the AddAllNext.
        /// </summary>
        public RelayCommand AddAllNext
        {
            get
            {
                return addAllNext
                    ?? (addAllNext = new RelayCommand(
                    () =>
                    {
                        McwsService.Instance.AddNext(Tracks.ToList());
                    },
                    () => Tracks.Count > 0));
            }
        }

        private RelayCommand addAllEnd;

        /// <summary>
        /// Gets the AddAllEnd.
        /// </summary>
        public RelayCommand AddAllEnd
        {
            get
            {
                return addAllEnd
                    ?? (addAllEnd = new RelayCommand(
                    () =>
                    {
                        McwsService.Instance.AddToEnd(Tracks.ToList());
                    },
                    () => Tracks.Count > 0));
            }
        }

        private RelayCommand playAll;

        /// <summary>
        /// Gets the PlayAll.
        /// </summary>
        public RelayCommand PlayAll
        {
            get
            {
                return playAll
                    ?? (playAll = new RelayCommand(
                    () =>
                    {
                        McwsService.Instance.Play(Tracks.ToList());
                    },
                    () => Tracks.Count > 0));
            }
        }


        public TrackListViewModel()
        {
            Tracks = new ObservableCollection<Track>();
            SelectedTracks = new ObservableCollection<Track>();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime data
                var album = new Album()
                {
                    AlbumArtist = new Artist()
                    {
                        Name = "Miles Davis"
                    },
                    Name = "Kind of Blue",
                    ImageSource = "https://upload.wikimedia.org/wikipedia/en/9/9c/MilesDavisKindofBlue.jpg",
                    Year = 1969,
                    DateImported = new DateTime(1987, 6, 19)
                };

                Tracks.Add(new Track() { TrackNumber = 1, Album = album, Artist = album.AlbumArtist, Name = "So What" });
                Tracks.Add(new Track() { TrackNumber = 2, Album = album, Artist = album.AlbumArtist, Name = "Freddie Freeloader" });
                Tracks.Add(new Track() { TrackNumber = 3, Album = album, Artist = album.AlbumArtist, Name = "Blue in Green" });
                Tracks.Add(new Track() { TrackNumber = 4, Album = album, Artist = album.AlbumArtist, Name = "All Blues" });
                Tracks.Add(new Track() { TrackNumber = 5, Album = album, Artist = album.AlbumArtist, Name = "Flamenco Sketches" });
                return;
            }
        }
    }
}
