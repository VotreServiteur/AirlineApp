using Planes;

namespace DataObjects;

public interface IPlaneDao
{
    Plane GetPlane();
    void Save();
    void DeletePlane(bool isPassenger, Plane plane);
    void AddPlane(bool isPassenger, Plane plane);
}