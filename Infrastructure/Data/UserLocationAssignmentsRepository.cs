using System;
using Core.Entities;
using Core.Interface;

namespace Infrastructure.Data;

public class UserLocationAssignmentsRepository : Repository<UserLocationAssignment>, IUserLocationAssignmentsRepository
{
    private readonly StoreContext _context;
    public UserLocationAssignmentsRepository(StoreContext context) : base(context)
    {
        _context = context;
    }

    public bool IsExists(int id)
    {
        return _context.Brands.Any(x => x.Id == id); 
    }

    public void Update(UserLocationAssignment userLocationAssignment)
    {
        _context.UserLocationAssignments.Update(userLocationAssignment);
    }
}
