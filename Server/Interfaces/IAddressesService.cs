using Server.Classes.Args;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Server.Interfaces
{
    /// <summary>
    /// Интерфейс сервиса для поиска мест и адресов
    /// </summary>
    public interface IAddressesService
    {
        List<NaviAddressInfo> SearchAddresses(AddressSearchArgs args);

        List<NaviAddressInfo> SearchNear(AddressSearchNearArgs args);
    }
}