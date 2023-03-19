using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    /// <summary>
    /// Företagets avdelningar, 10 olika avdelningar:
    /// Bl.a. Accountig, Import, Export, Sales och Support.
    /// </summary>
    public static class DepartmentHandler
    {
        private static Dictionary<int, string> departments = null;

        static DepartmentHandler()
        {
            departments = FileHandler.GetDepartments();
        }

        /// <summary>
        /// Returnerar företagets ALLA avdelningar
        /// </summary>
        /// <returns>Dictionary</returns>
        public static Dictionary<int, string> GetDepartments()
        {
            return departments;
        }

        /// <summary>
        /// Returnerar sorterad lista med företagets avdelningar
        /// I Alfabetisk ordning: Accounting, Cleaning, Design osv.
        /// </summary>
        /// <returns>Dictionary</returns>
        public static Dictionary<int, string> GetSortedDepartments()
        {
            var ordered = departments.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            return ordered;
        }

        /// <summary>
        /// Lägger in avdelning;
        /// </summary>
        /// <param name="num">Nummer</param>
        /// <param name="name">Namn</param>
        public static void AddDepartment(int num, string name)
        {
            departments.Add(num, name);
        }

        /// <summary>
        /// Returnerar listans längd
        /// </summary>
        /// <returns>Antal avdelningar</returns>
        public static int GetSizeOfList()
        {
            if (departments == null)
                return -2;
            else
                return departments.Count;
        }
    }
}
