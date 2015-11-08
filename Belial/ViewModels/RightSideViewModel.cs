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

namespace Belial.ViewModels
{
    public class RightSideViewModel : Belial.Mvvm.ViewModelBase
    {
        public RightSideViewModel()
        {
            Messenger.Default.Register<Status>(this, async (Status status) =>
            {
                IsPlaying = status.IsPlaying;
                volume = status.Volume; // if we write to the value using the property, it will trigger a send
                RaisePropertyChanged(VolumePropertyName);

            });

            Messenger.Default.Register<Track>(this, (Track newTrack) =>
            {
                CurrentTrack = newTrack;
                this.RaisePropertyChanged("CurrentTrack");
            });
        }

        public Track CurrentTrack { get; set; }


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
