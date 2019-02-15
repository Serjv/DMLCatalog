using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.AccessControl;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace DMLService
{
    // ПРИМЕЧАНИЕ. Команду "Переименовать" в меню "Рефакторинг" можно использовать для одновременного изменения имени класса "Service1" в коде, SVC-файле и файле конфигурации.
    // ПРИМЕЧАНИЕ. Чтобы запустить клиент проверки WCF для тестирования службы, выберите элементы Service1.svc или Service1.svc.cs в обозревателе решений и начните отладку.
    public class DMLService : IDMLService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public string CreateFolder(string value)
        {
            try { 
            DirectoryInfo dirInfo = new DirectoryInfo(value);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string EditAccess(string value)
        {
            try
            {
                string DirectoryName = value;

                //Console.WriteLine("Adding access control entry for " + DirectoryName);

                //// Add the access control entry to the directory.
                //AddDirectorySecurity(DirectoryName, @"domain\databaseadmins", FileSystemRights.ReadData, AccessControlType.Allow);

                Console.WriteLine("Removing access control entry from " + DirectoryName);

                // Remove the access control entry from the directory.
                RemoveDirectorySecurity(DirectoryName, @"domain\DMLAdmins", FileSystemRights.FullControl, AccessControlType.Allow);

                Console.WriteLine("Done.");

                return "OK";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ex.Message;
            }
        }

        public void Error (string value)
        {
            Console.WriteLine(value);
        }

        public static void RemoveDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            // Create a new DirectoryInfo object.
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            // Get a DirectorySecurity object that represents the 
            // current security settings.
            DirectorySecurity dSecurity = dInfo.GetAccessControl();
            //удаление наследования
            dSecurity.SetAccessRuleProtection(true, true);


            // Set the new access settings.
            
            dInfo.SetAccessControl(dSecurity);
            //удаление пользователя из ACL
            dSecurity = dInfo.GetAccessControl();
            dSecurity.RemoveAccessRuleAll(new FileSystemAccessRule(Account, Rights, ControlType));
            Directory.SetAccessControl(FileName, dSecurity);


        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
    }
}
