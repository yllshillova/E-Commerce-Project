using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using API.RequesHelpers;

namespace API.Extensions
{
    public  static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse response, MetaData metaData){
            var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            response.Headers.Add("Pagination", JsonSerializer.Serialize(metaData,options));
            // kjo eshte per me u shfaq edhe ne client side si header perndryshe nuk funksionon portat ndryshe 3000 5000 !!!!
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
            
        }
    }
}