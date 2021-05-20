using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CoreImplementation.Args;
using CoreImplementation.Model;


namespace CoreImplementation.ServiceInterfaces
{
    /// <summary>
    /// Интерфейс сервиса для поиска мест и адресов
    /// </summary>
    public interface IAddressesService
    {
        VMAddressInfo GetAddressInfo(int id);








        List<VMAddressInfo> SearchAddresses(AddressSearchArgs args);

        List<VMAddressInfo> SearchNear(AddressSearchNearArgs args);
    }
}