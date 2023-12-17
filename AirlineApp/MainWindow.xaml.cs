using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Airline;
using DataObjects;
using Planes;
using ServiceActions;
using SortingService;


namespace AirlineApp;

public partial class MainWindow : Window
{
    private Service service;
    public static bool IsPassenger { get; set; }
    private FromToModel _ftm;
    
    CollectionView passengerView;
    CollectionView cargoView;
    

    public MainWindow()
    {
        InitializeComponent();
        _ftm = new FromToModel();
        DataContext = _ftm;
        service = new Service();
        passengerView = (CollectionView)CollectionViewSource.GetDefaultView(PassengerPlanes.ItemsSource);
        cargoView = (CollectionView)CollectionViewSource.GetDefaultView(CargoPlanes.ItemsSource);
        passengerView.Filter = NameFilter;
        cargoView.Filter = NameFilter;
        ComboName.IsSelected = true;
    }


    private void ComboFuel_OnSelected(object sender, RoutedEventArgs e)
    {
        nameFilter.Visibility = Visibility.Hidden;
        nameFilter.Text = null;
        if (fromFilter.Visibility == Visibility.Hidden)
            FromToSwitch();
        fromFilter.Text = null;
        toFilter.Text = null;
        passengerView.Filter = FuelFilter;
        cargoView.Filter = FuelFilter;
    }

    private bool FuelFilter(object obj)
    {
        if (fromFilter.Text.Equals("") && toFilter.Text.Equals("") || (Validation.GetHasError(fromFilter) && Validation.GetHasError(toFilter)))
            return true;
        if (fromFilter.Text.Equals(""))
            return ((obj as Plane)!.FuelRate <= Convert.ToInt32(toFilter.Text));
        if (toFilter.Text.Equals(""))
            return ((obj as Plane)!.FuelRate >= Convert.ToInt32(fromFilter.Text));
        return ((obj as Plane)!.FuelRate >= Convert.ToInt32(fromFilter.Text)) &&
               ((obj as Plane)!.FuelRate <= Convert.ToInt32(toFilter.Text));
    }

    private bool RangeFilter(object obj)
    {
        if (fromFilter.Text.Equals("") && toFilter.Text.Equals(""))
            return true;
        if (fromFilter.Text.Equals(""))
            return ((obj as Plane)!.FlightRange <= Convert.ToInt32(toFilter.Text));
        if (toFilter.Text.Equals(""))
            return ((obj as Plane)!.FlightRange >= Convert.ToInt32(fromFilter.Text));
        return ((obj as Plane)!.FlightRange >= Convert.ToInt32(fromFilter.Text)) &&
               ((obj as Plane)!.FlightRange <= Convert.ToInt32(toFilter.Text));
    }

    private void FromToSwitch()
    {
        if (fromFilter.Visibility == Visibility.Visible)
        {
            fromFilter.Visibility = Visibility.Hidden;
            toFilter.Visibility = Visibility.Hidden;
            fromFilterBlock.Visibility = Visibility.Hidden;
            toFilterBlock.Visibility = Visibility.Hidden;
        }
        else
        {
            fromFilter.Visibility = Visibility.Visible;
            toFilter.Visibility = Visibility.Visible;
            fromFilterBlock.Visibility = Visibility.Visible;
            toFilterBlock.Visibility = Visibility.Visible;
        }
    }

    private void ComboRange_OnSelected(object sender, RoutedEventArgs e)
    {
        nameFilter.Visibility = Visibility.Hidden;
        nameFilter.Text = null;
        if (fromFilter.Visibility == Visibility.Hidden)
            FromToSwitch();
        fromFilter.Text = null;
        toFilter.Text = null;
        passengerView.Filter = RangeFilter;
        cargoView.Filter = RangeFilter;
    }

    private void ComboName_OnSelected(object sender, RoutedEventArgs e)
    {
        nameFilter.Visibility = Visibility.Visible;
        nameFilter.Text = null;
        if (fromFilter.Visibility == Visibility.Visible)
            FromToSwitch();
        fromFilter.Text = null;
        toFilter.Text = null;
        //passengerView.Filter = NameFilter;
        //cargoView.Filter = NameFilter;
    }

    private bool NameFilter(object item)
    {
        if (String.IsNullOrEmpty(nameFilter.Text)) return true;
        return (item as Plane)!.Name
            .IndexOf(nameFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0;
    }

    private void nameFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        CollectionViewSource.GetDefaultView(CargoPlanes.ItemsSource).Refresh();
        CollectionViewSource.GetDefaultView(PassengerPlanes.ItemsSource).Refresh();
    }

    private void toFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        uint result;
        if (UInt32.TryParse(toFilter.Text, out result))
        {
            //CollectionViewSource.GetDefaultView(CargoPlanes.ItemsSource).Refresh();
            //CollectionViewSource.GetDefaultView(PassengerPlanes.ItemsSource).Refresh();
        }
    }
    private void fromFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        uint result;
        //if (UInt32.TryParse(fromFilter.Text, out result))
        //{
         //   CollectionViewSource.GetDefaultView(CargoPlanes.ItemsSource).Refresh();
          //  CollectionViewSource.GetDefaultView(PassengerPlanes.ItemsSource).Refresh();
        //}
           
    }
    private void AddPlane(object sender, RoutedEventArgs e)
    {
        IsPassenger = (sender as Button)!.Name.Equals(CreatePassengerPlane.Name);
        var choose = new CreatingPlane(IsPassenger);
        if (choose.ShowDialog() == true && choose.Airplane != null) service.AddPlane(IsPassenger, choose.Airplane);
    }
    private void Exit(object sender, RoutedEventArgs e)
    {
        MessageBoxResult mbr = MessageBox.Show("Save Data?", "Exit", MessageBoxButton.YesNoCancel);
        switch (mbr)
        {
            case MessageBoxResult.Yes:
            {
                service.Save();
                Close();
                break;
            }
            case MessageBoxResult.No:
                Close();
                break;
            case MessageBoxResult.Cancel:
                break;
        }
    }


    private void СurPassengerPlanesColumnHeader_Click(object sender, RoutedEventArgs e)
        => PlanesColumnHeader_Click(sender, e, CurPasPlanes);

    private void CurCargoPlanesColumnHeader_Click(object sender, RoutedEventArgs e)
        => PlanesColumnHeader_Click(sender, e, CurCargoPlanes);

    private void PassengerPlanesColumnHeader_Click(object sender, RoutedEventArgs e)
        => PlanesColumnHeader_Click(sender, e, PassengerPlanes);

    private void CargoPlanesColumnHeader_Click(object sender, RoutedEventArgs e)
        => PlanesColumnHeader_Click(sender, e, CargoPlanes);

    private void PlanesColumnHeader_Click(object sender, RoutedEventArgs e, ListView lvPlanes)
    {
        var column = sender as GridViewColumnHeader;
        SortingListView.Sorting(column, lvPlanes);

    }

    private void Remove(object sender, RoutedEventArgs e)
    {
        bool pas = PassengerPlanes.IsMouseOver;
        var menuItem = (MenuItem)sender;
        var contextMenu = (ContextMenu)menuItem.Parent;
        var target = (ListView)contextMenu.PlacementTarget;
        MessageBoxResult mbr = MessageBox.Show("You sure?", "Delete plane", MessageBoxButton.YesNo);
        switch (mbr)
        {
            case MessageBoxResult.Yes:
            {
                Plane? plane;
                plane = target.Equals(PassengerPlanes)
                    ? PassengerPlanes.SelectedItem as Plane
                    : CargoPlanes.SelectedItem as Plane;
                service.DeletePlane(target.Equals(PassengerPlanes), plane);
                break;
            }
            case MessageBoxResult.No:
                break;
        }
    }

    private void TransferFrom(object sender, RoutedEventArgs e)
    {
        var menuItem = (MenuItem)sender;
        var contextMenu = (ContextMenu)menuItem.Parent;
        var target = (ListView)contextMenu.PlacementTarget;
        Plane plane;
        if (target.Equals(PassengerPlanes))
        {
            plane = PassengerPlanes.SelectedItem as Plane;
            service.DeletePlane(true, plane);
            CurPasPlanes.Items.Add(plane);
        }
        else
        {
            plane = CargoPlanes.SelectedItem as Plane;
            service.DeletePlane(false,plane);
            CurCargoPlanes.Items.Add(plane);
        }
    }

    private void TransferBack(object sender, RoutedEventArgs e)
    {
        var menuItem = (MenuItem)sender;
        var contextMenu = (ContextMenu)menuItem.Parent;
        var target = (ListView)contextMenu.PlacementTarget;
        Plane? plane;
        if (target.Equals(CurPasPlanes))
        {
            plane = CurPasPlanes.SelectedItem as Plane;
            CurPasPlanes.Items.Remove(plane);
            service.AddPlane(true, plane);
        }
        else
        {
            plane = CurCargoPlanes.SelectedItem as Plane;
            CurCargoPlanes.Items.Remove(plane);
            service.AddPlane(false, plane);
        }
    }

    private void SavingXml(object sender, RoutedEventArgs e) => service.Save();
}
