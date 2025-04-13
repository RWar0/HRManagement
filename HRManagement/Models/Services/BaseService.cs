using HRManagement.Models.Contexts;
using HRManagement.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.Services
{
    public abstract class BaseService<ModelClass, DtoClass>
        where ModelClass : class, new()
        where DtoClass : class
    {
        protected DatabaseContext Database {  get; set; }

        #region FieldsAndProperties
        // Sorting
        public string? OrderProperty { get; set; }
        public bool IsOrderDescending { get; set; }
        #endregion

        public BaseService() 
        {
            Database = new DatabaseContext();
        }

        #region Methods
        public abstract List<DtoClass> GetModels();
        public abstract ModelClass GetModel(int id);
        public abstract void AddModel(ModelClass model);
        public abstract void UpdateModel(ModelClass model);
        public abstract void DeleteModel(DtoClass model);
        public virtual ModelClass CreateModel()
        {
            return new ModelClass();
        }

        // Sorting
        public virtual List<SearchComboBoxDTO> GetOrderByComboBoxDtos() { return new(); }
        #endregion
    }
}
