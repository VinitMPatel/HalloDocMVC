using Data.DataContext;
using Microsoft.AspNetCore.Hosting;
using Services.Contracts;
using Services.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementation
{
    public class ProviderServices : IProviderServices
    {
        private readonly HalloDocDbContext _context;
        private readonly IHostingEnvironment _env;

        public ProviderServices(HalloDocDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public EditProviderViewModel CreateProvider()
        {
            EditProviderViewModel obj = new EditProviderViewModel();
            obj.regionList = _context.Regions.ToList();
            return obj;
        }
    }
}
