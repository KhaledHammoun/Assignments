using System;
using System.Collections.Generic;
using System.Linq;
using Assignment01_Adults_Blazor.Models;
using Assignment01_Adults_Blazor.Persistence.AdultPersistance;

namespace Assignment01_Adults_Blazor.Persistence.AdultPersistence
{
    public class AdultPersistence : IAdultPersistence
    {
        private FileContext fileContext;

        public AdultPersistence(FileContext fileContext)
        {
            this.fileContext = fileContext;
        }

        public void AddAdult(Adult adult)
        {
            if (adult == null) throw new NullReferenceException("Adult must not be null");
        
            try
            {
                Adult exists = fileContext.Adults.FirstOrDefault(a => a.Equals(adult));
                if (exists != null) throw new ArgumentException("Adult aready exists");
                int newId = fileContext.Adults.Max(person => person.Id);
                adult.Id = ++newId;
                fileContext.Adults.Add(adult);
                fileContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public List<Adult> GetAllAdults()
        {
            return fileContext.Adults.ToList();
        }

        public void RemoveAdult(int id)
        {
            Adult adult = fileContext.Adults.FirstOrDefault(a => a.Id.Equals(id));
            fileContext.Adults.Remove(adult);
            fileContext.SaveChanges();
        }
    }
}