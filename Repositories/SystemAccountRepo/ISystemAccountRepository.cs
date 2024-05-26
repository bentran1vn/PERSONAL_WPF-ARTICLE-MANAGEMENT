
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SystemAccountRepo
{
    public interface ISystemAccountRepository
    {
        SystemAccount? CheckLogin(string username, string password);

        IEnumerable<SystemAccount> GetAccounts();

        SystemAccount? DeleteAccount(short accountId);

        void AddAccount(SystemAccount account);

        void UpdateAccount(SystemAccount account);

        IEnumerable<SystemAccount> GetAccountsByEmail(string searchValue);
    }
}
