using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace MyRESTService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ProductRESTService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ProductRESTService.svc or ProductRESTService.svc.cs at the Solution Explorer and start debugging.
    public class ProductRESTService : IProductRESTService
    {
        public List<Product> GetProductList(string category)
        {            
            return Products.Instance.ProductList(category);
        }

        public bool AddScores(string score, string name)
        {
             return Products.Instance.AddScores(score, name);
        }

        public bool CreateUser(string name, string password)
        {
            return Products.Instance.CreateUser(name, password);
        }

        public int CheckUser(string name, string password)
        {
            return Products.Instance.CheckUser(name, password);
        }

        public int GetScores(string name)
        {
            return Products.Instance.GetScores(name);
        }

        public List<IndividualScore> GetIndividualScores(string name)
        {
            return Products.Instance.GetIndividualScores(name);
        }
    }
}
