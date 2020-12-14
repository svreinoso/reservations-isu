using System;
using System.Collections.Generic;
using System.Text;

namespace Reservation.Data.Interfaces
{
    public interface ITablesAudit
    {
        DateTimeOffset CreatedDate { get; set; }
        DateTimeOffset UpdatedDate { get; set; }
    }
}
