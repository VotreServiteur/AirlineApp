using System.Xml;
using System.Xml.Linq;

namespace Airline;
using static System.Convert;

public abstract class Plane
{
    public XmlElement CurNode { get; set; }
    public string Name { get; set; }
    
    public int Capacity { get; set; }
    public int LoadCapacity { get; set; }
    public static int TotalCapacity { get; set; } = 0;
    public static int TotalLoadCapacity { get; set; } = 0;

    public int Crew { get; set; }
    
    public int FuelRate { get; set; }
    public int FlightRange { get; set; }
    
    protected Plane(XmlElement xNode) :
        this(xNode.ChildNodes[0]?.InnerText,
            ToInt32(xNode.ChildNodes[1]?.InnerText),
            ToInt32(xNode.ChildNodes[2]?.InnerText),
            ToInt32(xNode.ChildNodes[3]?.InnerText),
            ToInt32(xNode.ChildNodes[4]?.InnerText),
            ToInt32(xNode.ChildNodes[5]?.InnerText)
        )
    { }

    protected Plane(string name, int capacity, int loadCapacity, int crew, int fuelRate, int flightRange)
    {
        
        Name = name;
        Capacity = capacity;
        LoadCapacity = loadCapacity;
        Crew = crew;
        FuelRate = fuelRate;
        FlightRange = flightRange;
        
        TotalCapacity += Capacity;
        TotalLoadCapacity += LoadCapacity;
    }

    protected Plane()
    {
        InputMessage("plane name");
        Name = Console.ReadLine() ?? "Plane";
        InputMessage("capacity");
        Capacity = Convert.ToInt32(Console.ReadLine());
        ;
        InputMessage("load capacity");
        LoadCapacity = Convert.ToInt32(Console.ReadLine());
        ;
        InputMessage("number of crew");
        Crew = Convert.ToInt32(Console.ReadLine());
        ;
        InputMessage("fuel rate");
        FuelRate = Convert.ToInt32(Console.ReadLine());
        ;
        InputMessage("flight range");
        FlightRange = Convert.ToInt32(Console.ReadLine());
        ;

        TotalCapacity += Capacity;
        TotalLoadCapacity += LoadCapacity;

    }

    public virtual XElement CreateXmlNode()
    {
        XElement plane = new("plane");
        plane.Add(new XElement("name",Name));
        plane.Add(new XElement("capacity",Capacity));
        plane.Add(new XElement("loadCapacity", LoadCapacity));
        plane.Add(new XElement("crew",Crew));
        plane.Add(new XElement("fuelRate", FuelRate));
        plane.Add(new XElement("flightRange", FlightRange));

        return plane;
    }
    public void InputMessage(string msg) =>
        Console.Write($"Input {msg}\n\t> ");

    public override string ToString()
    {
        return $" Plane: {Name}\n\tCapacity: {Capacity}\n\tLoadCapacity: {LoadCapacity}\n\tNumber of crew: {Crew}\n\tFuel rate:{FuelRate}\n\tFlight range:{FlightRange}\n\t";
    }
    
}