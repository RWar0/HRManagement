using HRManagement.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagement.Helpers
{
    public class OpenViewMessage
    {
        public object Sender { get; set; } = default!;
        public WorkspaceViewModel ViewModelToBeOpened { get; set; } = default!;

    }
}
