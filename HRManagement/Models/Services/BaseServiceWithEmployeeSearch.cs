using HRManagement.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public abstract class BaseServiceWithEmployeeSearch<ModelClass, DtoClass> 
        : BaseService<ModelClass, DtoClass>
        where ModelClass : class, new ()
        where DtoClass : class
    {
        public abstract List<EmployeeDTO> GetEmployeeModelsOfProperty(ModelClass model);
    }
}
