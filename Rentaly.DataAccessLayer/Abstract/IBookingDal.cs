using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface IBookingDal:IGenericDal<Booking>
    {
        Task<Booking> GetBookingWithDetailsByIdAsync(int id);
     
        Task<Booking> GetDetailsByIdAsync(int id);
    }
}
