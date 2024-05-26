using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessObjects
{
    public class SystemAccountDAO
    {
        private static SystemAccountDAO? instance;

        private static readonly object lockObj = new object();

        private SystemAccountDAO() { }
        public static SystemAccountDAO getInstance() 
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                    {
                        instance = new SystemAccountDAO();
                    }
                }
            }
            return instance;
        }

        public SystemAccount? GetAccountByEmail(string email)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var account = context.SystemAccounts.SingleOrDefault(x => x.AccountEmail == email);
                return account;
            }
            catch (Exception)
            {
                throw new Exception("User not exist !");
            }
        }

        public void Add(SystemAccount account)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var result = context.SystemAccounts.Add(account);
                context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Something Wrongs when Adding User !");
            }
        }

        public void Update(SystemAccount account)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var result = context.SystemAccounts.Update(account);
                context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Something Wrongs when Adding User !");
            }
        }

        public IEnumerable<SystemAccount> GetAll()
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var accounts = context.SystemAccounts.Where(x => true).Select(x => new SystemAccount
                {
                    AccountId = x.AccountId,
                    AccountEmail = x.AccountEmail,
                    AccountName = x.AccountName,
                    AccountRole = x.AccountRole,
                }).ToList();
                return accounts;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public SystemAccount? Delete(short accountId)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var account = context.SystemAccounts.Include(x => x.NewsArticles).SingleOrDefault(a => a.AccountId == accountId);
                if (account != null)
                {
                    foreach (var item in account.NewsArticles)
                    {
                        item.CreatedById = null;
                    }
                    context.SystemAccounts.Remove(account);
                    context.SaveChanges();
                    return account;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<SystemAccount> GetAccountsByEmail(string value)
        {
            using var context = new FunewsManagementDbContext();
            try
            {
                var account = context.SystemAccounts.Where(x => x.AccountEmail.ToLower().Contains(value)).ToList();
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
