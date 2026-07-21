using Rentaly.Businesslayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.Abstract
{
    public interface IBookingService:IGenericService<Booking>
    {
        Task<Booking> GetBookingWithDetailsByIdAsync(int id);
        Task<Booking> GetDetailsByIdAsync(int id);
    }
}
