using Planes;

namespace DataObjects;

public class PlaneDao:IPlaneDao
{
    private DataBase db = DataBase.GetInstance();
    public Plane GetPlane()
    {
        throw new NotImplementedException();
    }

    public void Save()
    {
        db.Saving();
    }

    public void DeletePlane(bool isPassenger, Plane plane)
    {
        db.DeletePlane(isPassenger, plane);
    }

    public void AddPlane(bool isPassenger, Plane plane)
    {
        db.AddPlane(isPassenger, plane);
    }
}