using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.Concreate
{
    public class BookingManager:IBookingService
    {
        private readonly IBookingDal _bookingDal;
        public BookingManager(IBookingDal bookingDal)
        {
            _bookingDal = bookingDal;
        }

        public async Task TDeleteAsync(int id)
        {
           await _bookingDal.DeleteAsync(id);
        }

        public async Task<Booking> TGetByIdAsync(int id)
        {
          return await _bookingDal.GetByIdAsync(id);
        }

        public async Task<List<Booking>> TGetListAsync()
        {
            return await _bookingDal.GetListAsync();
        }

        public async Task TInsertAsync(Booking entity)
        {
           await _bookingDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Booking entity)
        {
            await _bookingDal.UpdateAsync(entity);
        }
        public async Task<Booking> GetBookingWithDetailsByIdAsync(int id)
        {
            return await _bookingDal.GetBookingWithDetailsByIdAsync(id);
        }
     
        public async Task<Booking> GetDetailsByIdAsync(int id)
        {
            return await _bookingDal.GetDetailsByIdAsync(id);
        }
    }
}
