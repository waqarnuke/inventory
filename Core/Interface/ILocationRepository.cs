using Core.Entities;

namespace Core.Interface
{
    public interface ILocationRepository :IRepository<Location>
{
    void Update(Location location);
    bool IsExists(int id);
}
}