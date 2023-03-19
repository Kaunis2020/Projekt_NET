using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    public class CustomerManager
    {
        private readonly DatabaseManager manager = null;
        private List<Customer> kundlista = null;
        private List<string> ordlista = null;

        public CustomerManager(CompanyDatabasContext _con)
        {
            manager = new DatabaseManager(_con);
        }

        public void FillList()
        {
            kundlista = manager.GetAllCustomers();
            if (kundlista != null)
            {
                ordlista = new List<string>();
                foreach (Customer ku in kundlista)
                {
                    ordlista.Add(ku.Kund_ID);
                    if (!string.IsNullOrEmpty(ku.FirmaNamn)
                        && !string.IsNullOrWhiteSpace(ku.FirmaNamn))
                    {
                        ordlista.Add(ku.FirmaNamn);
                    }
                    ordlista.Add(ku.KontaktPerson);
                }
                ordlista.Sort();
                List<string> nyordlist = new List<string>(ordlista.Distinct());
                ordlista = nyordlist;
            }
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await manager.GetCustomers();
        }

        public List<Customer> GetAllCustomers()
        {
            return manager.GetAllCustomers();
        }

        public List<string> GetWordList()
        {
            return ordlista;
        }

        public bool InsertNewCustomer(Customer nycus)
        {
            return manager.InsertNewCustomer(nycus);
        }

        public List<Customer> GetAutoComplete(string keywords)
        {
            List<Customer> sendlista = new List<Customer>();
            List<Customer> templist = new List<Customer>();
            templist = new List<Customer>(kundlista.Where((Customer ku) =>
            ku.FirmaNamn.Equals(keywords) ||
            ku.Kund_ID.Equals(keywords) ||
            ku.KontaktPerson.Equals(keywords)));

            if (templist.Count > 0)
            {
                foreach (Customer ny in templist)
                {
                    sendlista.Add(ny);
                }
            }

            if (sendlista.Count > 0)
            {
                /*  Går IGENOM SAMTLIGA RESULTAT och väljer DISTINKTA ID-NUMMER,
                *  så att SAMMA ID-NUMMER INTE FÖREKOMMER FLERA GÅNGER; */

                IEnumerable<Customer> filteredList = sendlista.GroupBy(ku => ku.Kund_ID).Select(group => group.First());
                /*  KONVERTERAR IEnumerable till LIST;   */

                List<Customer> nylista = new List<Customer>(filteredList);

                /* Sorterar efter NUMMERISKT LÖPNUMMER; */
                nylista.Sort(new SortCustomer());
                return nylista;
            }
            else
                return null;
        }

        // Får in kundens namn, kontaktpersonens namn
        // eller företagets namn som sökkriterier;
        internal List<Customer> GetChosenCustomer(string[] keywords)
        {
            if (kundlista != null)
            {
                List<Customer> sendlista = new List<Customer>();
                List<Customer> templist = new List<Customer>();

                foreach (string wor in keywords)
                {
                    // Jag använder metoden ToLower() för att mata in sökord
                    // på vilket sätt som helst: t ex Astrid eller astrid;
                    templist = new List<Customer>(kundlista.Where((Customer ku) =>
                    ku.FirmaNamn.ToLower().Contains(wor.ToLower()) ||
                    ku.KontaktPerson.ToLower().Contains(wor.ToLower()) ||
                    ku.GatuAdress.ToLower().Contains(wor.ToLower())
                    || ku.Ort.ToLower().Contains(wor.ToLower())
                    || ku.Land.ToLower().Contains(wor.ToLower())));

                    if (templist.Count > 0)
                    {
                        foreach (Customer ny in templist)
                        {
                            sendlista.Add(ny);
                        }
                    }
                }

                if (sendlista.Count > 0)
                {
                    /*  Går IGENOM SAMTLIGA RESULTAT och väljer DISTINKTA ID-NUMMER,
                    *  så att SAMMA ID-NUMMER INTE FÖREKOMMER FLERA GÅNGER; */

                    IEnumerable<Customer> filteredList = sendlista.GroupBy(ku => ku.Kund_ID).Select(group => group.First());
                    /*  KONVERTERAR IEnumerable till LIST;   */

                    List<Customer> nylista = new List<Customer>(filteredList);
                    nylista.Sort(new SortCustomer());
                    return nylista;
                }
                else
                    return null;
            }
            else
                return null;
        }

        public bool ChangeAddress(string kund_id, string gata, string stad, string land)
        {
            return manager.ChangeAddress(kund_id, gata, stad, land);
        }

        public async Task<List<Message>> AllMessages()
        {
            return await manager.GetAllMessages();
        }

        public bool InsertMessage(Message medd)
        {
            return manager.InsertMessage(medd);
        }

        public bool DeleteMessage(int _id)
        {
            return manager.DeleteMessage(_id);
        }

        public bool DeleteCustomer(string kundid)
        {
            return manager.DeleteCustomer(kundid);
        }
    }
}
