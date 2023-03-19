using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekt.Models
{
    public class DatabaseManager
    {
        private readonly CompanyDatabasContext _context = null;

        public DatabaseManager(CompanyDatabasContext _con)
        {
            _context = _con;
        }

        public async Task<List<Media>> GetAllMedia()
        {
            var medialist = new List<Media>();
            List<Book> booklist = await _context.Books.ToListAsync();
            List<Cd> cdlist = await _context.Cds.ToListAsync();
            List<Dvd> dvdlist = await _context.Dvds.ToListAsync();
            medialist.AddRange(booklist);
            medialist.AddRange(cdlist);
            medialist.AddRange(dvdlist);
            return medialist;
        }

        public List<Cd> GetCds()
        {
            return _context.Cds.ToList();
        }

        public List<Book> GetBooks()
        {
            return _context.Books.ToList();
        }

        public List<Dvd> GetDvds()
        {
            return _context.Dvds.ToList();
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task<List<Cd>> GetAllCDs()
        {
            return await _context.Cds.ToListAsync();
        }

        public async Task<List<Dvd>> GetAllDVDs()
        {
            return await _context.Dvds.ToListAsync();
        }

        public async Task<List<Employee>> GetEmployees()
        {
            return await _context.Employees.ToListAsync();
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await _context.Customers.ToListAsync();
        }

        public List<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public List<Employee> GetAllEmployees()
        {
            return _context.Employees.ToList();
        }

        public bool InsertNewCustomer(Customer nycus)
        {
            if (nycus != null)
            {
                try
                {
                    _context.Customers.Add(nycus);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool InsertMessage(Message mess)
        {
            if (mess != null)
            {
                try
                {
                    _context.Messages.Add(mess);
                    _context.SaveChanges();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
                return false;
        }

        public async Task<List<Message>> GetAllMessages()
        {
            return await _context.Messages.ToListAsync();
        }

        public Message GetMessage(int _id)
        {
            return _context.Messages.FirstOrDefault(ku => ku.Number.Equals(_id));
        }

        public bool InsertNewEmployee(Employee nyemp)
        {
            if (nyemp != null)
            {
                try
                {
                    _context.Employees.Add(nyemp);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public Employee GetEmployee(string empl_id)
        {
            return _context.Employees.FirstOrDefault(ku => ku.ID.Equals(empl_id));
        }

        public bool DeleteMessage(int _id)
        {
            Message mess = _context.Messages.FirstOrDefault(ku => ku.Number.Equals(_id));
            if (mess != null)
            {
                try
                {
                    _context.Messages.Remove(mess);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool SaveEmplChanges(Employee empl)
        {
            try
            {
                Employee employ = _context.Employees.FirstOrDefault(cu => cu.ID.Equals(empl.ID));
                if (employ != null)
                {
                    employ.Department = empl.Department;
                    employ.Email = empl.Email;
                    employ.Phone = empl.Phone;
                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteEmployee(string emp_id)
        {
            try
            {
                Employee empl = _context.Employees.FirstOrDefault(ku => ku.ID.Equals(emp_id));
                if (empl != null)
                {
                    _context.Employees.Remove(empl);
                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ChangeAddress(string kund_id, string gata, string stad, string land)
        {
            try
            {
                Customer cust = _context.Customers.FirstOrDefault(cu => cu.Kund_ID.Equals(kund_id));
                if (cust != null)
                {
                    cust.GatuAdress = gata;
                    cust.Ort = stad;
                    cust.Land = land;
                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool UpdatePrice(string prod_id, int nyttpris)
        {
            try
            {
                if (prod_id.StartsWith("BO"))
                {
                    Book temp = _context.Books.FirstOrDefault(bok => bok.Bok_ID.Equals(prod_id));
                    if (temp != null)
                    {
                        temp.Pris = nyttpris;
                        _context.SaveChanges();
                        return true;
                    }
                }
                else if (prod_id.StartsWith("CD"))
                {
                    Cd temp = _context.Cds.FirstOrDefault(_cd => _cd.CD_ID.Equals(prod_id));
                    if (temp != null)
                    {
                        temp.Pris = nyttpris;
                        _context.SaveChanges();
                        return true;
                    }
                }
                else if (prod_id.StartsWith("DVD"))
                {
                    Dvd temp = _context.Dvds.FirstOrDefault(_dv => _dv.DVD_ID.Equals(prod_id));
                    if (temp != null)
                    {
                        temp.Pris = nyttpris;
                        _context.SaveChanges();
                        return true;
                    }
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        public bool RegisterBook(Book nyb)
        {
            if (nyb != null)
            {
                try
                {
                    _context.Books.Add(nyb);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool RegisterCD(Cd nycd)
        {
            if (nycd != null)
            {
                try
                {
                    _context.Cds.Add(nycd);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool RegisterDVD(Dvd nydv)
        {
            if (nydv != null)
            {
                try
                {
                    _context.Dvds.Add(nydv);
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
                return false;
        }

        public bool DeleteCustomer(string kundid)
        {
            try
            {
                Customer cust = _context.Customers.FirstOrDefault(ku => ku.Kund_ID.Equals(kundid));
                if (cust != null)
                {
                    _context.Customers.Remove(cust);
                    _context.SaveChanges();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteProduct(string prodid)
        {
            if (!string.IsNullOrEmpty(prodid))
            {
                try
                {
                    if (prodid.StartsWith("BO"))
                    {
                        Book temp = _context.Books.FirstOrDefault(bok => bok.Bok_ID.Equals(prodid));
                        if (temp != null)
                        {
                            _context.Books.Remove(temp);
                            _context.SaveChanges();
                            return true;
                        }
                    }
                    else if (prodid.StartsWith("CD"))
                    {
                        Cd temp = _context.Cds.FirstOrDefault(_cd => _cd.CD_ID.Equals(prodid));
                        if (temp != null)
                        {
                            _context.Cds.Remove(temp);
                            _context.SaveChanges();
                            return true;
                        }
                    }
                    else if (prodid.StartsWith("DVD"))
                    {
                        Dvd temp = _context.Dvds.FirstOrDefault(_dv => _dv.DVD_ID.Equals(prodid));
                        if (temp != null)
                        {
                            _context.Dvds.Remove(temp);
                            _context.SaveChanges();
                            return true;
                        }
                    }
                    else
                        return false;
                }
                catch (Exception)
                {
                    return false;
                }
                return false;
            }
            else
                return false;
        }
    }
}
