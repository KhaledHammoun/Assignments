using System.Collections.Generic;
using System.Threading.Tasks;
using Model;

namespace Client.Data.AdultService
{
    public interface IAdultService
    {
        Task<IList<Adult>> GetAllAdultsAsync();
        Task<Adult> GetAdultByIdAsync(int id);
        Task<Adult> AddAdultAsync(Adult adult);
        Task DeleteAdultAsync(int id);
        Task<Adult> EditAdultAsync(Adult adult);
    }
}