using HRManagement.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public abstract class BaseManyToManyTableService<ModelClass, DtoClass, ChildPartialClass>
        : BaseService<ModelClass, DtoClass>
        where ModelClass : class, new()
        where DtoClass : class
        where ChildPartialClass : class
    {
        public abstract void DeleteModel(ModelClass model);
        public abstract ModelClass GetModelForSpecificParentAndEmployee(ChildPartialClass parentModel, Employee employee);
    }
}
