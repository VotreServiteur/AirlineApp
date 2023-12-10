using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Xml;
using System.Xml.Linq;
using Airline;
using Planes;


namespace AirlineApp
{
   
    public partial class MainWindow : Window
    {
        public static XmlDocument xDoc = new XmlDocument();
        public static ObservableCollection<Plane> Planes = new();

        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;
        
        public static bool IsPassenger { get; set; }
        
        
        public MainWindow()
        {
            InitializeComponent();
            ReadXml();
        }
        public void ReadXml()
        {
            Environment.CurrentDirectory = Directory.GetCurrentDirectory().Substring(0, Directory.GetCurrentDirectory().Length - 15 - 21) + @"\PlanesClasses";
            xDoc.Load( @"airline.xml");
            XmlElement? xRoot = xDoc.DocumentElement;   
            if (xRoot != null)
            {
                foreach (XmlElement xNode in xRoot)
                {
                    Plane plane;
                    if (xNode.Attributes.GetNamedItem("type")!.Value!.Equals("P"))
                    {
                        plane = new PassengerPlane(xNode);
                        Passenger.Items.Add(plane);
                        //CurPasPlanes.Items.Add(plane);
                    }
                    else
                    {
                        plane = new CargoPlane(xNode);
                        Cargo.Items.Add(plane);
                    }
                    Planes.Add(plane);

                }

            }
        }

        public void SavingXml(object sender, RoutedEventArgs e)
        {
            xDoc.Save("airlinetemp.txt");
            var doc = new XDocument();
            XElement planes = new XElement("planes");

            foreach (var plane in Planes)
            {
                planes.Add(plane.CreateXmlNode());
            }
            doc.Add(planes);
            doc.Save("airline.xml");
        }
        private void AddPlane(object sender, RoutedEventArgs e)
        {
            IsPassenger = (sender as Button)!.Name.Equals(CreatePassengerPlane.Name);
            CreatingPlane choose = new CreatingPlane(IsPassenger);
            if (choose.ShowDialog() == true)
            {
                if (choose.Airplane != null) Planes.Add(choose.Airplane);
                if (IsPassenger)
                {
                    Passenger.Items.Add(choose.Airplane);
                }
                else
                {
                    Cargo.Items.Add(choose.Airplane);
                }
            }
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            SavingData sd = new SavingData();
            if (sd.ShowDialog() == true)
            {
                if (sd.IsForSave)
                    SavingXml(null, null);
                this.Close();
            }
        }

        private void Plane_OnDragEnter(object sender, DragEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Plane_OnMouseMove(object sender, MouseEventArgs e)
        {
            Plane plane = sender as Plane;
            if (plane != null && e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop( (DependencyObject)e.Source,
                    plane,
                    DragDropEffects.Copy);
            }
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
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            
            string sortBy = column.Tag.ToString();
            if(listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol)?.Remove(listViewSortAdorner);
                lvUsers.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if(listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            lvUsers.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }

        private void RemoveItemCommand(Plane planeToDelete)
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
    }

    
}