using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineRestaurant.Database.Enums
{
    public enum OrderState
    {
        InvalidOrderState = 0,
        Registered = 1,
        InPreparation = 2,
        Delievered = 4,
        Canceled = 8
    }
}
