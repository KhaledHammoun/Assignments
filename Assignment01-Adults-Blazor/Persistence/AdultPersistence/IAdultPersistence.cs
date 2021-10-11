using System.Collections.Generic;
using Assignment01_Adults_Blazor.Models;

namespace Assignment01_Adults_Blazor.Persistence.AdultPersistance
{
    public interface IAdultPersistence
    {
        void AddAdult(Adult adult);
        List<Adult> GetAllAdults();
        void RemoveAdult(int id);

        Adult GetAdultById(int id);
        void EditAdult(Adult adult);
    }
}