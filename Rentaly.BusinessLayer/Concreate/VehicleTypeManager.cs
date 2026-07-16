using Rentaly.BusinessLayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rentaly.BusinessLayer.Concreate
{
    public class VehicleTypeManager:IVehicleTypeService
    {
        private readonly IVehicleTypeDal _vehicleTypeDal;

        public VehicleTypeManager(IVehicleTypeDal vehicleTypeDal)
        {
            _vehicleTypeDal = vehicleTypeDal;
        }

        public async Task TDeleteAsync(int id)
        {
           await _vehicleTypeDal.DeleteAsync(id);
        }

        public async Task<VehicleType> TGetByIdAsync(int id)
        {
            return await _vehicleTypeDal.GetByIdAsync(id);
        }

        public async Task<List<VehicleType>> TGetListAsync()
        {
          return await _vehicleTypeDal.GetListAsync();
        }

        public async Task TInsertAsync(VehicleType entity)
        {
            await _vehicleTypeDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(VehicleType entity)
        {
           await _vehicleTypeDal.UpdateAsync(entity);
        }
    }
}
