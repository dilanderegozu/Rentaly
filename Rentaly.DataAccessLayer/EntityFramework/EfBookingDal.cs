using Microsoft.EntityFrameworkCore;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.RepositoryDesignPattern;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.EntityFramework
{
    public class EfBookingDal : GenericRepository<Booking>, IBookingDal
    {
        public EfBookingDal(RentalyContext context) : base(context)
        {
        }
       
        public async Task<Booking> GetBookingWithDetailsByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Car).ThenInclude(c => c.Brand)
                .Include(b => b.Car).ThenInclude(c => c.CarModel)
                .Include(b => b.PickUpBranch)
                .Include(b => b.DropOffBranch)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }

        public Task<Booking> GetCarWithDetailsByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<Booking> GetDetailsByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(b => b.Car).ThenInclude(c => c.Brand)
                .Include(b => b.Car).ThenInclude(c => c.CarModel)
                .Include(b => b.Car).ThenInclude(c => c.Category)
                .Include(b => b.PickUpBranch)
                .Include(b => b.DropOffBranch)
                .FirstOrDefaultAsync(b => b.BookingId == id);
        }
    }
}
