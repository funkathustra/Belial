using Belial.Models.Library;
using Belial.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Belial.Views
{
    public sealed partial class TrackList : UserControl
    {
        public TrackList()
        {
            this.InitializeComponent();
        }

        private void TracksListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewmodel = ((TrackListViewModel)DataContext);
            viewmodel.SelectedTracks.Clear();
            foreach (var item in TracksListView.SelectedItems)
            {
                ((TrackListViewModel)DataContext).SelectedTracks.Add((Track)item);
            }
        }

        private void TrackStackPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.AcceptedOperation = Windows.ApplicationModel.DataTransfer.DataPackageOperation.Copy;
        }

        private void TrackStackPanel_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
            //var flyout = this.Resources["TrackContextMenu"] as MenuFlyout;
            //flyout.ShowAt((UIElement)AlbumPage, new Windows.Foundation.Point(0, 0));
            //flyout.ShowAt((UIElement)sender, new Windows.Foundation.Point(0, 0));
            //flyout.ShowAt((FrameworkElement)sender);
        }

        private void TrackStackPanel_Drop(object sender, DragEventArgs e)
        {

        }
    }
}
