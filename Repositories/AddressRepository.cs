using System.Collections.Generic;
using System.Linq;
using SSSCalApp.Core.DomainService;
using SSSCalApp.Core.Entity;
using Microsoft.EntityFrameworkCore;

using SSSCalApp.Infrastructure.DataContext;

namespace SSSCalApp.Infrastructure.Repositories
{
    public class AddressRepository: IAddressRepository
    {

      readonly PersonContext _ctx;

        public AddressRepository(PersonContext ctx)
        {
            _ctx = ctx;
        }
        
        public Address Create(Address Address)
        {
            /*/
            if (Address.Type != null)
            {
                _ctx.Attach(Address.Type).State = EntityState.Unchanged;
            }*/
            var AddressSaved = _ctx.Addresses.Add(Address).Entity;
            _ctx.SaveChanges();
            return AddressSaved;
        }

        public Address ReadyById(int id)
        {
            return _ctx.Addresses
//                .Include(c => c.Type)
                .FirstOrDefault(c => c.Id == id);
        }

        public Address ReadyByIdIncludeOrders(int id)
        {
            return _ctx.Addresses
  //              .Include(c => c.Type)
   //             .Include(c => c.Orders)
                .FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<Address> ReadAll()
        {
            //Create a Filtered List
            var filteredList = _ctx.Addresses;
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
        
        public Address Update(Address AddressUpdate)
        {
            _ctx.Attach(AddressUpdate).State = EntityState.Modified;
       /*     _ctx.Entry(AddressUpdate).Collection(c => c.Orders).IsModified = true;
            _ctx.Entry(AddressUpdate).Reference(c => c.Type).IsModified = true;
            var orders = _ctx.Orders.Where(o => o.Address.Id == AddressUpdate.Id
                                   && !AddressUpdate.Orders.Exists(co => co.Id == o.Id));
            foreach (var order in orders)
            {
                order.Address = null;
                _ctx.Entry(order).Reference(o => o.Address)
                    .IsModified = true;
            }
            */
            _ctx.SaveChanges();
            return AddressUpdate;
        }

        public bool Delete(int id)
        {
            /*var ordersToRemove = _ctx.Orders.Where(o => o.Address.Id == id);
            _ctx.RemoveRange(ordersToRemove);*/
            var custRemoved = _ctx.Remove(new Address {Id = id}).Entity;
            _ctx.SaveChanges();
            return true; //custRemoved;
        }

        public int Count()
        {
            return _ctx.Addresses.Count();
        }        
    }
}