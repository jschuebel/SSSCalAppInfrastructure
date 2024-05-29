using System.Collections.Generic;
using System.Linq;
using SSSCalApp.Core.DomainService;
using SSSCalApp.Core.Entity;
using Microsoft.EntityFrameworkCore;

using SSSCalApp.Infrastructure.DataContext;

namespace SSSCalApp.Infrastructure.Repositories
{
    public class GroupRepository: IGroupRepository
    {

      readonly PersonContext _ctx;

        public GroupRepository(PersonContext ctx)
        {
            _ctx = ctx;
        }
        

        public IEnumerable<Group> ReadyById(int id)
        {
            return _ctx.Groups
                .Include(c => c.People)
                .Where(c => c.EventId == id);
        }

        public IEnumerable<Group> ReadAll()
        {
            //Create a Filtered List
            var filteredList = _ctx.Groups;
            /* If there is a Filter then filter the list and set Count
            if (filter != null && filter.ItemsPrPage > 0 && filter.CurrentPage > 0)
            {
                filteredList = _ctx.General
                    .Include(c => c.Type)
                    .Skip((filter.CurrentPage - 1) * filter.ItemsPrPage)
                    .Take(filter.ItemsPrPage);
                filteredList.Count = _ctx.Addresss.Count();
                return filteredList;
            }
            
            //Else just return the full list and get the count from the list (to save a SQL call)
            filteredList.List = _ctx.Addresss
                    .Include(c => c.Type);
            filteredList.Count = filteredList.List.Count();
            */
            return filteredList;
        }
        

        public int Count()
        {
            return _ctx.Groups.Count();
        }        
    }
}