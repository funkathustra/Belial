using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Belial.Services.MediaCenterServices;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Belial.Messages;
using Windows.UI.Xaml;
using Belial.Models.Library;
namespace Belial.ViewModels
{
    public class RightSideViewModel : Belial.Mvvm.ViewModelBase
    {
        public RightSideViewModel()
        {
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

                NowPlaying.Add(new Track() { TrackNumber = 1, Album = album, Artist = album.AlbumArtist, Name = "So What" });
                NowPlaying.Add(new Track() { TrackNumber = 2, Album = album, Artist = album.AlbumArtist, Name = "Freddie Freeloader" });
                NowPlaying.Add(new Track() { TrackNumber = 3, Album = album, Artist = album.AlbumArtist, Name = "Blue in Green" });
                NowPlaying.Add(new Track() { TrackNumber = 4, Album = album, Artist = album.AlbumArtist, Name = "All Blues" });
                NowPlaying.Add(new Track() { TrackNumber = 5, Album = album, Artist = album.AlbumArtist, Name = "Flamenco Sketches" });
                RaisePropertyChanged("NowPlaying");
                return;
            }

            Messenger.Default.Register<Status>(this, (Status status) =>
            {
                IsPlaying = status.IsPlaying;
                volume = status.Volume; // if we write to the value using the property, it will trigger a send
                RaisePropertyChanged(VolumePropertyName);
            });

            Messenger.Default.Register<Track>(this, (newTrack) =>
            {
                CurrentTrack = newTrack;
                this.RaisePropertyChanged("CurrentTrack");
            });

            Messenger.Default.Register<NowPlayingChanged>(this, newList =>
            {
                NowPlaying = newList.NowPlaying;
            });

            //NowPlaying = new List<Track>();
        }

        public Track CurrentTrack { get; set; }

        /// <summary>
        /// The <see cref="NowPlaying" /> property's name.
        /// </summary>
        public const string NowPlayingPropertyName = "NowPlaying";

        private List<Track> nowPlaying = new List<Track>();

        /// <summary>
        /// Sets and gets the NowPlaying property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<Track> NowPlaying
        {
            get
            {
                return nowPlaying;
            }

            set
            {
                if (nowPlaying == value)
                {
                    return;
                }

                nowPlaying = value;
                RaisePropertyChanged(NowPlayingPropertyName);
            }
        }


        /// <summary>
        /// The <see cref="Volume" /> property's name.
        /// </summary>
        public const string VolumePropertyName = "Volume";

        private double volume = 0;

        /// <summary>
        /// Sets and gets the Volume property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public double Volume
        {
            get
            {
                return volume;
            }

            set
            {
                if (volume == value)
                {
                    return;
                }

                volume = value;
                McwsService.Instance.SetVolume(volume);
                RaisePropertyChanged(VolumePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsPlaying" /> property's name.
        /// </summary>
        public const string IsPlayingPropertyName = "IsPlaying";

        private bool isPlaying = false;

        /// <summary>
        /// Sets and gets the IsPlaying property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public bool IsPlaying
        {
            get
            {
                return isPlaying;
            }

            set
            {
                if (isPlaying == value)
                {
                    return;
                }

                isPlaying = value;
                RaisePropertyChanged(IsPlayingPropertyName);
                RaisePropertyChanged("PlayIcon");
            }
        }


        private RelayCommand playPause;

        /// <summary>
        /// Gets the PlayPause.
        /// </summary>
        public RelayCommand PlayPause
        {
            get
            {
                return playPause
                    ?? (playPause = new RelayCommand(
                    () =>
                    {
                        McwsService.Instance.PlayPause();
                        IsPlaying = !IsPlaying;
                    }));
            }
        }

        private RelayCommand pause;

        /// <summary>
        /// Gets the Pause.
        /// </summary>
        public RelayCommand Pause
        {
            get
            {
                return pause
                    ?? (pause = new RelayCommand(
                    () =>
                    {
                        McwsService.Instance.Pause();
                    }));
            }
        }

        private RelayCommand stop;

        /// <summary>
        /// Gets the Stop.
        /// </summary>
        public RelayCommand Stop
        {
            get
            {
                return stop
                    ?? (stop = new RelayCommand(
                    () =>
                    {
                        McwsService.Instance.Stop();
                    }));
            }
        }

        private RelayCommand next;

        /// <summary>
        /// Gets the Next.
        /// </summary>
        public RelayCommand Next
        {
            get
            {
                return next
                    ?? (next = new RelayCommand(
                    () =>
                    {
                        McwsService.Instance.Next();
                    }));
            }
        }

        private RelayCommand prev;

        /// <summary>
        /// Gets the Prev.
        /// </summary>
        public RelayCommand Prev
        {
            get
            {
                return prev
                    ?? (prev = new RelayCommand(
                    () =>
                    {
                        McwsService.Instance.Prev();
                    }));
            }
        }

        /// <summary>
        /// Sets and gets the PlayIcon property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string PlayIcon
        {
            get
            {
                return IsPlaying ? "Pause" : "Play";
            }
        }
    }
}
