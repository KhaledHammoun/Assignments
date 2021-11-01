using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Model;

namespace Server.Persistence.AdultPersistence
{
    public class AdultPersistence : IAdultPersistence
    {
        private FileContext fileContext;

        public AdultPersistence(FileContext fileContext)
        {
            this.fileContext = fileContext;
        }

        public async Task<Adult> AddAdultAsync(Adult adult)
        {
            if (adult == null) throw new NullReferenceException("Adult must not be null");
        
            try
            {
                Adult exists = fileContext.Adults.FirstOrDefault(a => a.Equals(adult));
                if (exists != null) throw new ArgumentException("Adult already exists");
                int newId = fileContext.Adults.Max(person => person.Id);
                adult.Id = ++newId;
                fileContext.Adults.Add(adult);
                fileContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return adult;
        }

        public async Task<IList<Adult>> GetAllAdultsAsync()
        {
            return fileContext.Adults.ToList();
        }

        public async Task RemoveAdultAsync(int id)
        {
            Adult adult = fileContext.Adults.FirstOrDefault(a => a.Id.Equals(id));
            fileContext.Adults.Remove(adult);
            fileContext.SaveChanges();
        }

        public async Task<Adult> GetAdultByIdAsync(int id)
        {
            Adult adultToReturn = fileContext.Adults.FirstOrDefault(a => a.Id.Equals(id));
            if (adultToReturn == null) throw new Exception("Adult does not exist!");
            return adultToReturn;
        }

        public async Task<Adult> EditAdultAsync(Adult adult)
        {
            fileContext.Adults[fileContext.Adults.ToList().FindIndex(index => index.Id.Equals(adult.Id))] = adult;
            fileContext.SaveChanges();
            return adult;
        }
    }
}