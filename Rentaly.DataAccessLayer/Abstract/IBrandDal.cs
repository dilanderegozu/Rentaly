using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.DataAccessLayer.Abstract
{
    public interface IBrandDal :IGenericDal<Brand>
    {
        Task<List<Brand>> TGetListAsync();
    }
}
