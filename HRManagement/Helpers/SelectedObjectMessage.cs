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
    /// <typeparam name="SelectedObjectClass">Class of the object to be selected</typeparam>
    public class SelectedObjectMessage<SelectedObjectClass>
        where SelectedObjectClass : class
    {
        #region FieldsAndProperties
        public object WhoRequestedToSelect { get; set; } = default!;
        public SelectedObjectClass SelectedObject { get; set; } = default!;
        #endregion

        public SelectedObjectMessage(object whoRequestedToSelect, SelectedObjectClass selectedObject)
        {
            WhoRequestedToSelect = whoRequestedToSelect;
            SelectedObject = selectedObject;
        }

    }
}
