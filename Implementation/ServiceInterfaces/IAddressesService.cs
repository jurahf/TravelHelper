using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Implementation.Args;
using Implementation.Model;


namespace Implementation.ServiceInterfaces
{
    /// <summary>
    /// Интерфейс сервиса для поиска мест и адресов
    /// </summary>
    public interface IAddressesService
    {
        List<VMAddressInfo> SearchAddresses(AddressSearchArgs args);

        List<VMAddressInfo> SearchNear(AddressSearchNearArgs args);
    }
}