using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Server.Persistence.AdultPersistence
{
    public interface IAdultPersistence
    {
        Task<Adult> AddAdultAsync(Adult adult);
        Task<IList<Adult>> GetAllAdultsAsync();
        Task RemoveAdultAsync(int id);

        Task<Adult> GetAdultByIdAsync(int id);
        Task<Adult> EditAdultAsync(Adult adult);
    }
}