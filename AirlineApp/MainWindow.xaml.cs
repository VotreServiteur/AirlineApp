using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Airline;


namespace AirlineApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static XmlDocument xDoc = new XmlDocument();
        public static List<Plane> Planes = new();
        public static bool IsPassenger;
        public MainWindow()
        {
            InitializeComponent();
            ReadXml();
        }
        public void ReadXml()
        {
            xDoc.Load(@"C:\Users\makst\Source\Repos\VotreServiteur\AirlineApp\PlanesClasses\airline.xml");
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

        private void AddPlane(object sender, RoutedEventArgs e)
        {
            IsPassenger = (sender as Button).Name.Equals(CreatePassengerPlane.Name);
            Window choose = new CreatingPlane(IsPassenger);
            choose.ShowDialog();
        }
    }
}