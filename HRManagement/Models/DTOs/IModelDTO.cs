using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Models.DTOs
{
    public interface IModelDTO : IBaseModelDTO
    {
        public string Title { get; set; }
    }
}
