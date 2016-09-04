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
using MyToolkit.Collections;
using System.Collections.ObjectModel;
using Belial.Services.SettingsServices;

namespace Belial.ViewModels
{
    public enum AlbumSortMode
    {
        Alphabetical,
        RecentlyImported,
        Newest
    }

    public class AlbumViewModel : Belial.Mvvm.ViewModelBase
    {
        public AlbumViewModel()
        {
            FilteredTracks = new TrackListViewModel();

            Messenger.Default.Register<Messages.LibraryLoaded>(this, (message) =>
            {
                RaisePropertyChanged("Albums");
            });

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {

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

        public List<Album> Albums
        {
            get
            {
                switch(SortMode)
                {
                    default:
                        return LibraryService.Instance.Albums.Values.ToList();
                        break;
                    case AlbumSortMode.Newest:
                        return LibraryService.Instance.Albums.Values.OrderBy(x => x.Year).Reverse().ToList();
                        break;

                    case AlbumSortMode.RecentlyImported:
                        return LibraryService.Instance.Albums.Values.OrderBy(x => x.DateImported).Reverse().ToList();
                        break;
                }
                    

            }
        }

        public TrackListViewModel FilteredTracks { get; set; }

        public AlbumSortMode SortMode
        {
            get
            {
                return SettingsService.Instance.AlbumSortMode;
            }
            set
            {
                if (SettingsService.Instance.AlbumSortMode == value)
                    return;
                SettingsService.Instance.AlbumSortMode = value;
                RaisePropertyChanged("SortMode");
                RaisePropertyChanged("Albums");
            }
        }
        

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

                FilteredTracks.Tracks = new ObservableCollection<Track>(query.ToList());
                this.RaisePropertyChanged("SelectedAlbum");
                this.RaisePropertyChanged("FilteredTracks");
                this.RaisePropertyChanged("SelectedAlbumDateImported");
                this.RaisePropertyChanged("AddAllNext");
                this.RaisePropertyChanged("AddAllEnd");
                this.RaisePropertyChanged("PlayAll");
            }
        }

        //        <MenuFlyoutItem Text = "Play" Command="{Binding PlaySelected}" />
        //<MenuFlyoutItem Text = "Add Next" Command="{Binding AddSelectedNext}" />
        //<MenuFlyoutItem Text = "Add To End" Command="{Binding AddSelectedEnd}" />


        private RelayCommand sortNewestFirstCommand;

        /// <summary>
        /// Gets the SortNewestFirst.
        /// </summary>
        public RelayCommand SortNewestFirstCommand
        {
            get
            {
                return sortNewestFirstCommand
                    ?? (sortNewestFirstCommand = new RelayCommand(
                    () =>
                    {
                        SortMode = AlbumSortMode.Newest;
                    }));
            }
        }

        private RelayCommand sortRecentlyImportedCommand;

        /// <summary>
        /// Gets the SortRecentlyImported.
        /// </summary>
        public RelayCommand SortRecentlyImportedCommand
        {
            get
            {
                return sortRecentlyImportedCommand
                    ?? (sortRecentlyImportedCommand = new RelayCommand(
                    () =>
                    {
                        SortMode = AlbumSortMode.RecentlyImported;
                    }));
            }
        }

        private RelayCommand sortAlphabeticalCommand;

        /// <summary>
        /// Gets the SortAlphabeticalCommand.
        /// </summary>
        public RelayCommand SortAlphabeticalCommand
        {
            get
            {
                return sortAlphabeticalCommand
                    ?? (sortAlphabeticalCommand = new RelayCommand(
                    () =>
                    {
                        SortMode = AlbumSortMode.Alphabetical;
                    }));
            }
        }

    }
}

