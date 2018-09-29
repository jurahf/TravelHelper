using Server.Controllers;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Server.Classes
{
    public class NaviLoadHelper
    {
        private readonly DBWork data;
        private readonly AddressesController addressController;
        private readonly CommonParseHelper parseHelper;

        public NaviLoadHelper(DBWork data, AddressesController addressController)
        {
            this.data = data;
            this.addressController = addressController;
        }

        public List<NaviAddressInfo> LoadAdditionalInfoParallel(List<NaviAddressInfo> addressesList)
        {
            int degreeOfParallelism = 10;
            int addrPerTask = Math.Max(addressesList.Count / degreeOfParallelism, 1);
            List<Task<List<NaviAddressInfo>>> taskList = new List<Task<List<NaviAddressInfo>>>();

            for (int i = 0; i < degreeOfParallelism; i++)
            {
                var innerList = addressesList.Skip(addrPerTask * i).Take(addrPerTask).ToList();
                taskList.Add(Task.Factory.StartNew(
                    () => LoadAdditionalInfoInner(innerList))
                );
            }

            Task.WaitAll(taskList.ToArray());
            return taskList.SelectMany(t => t.Result).ToList();
        }

        private List<NaviAddressInfo> LoadAdditionalInfoInner(List<NaviAddressInfo> addressesList)
        {
            List<NaviAddressInfo> resultList = new List<NaviAddressInfo>();

            foreach (var address in addressesList)
            {
                NaviAddressInfo additionalInfo = null;
                try
                {
                    // пробуем поискать сначала по базе, может сохраняли уже такой адрес
                    //additionalInfo = data.GetFromDatabase<NaviAddressInfo>(x => x.ContainerAddress == address.ContainerAddress && x.SelfAddress == address.SelfAddress).FirstOrDefault();

                    if (additionalInfo == null)
                        additionalInfo = addressController.GetAddressInfo(address.ContainerAddress, address.SelfAddress);
                }
                catch
                {
                }

                resultList.Add(additionalInfo ?? address);
            }

            return resultList;
        }

        public NaviAddressInfo LoadAdditionalInfoSingle(NaviAddressInfo address)
        {
            NaviAddressInfo additionalInfo = null;
            try
            {
                // пробуем поискать сначала по базе, может сохраняли уже такой адрес
                additionalInfo = data.GetFromDatabase<NaviAddressInfo>(x => x.ContainerAddress == address.ContainerAddress && x.SelfAddress == address.SelfAddress).FirstOrDefault();

                if (additionalInfo == null)
                    additionalInfo = addressController.GetAddressInfo(address.ContainerAddress, address.SelfAddress);
            }
            catch
            {
            }

            return additionalInfo ?? address;
        }
    }
}