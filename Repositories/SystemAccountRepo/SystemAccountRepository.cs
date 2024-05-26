using BusinessObjects;
using DataAccessObjects;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SystemAccountRepo
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public SystemAccount? CheckLogin(string email, string password)
        {
            try
            {
                var account = SystemAccountDAO.getInstance().GetAccountByEmail(email);
                return account?.AccountPassword == password ? account : null;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<SystemAccount> GetAccounts()
        {
            try
            {
                var account = SystemAccountDAO.getInstance().GetAll();
                if(account == null) throw new Exception("Acounts is Empty");
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public SystemAccount? DeleteAccount(short accountId)
        {
            try
            {
                var account = SystemAccountDAO.getInstance().Delete(accountId);
                if (account == null) throw new Exception("Acounts is Empty");
                return account;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AddAccount(SystemAccount account)
        {
            try
            {
                SystemAccountDAO.getInstance().Add(account);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void UpdateAccount(SystemAccount account)
        {
            try
            {
                SystemAccountDAO.getInstance().Update(account);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public IEnumerable<SystemAccount> GetAccountsByEmail(string searchValue)
        {
            try
            {
                var result = SystemAccountDAO.getInstance().GetAccountsByEmail(searchValue);
                if (result == null) throw new Exception("Can not find account");
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
