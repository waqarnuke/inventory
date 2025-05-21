using System;
using Core.Entities;

namespace Core.Interface;

public interface IUserLocationAssignmentsRepository :IRepository<UserLocationAssignment>
{
    void Update(UserLocationAssignment userLocationAssignment);
    bool IsExists(int id);
}   
