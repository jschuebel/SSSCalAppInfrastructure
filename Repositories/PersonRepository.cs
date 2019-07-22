using System.Collections.Generic;
using System.Linq;
using SSSCalApp.Core.DomainService;
using SSSCalApp.Core.Entity;
using Microsoft.EntityFrameworkCore;

using SSSCalApp.Infrastructure.DataContext;

namespace SSSCalApp.Infrastructure.Repositories
{
    public class PersonRepository: IPersonRepository
    {

      readonly PersonContext _ctx;

        public PersonRepository(PersonContext ctx)
        {
            _ctx = ctx;
        }
        
        public Person Create(Person Person)
        {
            /*/
            if (Person.Type != null)
            {
                _ctx.Attach(Person.Type).State = EntityState.Unchanged;
            }*/
            var PersonSaved = _ctx.People.Add(Person).Entity;
            _ctx.SaveChanges();
            return PersonSaved;
        }

        public Person GetById(int id)
        {
            var item = (from p in _ctx.People
                .Include(c => c.Address)
                join evt in _ctx.Events.Where(x=>x.TopicId==1) on p.Id equals evt.UserId
                where p.Id == evt.UserId
                select p.Copy(evt)).FirstOrDefault();

/*
                var evt = _ctx.Events.FirstOrDefault(x=>x.UserId==id);
                var p = _ctx.People
                .Include(c => c.Address)
//                .Include(c => c.Events)
                .FirstOrDefault(c => c.Id == id);
                if (evt!=null)
                    p.Events.Add(evt);
*/                    
            return item;
        }

        public Person ReadyByIdIncludeOrders(int id)
        {
            return _ctx.People
  //              .Include(c => c.Type)
   //             .Include(c => c.Orders)
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Person> ReadAll()
        {
            //Create a Filtered List
            var filteredList = from p in _ctx.People
                    join evt in _ctx.Events.Where(x=>x.TopicId==1) on p.Id equals evt.UserId
                    select p.Copy(evt);

            /* If there is a Filter then filter the list and set Count
            if (filter != null && filter.ItemsPrPage > 0 && filter.CurrentPage > 0)
            {
                filteredList = _ctx.General
                    .Include(c => c.Type)
                    .Skip((filter.CurrentPage - 1) * filter.ItemsPrPage)
                    .Take(filter.ItemsPrPage);
                filteredList.Count = _ctx.Persons.Count();
                return filteredList;
            }
            
            //Else just return the full list and get the count from the list (to save a SQL call)
            filteredList.List = _ctx.Persons
                    .Include(c => c.Type);
            filteredList.Count = filteredList.List.Count();
            */
            return filteredList;
        }
        
        public Person Update(Person PersonUpdate)
        {
            _ctx.Attach(PersonUpdate).State = EntityState.Modified;
       /*     _ctx.Entry(PersonUpdate).Collection(c => c.Orders).IsModified = true;
            _ctx.Entry(PersonUpdate).Reference(c => c.Type).IsModified = true;
            var orders = _ctx.Orders.Where(o => o.Person.Id == PersonUpdate.Id
                                   && !PersonUpdate.Orders.Exists(co => co.Id == o.Id));
            foreach (var order in orders)
            {
                order.Person = null;
                _ctx.Entry(order).Reference(o => o.Person)
                    .IsModified = true;
            }
            */
            _ctx.SaveChanges();
            return PersonUpdate;
        }

        public bool Delete(int id)
        {
            /*var ordersToRemove = _ctx.Orders.Where(o => o.Person.Id == id);
            _ctx.RemoveRange(ordersToRemove);*/
            var per = _ctx.People.FirstOrDefault(x=>x.Id==id);
            if (per==null) return false;
            var custRemoved = _ctx.Remove(per).Entity;
            _ctx.SaveChanges();
            return true; //custRemoved;
        }

        public int Count()
        {
            return _ctx.People.Count();
        }        
    }
}