using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    public class EmployeeManager
    {
        private readonly DatabaseManager manager = null;
        private readonly List<Employee> medarbetare = null;
        private List<string> ordlista = null;

        public EmployeeManager(CompanyDatabasContext _con)
        {
            manager = new DatabaseManager(_con);
            medarbetare = manager.GetAllEmployees();
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await manager.GetEmployees();
        }

        public List<Employee> GetAllEmployees()
        {
            return manager.GetAllEmployees();
        }

        public List<string> GetWordList()
        {
            if (medarbetare != null)
            {
                ordlista = new List<string>();
                foreach (Employee empl in medarbetare)
                {
                    ordlista.Add(empl.ID);
                    ordlista.Add(empl.First_name);
                    ordlista.Add(empl.Last_name);
                }
                ordlista.Sort();
                List<string> nyordlist = new List<string>(ordlista.Distinct());
                ordlista = nyordlist;
                return ordlista;
            }
            else
                return null;
        }

        /// <summary>
        /// Lägger in en ny avdelning, t ex export, import, accounting
        /// </summary>
        /// <param name="newdepart"></param>
        /// <returns></returns>
        public string NewDepartment(string newdepart)
        {
            if (string.IsNullOrEmpty(newdepart) || string.IsNullOrWhiteSpace(newdepart))
                return "FEL";
            else
            {
                int next = DepartmentHandler.GetSizeOfList();
                if (next >= 0)
                {
                    next++;
                    DepartmentHandler.AddDepartment(next, newdepart);
                    FileHandler.SaveDepartments();
                    return "OK";
                }
                else
                    return "FEL";
            }
        }

        public List<Employee> GetAutoComplete(string keywords)
        {
            List<Employee> sendlista = new List<Employee>();
            List<Employee> templist = new List<Employee>();
            templist = new List<Employee>(medarbetare.Where((Employee ku) =>
            ku.ID.Equals(keywords) ||
            ku.First_name.Equals(keywords) ||
            ku.Last_name.Equals(keywords)));
            if (templist.Count > 0)
            {
                foreach (Employee ny in templist)
                {
                    sendlista.Add(ny);
                }
            }
            if (sendlista.Count > 0)
            {
                /*  Går IGENOM SAMTLIGA RESULTAT och väljer DISTINKTA ID-NUMMER,
                *  så att SAMMA ID-NUMMER INTE FÖREKOMMER FLERA GÅNGER; */
                IEnumerable<Employee> filteredList = sendlista.GroupBy(ku => ku.ID).Select(group => group.First());
                /*  KONVERTERAR IEnumerable till LIST;   */
                List<Employee> nylista = new List<Employee>(filteredList);
                /* Sorterar efter NUMMERISKT LÖPNUMMER; */
                nylista.Sort(new SortEmpl());
                return nylista;
            }
            else
                return null;
        }

        public bool InsertNewEmployee(Employee medarb)
        {
            return manager.InsertNewEmployee(medarb);
        }

        public Employee GetEmployee(string empl_id)
        {
            return manager.GetEmployee(empl_id);
        }

        // Får in medarbetarens namn mm.
        public List<Employee> GetChosenEmpl(string[] keywords)
        {
            if (medarbetare != null)
            {
                List<Employee> sendlista = new List<Employee>();
                List<Employee> templist = new List<Employee>();

                foreach (string wor in keywords)
                {
                    // Jag använder metoden ToLower() för att mata in sökord
                    // på vilket sätt som helst: t ex Astrid eller astrid;
                    templist = new List<Employee>(medarbetare.Where((Employee ku) =>
                    ku.First_name.ToLower().Contains(wor.ToLower()) ||
                    ku.Last_name.ToLower().Contains(wor.ToLower()) ||
                    ku.Department.ToLower().Contains(wor.ToLower())
                    || ku.Email.ToLower().Contains(wor.ToLower())));

                    if (templist.Count > 0)
                    {
                        foreach (Employee ny in templist)
                        {
                            sendlista.Add(ny);
                        }
                    }
                }
                if (sendlista.Count > 0)
                {
                    /*  Går IGENOM SAMTLIGA RESULTAT och väljer DISTINKTA ID-NUMMER,
                    *  så att SAMMA ID-NUMMER INTE FÖREKOMMER FLERA GÅNGER; */

                    IEnumerable<Employee> filteredList = sendlista.GroupBy(ku => ku.ID).Select(group => group.First());
                    /*  KONVERTERAR IEnumerable till LIST;   */

                    List<Employee> nylista = new List<Employee>(filteredList);
                    nylista.Sort(new SortEmpl());
                    return nylista;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public bool SaveEmplChanges(Employee empl)
        {
            return manager.SaveEmplChanges(empl);
        }

        public bool DeleteEmployee(string emp_id)
        {
            return manager.DeleteEmployee(emp_id);
        }
    }
}
