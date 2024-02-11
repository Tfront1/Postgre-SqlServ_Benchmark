using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Enums.Operations
{
    public enum InsertType
    {
        Common = 1,
        Bulk = 2,
        TransactionBalk = 3,
    }
}
