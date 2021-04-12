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
        List<AddressInfo> SearchAddresses(AddressSearchArgs args);

        List<AddressInfo> SearchNear(AddressSearchNearArgs args);
    }
}