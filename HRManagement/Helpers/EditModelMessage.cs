using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="ClassOfModel">Class of Model to be edited</typeparam>
    public class EditModelMessage<ClassOfModel>
    {
        public object Sender { get; set; } = default!;
        public ClassOfModel ModelToEdit { get; set; } = default!;

        public EditModelMessage(object sender, ClassOfModel modelToEdit)
        {
            Sender = sender;
            ModelToEdit = modelToEdit;
        }
    }
}
