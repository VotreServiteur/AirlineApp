using System.Collections.ObjectModel;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Airline;
using Planes;

namespace DataObjects;

public class DataBase
{
    public static ObservableCollection<Plane> Planes { get; set; }
    public static ObservableCollection<PassengerPlane> Passenger { get; set; }
    public static ObservableCollection<CargoPlane> Cargo { get; set; }

    private static DataBase? instance = new DataBase(ConnectionString);
    private static string? ConnectionString { get; set; }

    public DataBase(string connectionString)
    {
        ConnectionString = ConnectionString == null ? "airline.xml" : connectionString;

        Planes = new ObservableCollection<Plane>();
        Passenger = new ObservableCollection<PassengerPlane>();
        Cargo = new ObservableCollection<CargoPlane>();

        Read();
    }

    public void DeletePlane(bool IsPassenger, Plane plane)
    {
        if (IsPassenger)
            Passenger?.Remove(plane as PassengerPlane);
        else
            Cargo?.Remove(plane as CargoPlane);

        Planes.Remove(plane);
    }

    public void AddPlane(bool isPassenger, Plane plane)
    {
        Planes.Add(plane);
        if (isPassenger)
            Passenger?.Add(plane as PassengerPlane);
        else
            Cargo?.Add(plane as CargoPlane);
    }

    private void Read()
    {
        Environment.CurrentDirectory =
            Directory.GetCurrentDirectory()
                .Substring(0, Directory.GetCurrentDirectory().Length - 15 - 21) +
            @"\PlanesClasses";
        XmlDocument xDoc = new();
        xDoc.Load(ConnectionString);
        var xRoot = xDoc.DocumentElement;
        if (xRoot != null)
            foreach (XmlElement xNode in xRoot)
            {
                Plane plane;
                if (xNode.Attributes.GetNamedItem("type")!.Value!.Equals("P"))
                {
                    plane = new PassengerPlane(xNode);
                    Passenger.Add(plane as PassengerPlane);
                }
                else
                {
                    plane = new CargoPlane(xNode);
                    Cargo.Add(plane as CargoPlane);
                }

                Planes.Add(plane);
            }
    }

    public void Saving()
    {
        XmlDocument xDoc = new();
        xDoc.Load(ConnectionString);
        xDoc.Save("airlinetemp.txt");
        var doc = new XDocument();
        var planes = new XElement("planes");

        foreach (var plane in Planes) planes.Add(plane.CreateXmlNode());
        doc.Add(planes);
        doc.Save("airline.xml");
    }
    public static DataBase GetInstance()
    {
        if (instance == null) return new DataBase(ConnectionString);
        return instance;
    }

    public ObservableCollection<PassengerPlane?>? GetPassengerPlanes()
    {
        return Passenger;
    }
    public ObservableCollection<CargoPlane?>? GetCargoPlanes()
    {
        return Cargo;
    }
}