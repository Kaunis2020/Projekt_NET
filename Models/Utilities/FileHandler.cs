using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    [NotMapped]
    public static class FileHandler
    {
        private const string fil = "App_Data/config/adminlog.conf";
        private static KeyValuePair<string, string> loginfo;
        private const string fil2 = "App_Data/binaries/departments.bin";
        /* Json-fil med alla anställda; */
        private const string fil3 = "App_Data/json/employees.json";
        /* Json-fil med alla kunder; */
        private const string fil4 = "App_Data/json/customers.json";

        static FileHandler()
        {
            OpenFile();
        }

        private static bool IsEmpty(string path)
        {
            return new FileInfo(path).Length == 0;
        }

        private static void OpenFile()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), fil);
            try // Try-catch om filen inte existerar;
            {
                using (StreamReader sreader = new StreamReader(path))
                {
                    if (File.Exists(path) && !IsEmpty(path))
                    {
                        string rad = sreader.ReadLine();
                        string[] data = rad.Split(';');
                        loginfo = new KeyValuePair<string, string>(data[0], data[1]);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                return;
            }
        }

        public static Dictionary<int, string> GetDepartments()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), fil2);
            return BinSerializerUtility.BinaryFileDeserialize<Dictionary<int, string>>(path);
        }

        public static void SaveDepartments()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), fil2);
            BinSerializerUtility.BinaryFileSerialize(DepartmentHandler.GetDepartments(), path);
        }

        public static KeyValuePair<string, string> GetLogInfo()
        {
            return loginfo;
        }

        public static bool SaveEmployees(List<Employee> emplolist)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), fil3);
            if (JSONSerializer.JSONFileSerialize(emplolist, path))
                return true;
            else
                return false;
        }

        public static bool SaveCustomers(List<Customer> customers)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), fil4);
            if (JSONSerializer.JSONFileSerialize(customers, path))
                return true;
            else
                return false;
        }

        public static byte[] GetFile(string filename)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), filename);
            byte[] data = File.ReadAllBytes(path);
            return data;
        }
    }
}
