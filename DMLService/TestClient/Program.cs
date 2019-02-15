using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.ServiceModel.Description;
using DMLService;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Uri baseAddress = new Uri("http://test-app-01:8080/dmlservice");
            ServiceHost WcfHost = new ServiceHost(typeof(DMLService.DMLService), baseAddress);
            //WcfHost.AddDefaultEndpoints();

            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            WcfHost.Description.Behaviors.Add(smb);


            WcfHost.Open();
            Console.WriteLine("The service is ready at {0}", baseAddress);
            Console.WriteLine("Press <Enter> to stop the service.");
            Console.ReadLine();
            WcfHost.Close();
        }
    }
}
