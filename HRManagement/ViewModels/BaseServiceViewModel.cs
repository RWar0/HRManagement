using HRManagement.Models.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.ViewModels
{
    public class BaseServiceViewModel<ServiceClass, ModelClass, DtoClass> : WorkspaceViewModel
        where ServiceClass : BaseService<ModelClass, DtoClass>, new()
        where ModelClass : class, new()
        where DtoClass : class
    {

        public ServiceClass Service { get; set; }

        public BaseServiceViewModel(string displayName) : base(displayName)
        {
            Service = new ServiceClass();
        }
    }
}
