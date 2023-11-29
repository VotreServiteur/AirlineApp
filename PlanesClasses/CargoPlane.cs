using System.Xml;
using System.Xml.Linq;

namespace Airline;
[Serializable]
public class CargoPlane : Plane
{
   

    public int LoadPrice { get; set; }

    public CargoPlane(string name, int capacity, int loadCapacity, int crew, int fuelRate, int flightRange, int loadPrice) 
        : base( name, capacity,  loadCapacity, crew, fuelRate, flightRange)
    {
        LoadPrice = loadPrice;
    }
    public CargoPlane(XmlElement xNode) : base(xNode)
    {
        LoadPrice = Convert.ToInt32(xNode.LastChild?.InnerText);
    }
    public CargoPlane()
        : base()
    {
        InputMessage("load price");
        LoadPrice = Convert.ToInt32(Console.ReadLine());

    }
    public override XElement CreateXmlNode()
    {
        XElement plane = base.CreateXmlNode();
        plane.Add(new XAttribute("type", "C"));
        plane.Add(new XElement("LoadPrice",LoadPrice));
        return plane;
    }
    public override string ToString()
    {
        return base.ToString() + $"Load price: {LoadPrice}\n";
    }
}