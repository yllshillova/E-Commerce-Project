using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;

namespace API.Extensions
{
    // e bojm static qe mos me kriju instanc t ksaj klase per me perdor dikun tjter
    public static class ProductExtensions
    {
        public static IQueryable<Product> Sort(this IQueryable<Product> query, string orderBy)
        {
            if(string.IsNullOrWhiteSpace(orderBy)) return query.OrderBy(p => p.Name);
             query = orderBy switch
            {
                "price" => query.OrderBy(p => p.Price),
                "priceDesc" => query.OrderByDescending(p => p.Price),
                _ => query.OrderBy(p => p.Name)
            };
            return query;
        }
        public static IQueryable<Product> Search(this IQueryable<Product> query, string searchTerm)
        {
            if(string.IsNullOrWhiteSpace(searchTerm)) return query;
            var lowerCaseSearchTerm = searchTerm.Trim().ToLower();

            return query.Where(p => p.Name.ToLower().Contains(lowerCaseSearchTerm));
        }

        public static IQueryable<Product> Filter(this IQueryable<Product> query, string brands, string types)
        {
            var brandList = new List<string>();
            var typeList = new List<string>();
            if(!string.IsNullOrEmpty(brands)){
                brandList.AddRange(brands.ToLower().Split(",").ToList());
            }
            if(!string.IsNullOrEmpty(types)){
                typeList.AddRange(types.ToLower().Split(",").ToList());
            }
            query = query.Where(p => brandList.Count == 0 || brandList.Contains(p.Brand.ToLower()));
            query = query.Where(p => typeList.Count == 0 || typeList.Contains(p.Type.ToLower()));
            return query;
        }



    }
}

//Extensions lejojnë shtimin e metodav të reja në klasat ekzistuese pa ndryshuar kodin e burimit të tyre. 
//p.sh ne rastin tone ne ProductController te pjesa e kthimit te nje produkti ne variablin query nuk e kem nje mundesi qe te sortojm ne baz te 
// parametrit qe e pranojm psh me bo query.Sort() 
//e per ket arsyje na i perdorum extensions qe me mujt me bo metoda t reja available per perdorim ne ato metoda ekzistuse
