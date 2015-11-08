using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Belial.Models.Library;
using Belial.Services.MediaCenterServices;
using Windows.Globalization.DateTimeFormatting;
using Windows.Globalization;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace Belial.ViewModels
{
    public class AlbumViewModel : Belial.Mvvm.ViewModelBase
    {
        public AlbumViewModel()
        {
            FilteredTracks = new List<Track>();

            Messenger.Default.Register<Messages.LibraryLoaded>(this, (message) =>
            {
                RaisePropertyChanged("Albums");
            });

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime data
                this.Value = "Designtime value";
                this.SelectedAlbum = new Album() { AlbumArtist = new Artist() { Name = "Miles Davis" }, Name = "Kind of Blue", ImageSource = "https://upload.wikimedia.org/wikipedia/en/9/9c/MilesDavisKindofBlue.jpg", Year = 1969, DateImported = new DateTime(1987, 6, 19) };

                return;
            }
        }

        public override void OnNavigatedTo(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            if (state.Any())
            {
                // use cache value(s)
                if (state.ContainsKey(nameof(Value))) Value = state[nameof(Value)]?.ToString();
                // clear any cache
                state.Clear();
            }
            else
            {
                // use navigation parameter
                Value = parameter?.ToString();
            }
        }

        public override Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            if (suspending)
            {
                // persist into cache
                state[nameof(Value)] = Value;
            }
            return base.OnNavigatedFromAsync(state, suspending);
        }

        public override void OnNavigatingFrom(NavigatingEventArgs args)
        {
            args.Cancel = false;
        }

        private string _Value = "Default";
        public string Value { get { return _Value; } set { Set(ref _Value, value); } }

        public SortedDictionary<string, Album>.ValueCollection Albums { get { return LibraryService.Instance.Albums.Values; } }

        public List<Track> FilteredTracks { get; set; }

        DateTimeFormatter dateTimeFormatter = new DateTimeFormatter(YearFormat.Full,
            MonthFormat.Full,
            DayFormat.Default,
            DayOfWeekFormat.None,
            HourFormat.None,
            MinuteFormat.None,
            SecondFormat.None,
            new[] { "en-US" }
            );

        public string SelectedAlbumDateImported
        {
            get
            {
                if(SelectedAlbum != null && SelectedAlbum.DateImported != null)
                {
                    return dateTimeFormatter.Format(SelectedAlbum.DateImported);
                }
                return "";
            }
        }

        private Album selectedAlbum;
        public Album SelectedAlbum {
            get
            {
                return selectedAlbum;
            }
            set
            {
                if (value == selectedAlbum)
                    return;
                selectedAlbum = value;

                // Update the filtered track stuff now

                var query = from tracks in LibraryService.Instance.Tracks.Values
                            where tracks.Album == SelectedAlbum
                            orderby tracks.TrackNumber ascending
                            select tracks;

                FilteredTracks = query.ToList();
                this.RaisePropertyChanged("SelectedAlbum");
                this.RaisePropertyChanged("FilteredTracks");
                this.RaisePropertyChanged("SelectedAlbumDateImported");
                this.RaisePropertyChanged("AddAllNext");
                this.RaisePropertyChanged("AddAllEnd");
                this.RaisePropertyChanged("PlayAll");
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
                        McwsService.Instance.AddNext(FilteredTracks);
                    },
                    () => FilteredTracks.Count > 0));
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
                        McwsService.Instance.AddToEnd(FilteredTracks);
                    },
                    () => FilteredTracks.Count > 0));
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
                        McwsService.Instance.Play(FilteredTracks);
                    },
                    () => FilteredTracks.Count > 0));
            }
        }
    }
}

