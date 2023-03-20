using CommonServiceLocator;
using SolrNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchLibrary.Implementation
{
    public class Connection<T>
    {
        //public Connection(string module)
        //{
        //    string coreURL = System.Configuration.ConfigurationManager.AppSettings["SolrURL"];
        //    //InitializeConnection(coreURL + module);
        //}

        public void InitializeConnection(string coreURL)
        {
            Startup.Init<T>(coreURL);
        }

        internal ISolrOperations<T> GetSolrInstance()
        {
            ISolrOperations<T> solrInstance = ServiceLocator.Current.GetInstance<ISolrOperations<T>>();
            return solrInstance;
        }
    }
}
