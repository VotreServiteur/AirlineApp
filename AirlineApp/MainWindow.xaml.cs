using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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


    private GridViewColumnHeader listViewSortCol;
    private SortAdorner listViewSortAdorner;

    public static bool IsPassenger { get; set; }


    public MainWindow()
    {
        InitializeComponent();
        ReadXml();
        DataContext = true;
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
                    //CurPasPlanes.Items.Add(plane);
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

    private void Plane_OnDragEnter(object sender, DragEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Plane_OnMouseMove(object sender, MouseEventArgs e)
    {
        var plane = sender as Plane;
        if (plane != null && e.LeftButton == MouseButtonState.Pressed)
            DragDrop.DoDragDrop((DependencyObject)e.Source,
                plane,
                DragDropEffects.Copy);
    }

    private void Plane_OnGiveFeedback(object sender, GiveFeedbackEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Plane_OnDragLeave(object sender, DragEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Plane_OnDragOver(object sender, DragEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void Plane_OnDrop(object sender, DragEventArgs e)
    {
        throw new NotImplementedException();
    }

    private void PassengerPlanesColumnHeader_Click(object sender, RoutedEventArgs e)
    {
        PlanesColumnHeader_Click(sender, e, CurPasPlanes);
    }

    private void CargoPlanesColumnHeader_Click(object sender, RoutedEventArgs e)
    {
        PlanesColumnHeader_Click(sender, e, CurCargoPlanes);
    }

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
        AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
        lvUsers.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
    }

    /* private void RemoveItemCommand(Plane planeToDelete)
     {
         int delpos = 0;
         foreach (Plane plane in PlaneTree.Items)
         {
             if (plane.Equals(planeToDelete))
             {
                 break;
             }
             delpos++;
         }

         MessageBox.Show(delpos.ToString());
         PlaneTree.Items.Remove(planeToDelete);
     }
     */
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