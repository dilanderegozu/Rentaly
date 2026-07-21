using Rentaly.EntityLayer.Entities;

namespace Rentaly.Businesslayer.Abstract
{
    public interface IBranchService : IGenericService<Branch>
    {
        Task<List<Branch>> TGetListAsync();
    }
}