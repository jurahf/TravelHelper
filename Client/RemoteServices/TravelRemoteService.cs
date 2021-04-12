using Implementation.Model;
using Client.RemoteServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;

namespace Client.RemoteServices
{
    public class TravelRemoteService
    {
        private HttpClient client = null;

        public TravelRemoteService(HttpClient client)
        {
            this.client = client;
        }

        public List<Category> GetCategories()
        {
            string query = $"/api/Travel/GetAllCategories";

            HttpResponseMessage response = client.GetAsync(query).Result;

            if (response.IsSuccessStatusCode)
            {
                var categories = response.Content.ReadAsAsync<List<Category>>().Result;

                foreach (var cat in categories)
                {
                    if (cat.Parent != null)
                    {
                        var parent = categories
                            .FirstOrDefault(x => x.Id == cat.Parent.Id);

                        if (parent != null)
                        {
                            if (parent.Childs == null)
                                parent.Childs = new List<Category>();
                            parent.Childs.Add(cat);
                        }
                    }
                }

                return categories;
            }
            else
                return new List<Category>();  // todo: Exception?
        }

        public List<Travel> GetTravelsList(User currentUser)
        {
            string query = $"/api/Travel/GetTravelsList?login={currentUser.Login}";

            HttpResponseMessage response = client.GetAsync(query).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<List<Travel>>().Result;
            }
            else
                return new List<Travel>();  // todo: Exception?
        }

        public int? GetSelectedTravelId(User currentUser)
        {
            string query = $"/api/Travel/GetSelectedTravel?login={currentUser.Login}";

            HttpResponseMessage response = client.GetAsync(query).Result;

            if (response.IsSuccessStatusCode)
            {
                return response.Content.ReadAsAsync<int?>().Result;
            }
            else
                return null;  // todo: Exception?
        }


    }
}