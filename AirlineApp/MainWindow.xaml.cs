using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;
using Airline;


namespace AirlineApp
{
   
    public partial class MainWindow : Window
    {
        public static XmlDocument xDoc = new XmlDocument();
        public static ObservableCollection<Plane> Planes = new();
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
    }
}