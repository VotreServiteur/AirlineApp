using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace AirlineApp;

public class PlaneModel : IDataErrorInfo
{
    public string Name { get; set; } = "Name";

    public int Capacity { get; set; }
    public int LoadCapacity { get; set; }

    public int Crew { get; set; }

    public int FuelRate { get; set; }
    public int FlightRange { get; set; }
    public int AmountOfPassengers { get; set; } = 1;

    public int LoadPrice { get; set; }
    public bool IsPassenger { get; set; }

    public string Error
    {
        get { throw new NotImplementedException(); }
    }

    
    
    public string this[string columnName]
    {
        get
        {
            string error = String.Empty;
            switch (columnName)
            {
                case "Name":
                    if (Name.Equals("") || Name == null)
                    {
                        error = "String can not be empty";
                    }
                    else
                    {
                        if (!Regex.IsMatch(Name, "\\b+\\d*") || Name.Length > 20 || Name.Length < 3)
                        {
                            error = "Wrong format";
                        }
                    }

                    break;
                case "Capacity":
                    if (Capacity < 20 || Capacity > 200)
                    {
                        error = "Wrong number";
                    }

                    break;
                case "LoadCapacity":
                    if (LoadCapacity < 20 || LoadCapacity > 50)
                    {
                        error = "Wrong number (< 20 or > 50)";
                    }

                    break;
                case "Crew":
                    if (Crew <= 1 || Crew > 10)
                    {
                        error = "Wrong number";
                    }

                    break;
                case "FuelRate":
                    if (FuelRate < 200 || FuelRate > 1000)
                    {
                        error = "Wrong number";
                    }

                    break;
                case "FlightRange":
                    if (FlightRange < 20 || FlightRange > 20000)
                    {
                        error = "Wrong number";
                    }

                    break;
                case "AmountOfPassengers":
                    if (AmountOfPassengers <= 0 || AmountOfPassengers > 250)
                    {
                        error = "Wrong number";
                    }

                    break;

                case "LoadPrice":
                    if (LoadPrice is < 0 or > 25000)
                    {
                        error = "Wrong number";
                    }

                    break;
            }

            return error;
        }
    }
}