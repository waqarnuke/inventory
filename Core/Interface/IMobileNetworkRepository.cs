using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interface
{
    public interface IMobileNetworkRepository :IRepository<MobileNetwork>
{
    void Update(MobileNetwork mobileNetwork);
    bool IsExists(int id);
}
}