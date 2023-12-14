using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using Airline;
using Planes;

namespace AirlineApp;

public partial class MainWindow : Window
{
    public static XmlDocument xDoc = new();
    public static ObservableCollection<Plane> Planes = new();

    public static ObservableCollection<Plane> passenger = new();

    public static ObservableCollection<Plane> cargo = new();
    public static ObservableCollection<Plane> Passenger => passenger;
    public static ObservableCollection<Plane> Cargo => cargo;

    

    CollectionView passengerView;
    CollectionView cargoView;


    private GridViewColumnHeader? listViewSortCol;
    private SortAdorner? listViewSortAdorner;

    public static bool IsPassenger { get; set; }
    private FromToModel _ftm;

    public MainWindow()
    {
        InitializeComponent();
        ReadXml();
        _ftm = new FromToModel();
        DataContext = _ftm;
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
        passengerView.Filter = NameFilter;
        cargoView.Filter = NameFilter;
    }

    private bool NameFilter(object item)
    {
        if (String.IsNullOrEmpty(nameFilter.Text))
            return true;
        else
            return ((item as Plane)!.Name.IndexOf(nameFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
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
            CollectionViewSource.GetDefaultView(CargoPlanes.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(PassengerPlanes.ItemsSource).Refresh();
        }
    }
    private void fromFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {
        uint result;
        if (UInt32.TryParse(fromFilter.Text, out result))
        {
            CollectionViewSource.GetDefaultView(CargoPlanes.ItemsSource).Refresh();
            CollectionViewSource.GetDefaultView(PassengerPlanes.ItemsSource).Refresh();
        }
           
    }

    
    public void ReadXml()
    {
        Environment.CurrentDirectory =
            Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 15 - 21) +
            @"\PlanesClasses";
        xDoc.Load(@"airline.xml");
        var xRoot = xDoc.DocumentElement;
        if (xRoot != null)
            foreach (XmlElement xNode in xRoot)
            {
                Plane plane;
                if (xNode.Attributes.GetNamedItem("type")!.Value!.Equals("P"))
                {
                    plane = new PassengerPlane(xNode);
                    Passenger.Add(plane);
                }
                else
                {
                    plane = new CargoPlane(xNode);
                    Cargo.Add(plane);
                }

                Planes.Add(plane);
            }
    }

    public void SavingXml(object sender, RoutedEventArgs e)
    {
        xDoc.Save("airlinetemp.txt");
        var doc = new XDocument();
        var planes = new XElement("planes");

        foreach (var plane in Planes) planes.Add(plane.CreateXmlNode());
        doc.Add(planes);
        doc.Save("airline.xml");
    }

    private void AddPlane(object sender, RoutedEventArgs e)
    {
        IsPassenger = (sender as Button)!.Name.Equals(CreatePassengerPlane.Name);
        var choose = new CreatingPlane(IsPassenger);
        if (choose.ShowDialog() == true)
        {
            if (choose.Airplane != null) Planes.Add(choose.Airplane);
            if (IsPassenger)
                passenger.Add(choose.Airplane);
            else
                Cargo.Add(choose.Airplane);
        }
    }

    private void Exit(object sender, RoutedEventArgs e)
    {
        MessageBoxResult mbr = MessageBox.Show("Save Data?", "Exit", MessageBoxButton.YesNoCancel);
        switch (mbr)
        {
            case MessageBoxResult.Yes:
            {
                SavingXml(null, null);
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

    private void PlanesColumnHeader_Click(object sender, RoutedEventArgs e, ListView lvUsers)
    {
        var column = sender as GridViewColumnHeader;

        var sortBy = column.Tag.ToString();
        if (listViewSortCol != null)
        {
            AdornerLayer.GetAdornerLayer(listViewSortCol)?.Remove(listViewSortAdorner);
            lvUsers.Items.SortDescriptions.Clear();
        }

        var newDir = ListSortDirection.Ascending;
        if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
            newDir = ListSortDirection.Descending;

        listViewSortCol = column;
        listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
        AdornerLayer.GetAdornerLayer(listViewSortCol)?.Add(listViewSortAdorner);
        lvUsers.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
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
                Plane plane;
                if (target.Equals(PassengerPlanes))
                {
                    plane = PassengerPlanes.SelectedItem as Plane;
                    Passenger.Remove(plane);
                }
                else
                {
                    plane = CargoPlanes.SelectedItem as Plane;
                    Cargo.Remove(plane);
                }

                Planes.Remove(plane);
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
            Passenger.Remove(plane);
            CurPasPlanes.Items.Add(plane);
        }
        else
        {
            plane = CargoPlanes.SelectedItem as Plane;
            Cargo.Remove(plane);
            CurCargoPlanes.Items.Add(plane);
        }
    }

    private void TransferBack(object sender, RoutedEventArgs e)
    {
        var menuItem = (MenuItem)sender;
        var contextMenu = (ContextMenu)menuItem.Parent;
        var target = (ListView)contextMenu.PlacementTarget;
        Plane plane;
        if (target.Equals(CurPasPlanes))
        {
            plane = CurPasPlanes.SelectedItem as Plane;
            CurPasPlanes.Items.Remove(plane);
            Passenger.Add(plane);
        }
        else
        {
            plane = CurCargoPlanes.SelectedItem as Plane;
            CurCargoPlanes.Items.Remove(plane);
            Cargo.Add(plane);
        }
    }

}

/*
 <TreeView x:Name="PlaneTree"
                  dd:DragDrop.IsDragSource="True"
                  dd:DragDrop.IsDropTarget="True">
            <TreeView.Resources>
                <DataTemplate x:Key="PTempl" DataType="{x:Type planes:Plane}">
                    <TextBlock Text="{Binding Path=Name}" FontSize="15" ContextMenu="{StaticResource TreeCM}"/>
                </DataTemplate>
            </TreeView.Resources>

            <TreeViewItem Header="Passenger" x:Name="Passenger" ItemTemplate="{DynamicResource PTempl}"
                          Focusable="False" FontSize="15"/>
            <TreeViewItem Header="Cargo" x:Name="Cargo" ItemTemplate="{DynamicResource PTempl}" Focusable="False"
                          FontSize="15" />
        </TreeView>
 */