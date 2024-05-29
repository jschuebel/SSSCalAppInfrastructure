using System.Collections.Generic;
using System.Linq;
using SSSCalApp.Core.DomainService;
using coreevent = SSSCalApp.Core.Entity;
using Microsoft.EntityFrameworkCore;

using SSSCalApp.Infrastructure.DataContext;

namespace SSSCalApp.Infrastructure.Repositories
{
    public class EventRepository: IEventRepository
    {

      readonly PersonContext _ctx;

        public EventRepository(PersonContext ctx)
        {
            _ctx = ctx;
        }
        
        public coreevent.Event Create(coreevent.Event evt)
        {
            /*/
            if (Event.Type != null)
            {
                _ctx.Attach(Event.Type).State = EntityState.Unchanged;
            }*/
            var EventSaved = _ctx.Events.Add(evt).Entity;
            _ctx.SaveChanges();
            return EventSaved;
        }

        public coreevent.Event GetEventById(int id)
        {
            var item = (from evt in _ctx.Events
            .Include(c => c.topicf)
            join p in _ctx.People on evt.UserId equals p.Id
            where evt.Id==id
            select evt.Copy(p)).FirstOrDefault();

            return item;

//            return _ctx.Events
//               .Include(c => c.topicf)
//                .FirstOrDefault(c => c.Id == id);
        }

        public List<coreevent.Person> GetEventByIdWithPeople(int id)
        {
            return _ctx.Groups
                .Where(x => x.EventId == id)
                .Select(x => x.People).ToList();
        }

        public IEnumerable<coreevent.Event> ReadAll()
        {
            //Create a Filtered List
            var filteredList = from evt in _ctx.Events
                   .AsNoTracking()
                   .Include(c => c.topicf)
                    join p in _ctx.People on evt.UserId equals p.Id
                    select evt.Copy(p);

             /* If there is a Filter then filter the list and set Count
            if (filter != null && filter.ItemsPrPage > 0 && filter.CurrentPage > 0)
            {
                filteredList = _ctx.General
                    .Include(c => c.Type)
                    .Skip((filter.CurrentPage - 1) * filter.ItemsPrPage)
                    .Take(filter.ItemsPrPage);
                filteredList.Count = _ctx.Events.Count();
                return filteredList;
            }
            
            //Else just return the full list and get the count from the list (to save a SQL call)
            filteredList.List = _ctx.Events
                    .Include(c => c.Type);
            filteredList.Count = filteredList.List.Count();
            */
            return filteredList;
        }
        
        public coreevent.Event Update(coreevent.Event EventUpdate)
        {
            if (EventUpdate.Createdate == null || (EventUpdate.Createdate != null && EventUpdate.Createdate.Year==1)) EventUpdate.Createdate = System.DateTime.Now;
            _ctx.Attach(EventUpdate).State = EntityState.Modified;
       /*     _ctx.Entry(EventUpdate).Collection(c => c.Orders).IsModified = true;
            _ctx.Entry(EventUpdate).Reference(c => c.Type).IsModified = true;
            var orders = _ctx.Orders.Where(o => o.Event.Id == EventUpdate.Id
                                   && !EventUpdate.Orders.Exists(co => co.Id == o.Id));
            foreach (var order in orders)
            {
                order.Event = null;
                _ctx.Entry(order).Reference(o => o.Event)
                    .IsModified = true;
            }
            */
            _ctx.SaveChanges();
            return EventUpdate;
        }

        public bool Delete(int id)
        {
            /*var ordersToRemove = _ctx.Orders.Where(o => o.Event.Id == id);
            _ctx.RemoveRange(ordersToRemove);*/
            var evt = _ctx.Events.FirstOrDefault(x=>x.Id==id);
            if (evt==null) return false;

            //var custRemoved = _ctx.Remove(new coreevent.Event {Id = id}).Entity;
            var custRemoved = _ctx.Remove(evt).Entity;
            _ctx.SaveChanges();
            return true; //custRemoved;
        }

        public int Count()
        {
            return _ctx.Events.Count();
        }        
    }
}