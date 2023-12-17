using System.Collections.ObjectModel;
using Airline;
using Planes;

namespace ServiceActions;

public interface IService
{
   public void Save();
    public void DeletePlane(bool isPassenger, Plane plane);
    public void AddPlane(bool isPassenger, Plane plane);
    
}