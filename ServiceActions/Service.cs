using System.Collections.ObjectModel;
using Airline;
using DataObjects;
using Planes;

namespace ServiceActions;

public class Service : IService
{
    private static readonly IPlaneDao PlaneDao = new PlaneDao();
  

    public void Save()
    {
        PlaneDao.Save();
    }

    public void DeletePlane(bool isPassenger, Plane plane)
    {
        PlaneDao.DeletePlane(isPassenger, plane);
    }

    public void AddPlane(bool isPassenger, Plane plane)
    {
        PlaneDao.AddPlane(isPassenger, plane);
    }
}