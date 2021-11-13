using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Model;

namespace Server.Persistence.AdultPersistence
{
    public class AdultPersistence : IAdultPersistence
    {
        //private FileContext fileContext;

        // public AdultPersistence(FileContext fileContext)
        // {
        //     this.fileContext = fileContext;
        // }

        public async Task<Adult> AddAdultAsync(Adult adult)
        {
            if (adult == null) throw new NullReferenceException("Adult must not be null");
        
            try
            {
                await using FamilyContext familyContext = new FamilyContext();
                Adult exists =await familyContext.Adults.FirstOrDefaultAsync(a => a.Equals(adult));
                if (exists != null) throw new ArgumentException("Adult already exists");
                await familyContext.Adults.AddAsync(adult);
                await familyContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return adult;
        }

        public async Task<IList<Adult>> GetAllAdultsAsync()
        {
            await using FamilyContext familyContext = new FamilyContext();
            var adults = familyContext.Persons.OfType<Adult>().Include(a => a.JobTitle).ToList();
            return adults;
        }

        public async Task RemoveAdultAsync(int id)
        {
            await using FamilyContext familyContext = new FamilyContext();
            Adult adult = await familyContext.Adults.FirstOrDefaultAsync(a => a.Id.Equals(id));
            familyContext.Adults.Remove(adult);
            await familyContext.SaveChangesAsync();
        }

        public async Task<Adult> GetAdultByIdAsync(int id)
        {
            await using FamilyContext familyContext = new FamilyContext();
            Adult adultToReturn = await familyContext.Adults.FirstOrDefaultAsync(a => a.Id.Equals(id));
            if (adultToReturn == null) throw new Exception("Adult does not exist!");
            return adultToReturn;
        }

        public async Task<Adult> EditAdultAsync(Adult adult)
        {
            await using FamilyContext familyContext = new FamilyContext();
            var firstOrDefaultAsync = familyContext.Adults.FirstOrDefaultAsync(a => a.Equals(adult));
            if (firstOrDefaultAsync == null) throw new ArgumentException("Invalid adult!");
            familyContext.Adults.Update(adult);
            await familyContext.SaveChangesAsync();
            return adult;
        }
    }
}