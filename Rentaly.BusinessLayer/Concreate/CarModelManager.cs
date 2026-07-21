using Rentaly.Businesslayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rentaly.Businesslayer.Concreate
{
    public class CarModelManager : ICarModelService
    {
        private readonly ICarModel _carModelDal;

        public CarModelManager(ICarModel carModelDal)
        {
            _carModelDal = carModelDal;
        }

        public async Task TDeleteAsync(int id)
        {
            await _carModelDal.DeleteAsync(id);
        }

        public async Task<CarModel> TGetByIdAsync(int id)
        {
           return await _carModelDal.GetByIdAsync(id);
        }

        public async Task<List<CarModel>> TGetListAsync()
        {
            return await _carModelDal.TGetListAsync();  
        }

        public async Task TInsertAsync(CarModel entity)
        {
            await _carModelDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(CarModel entity)
        {
            await _carModelDal.UpdateAsync(entity);
        }
    }
}
