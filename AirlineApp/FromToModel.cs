using System;
using System.ComponentModel;

namespace AirlineApp;

public class FromToModel : IDataErrorInfo
{
    public string Error { get; }
    public uint From { get; set; }
    public uint To { get; set; }
    public string this[string columnName]
    {
        get {
            string error = String.Empty;
            switch (columnName)
            {
                case "From":
                    if (From < 0)
                    {
                        error = "Incorrect";
                    }
                    
                    break;
                case "To":
                    if (To < 0)
                    {
                        error = "Incorrect";
                    }
                    
                    break;
            }

            return error;
        }
    }
}