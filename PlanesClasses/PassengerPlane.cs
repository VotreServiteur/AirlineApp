using System.Xml;
using System.Xml.Linq;
using Planes;

namespace Airline;

public class PassengerPlane : Plane
{
    public int AmountOfPassengers { get; set; }
    public PassengerPlane(XmlElement xNode) : base(xNode)
    {
        AmountOfPassengers = Convert.ToInt32(xNode.LastChild?.InnerText);
    }

    public PassengerPlane(string name, int capacity, int loadCapacity, int crew, int fuelRate, int flightRange,
        int amountOfPassengers)
        : base(name, capacity, loadCapacity, crew, fuelRate, flightRange)
    {
        AmountOfPassengers = amountOfPassengers;
    }

    public PassengerPlane()
        : base()
    {
        InputMessage("amount of passengers");
        AmountOfPassengers = Convert.ToInt32(Console.ReadLine());
    }

    

    public override XElement CreateXmlNode()
    {
        var plane = base.CreateXmlNode();
        plane.Add(new XAttribute("type", "P"));
        plane.Add(new XElement("amountOfPassengers", AmountOfPassengers));
        return plane;
    }

    public override string ToString()
    {
        return base.ToString() + $"Amount if passengers: {AmountOfPassengers}\n";
    }

   
}