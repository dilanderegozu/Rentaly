using Rentaly.Businesslayer.Abstract;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.Businesslayer.Concreate
{
    public class BranchManager : IBranchService
    {
        private readonly IBranchDal _branchDal;

        public BranchManager(IBranchDal branchDal)
        {
            _branchDal = branchDal;
        }

        public async Task<List<Branch>> TGetListAsync()
        {
            return await _branchDal.TGetListAsync();
        }

        public async Task<Branch> TGetByIdAsync(int id)
        {
            return await _branchDal.GetByIdAsync(id);
        }

        public async Task TInsertAsync(Branch entity)
        {
            await _branchDal.InsertAsync(entity);
        }

        public async Task TUpdateAsync(Branch entity)
        {
            await _branchDal.UpdateAsync(entity);
        }

        public async Task TDeleteAsync(int id)
        {
            await _branchDal.DeleteAsync(id);
        }
    }
}