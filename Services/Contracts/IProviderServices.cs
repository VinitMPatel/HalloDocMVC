using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IProviderServices
    {
        EditProviderViewModel CreateProvider();
        Task CreateProviderAccount(EditProviderViewModel obj, List<int> selectedRegion, int adminId);
    }
}
