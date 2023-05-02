using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Enumerations
{
    public enum PaymentMethod
    {
        CB = 1, 
        Cash = 2, 
        DirectDebit = 3,
        Transfer = 4, 
        InternalTransfer = 5
    }
}
