using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pb.Ninja.Service.AccountList
{
    public class AccountListService
    {
        /**
         * Function to get account list from local resource(Need to change into API service)
         */
        public async Task<List<Tuple<string, string>>> GetAccountListAsync()
        {
            List<Tuple<string, string>> result = new List<Tuple<string, string>>();

            try
            {
                string[] files = File.ReadAllLines(@"Data\accountlist.txt");
                foreach (var line in files)
                {
                    var values = line.Split(',');
                    result.Add(new Tuple<string, string>(values[0], values[1]));
                }
                return result;
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                return result;
            }
        }
    }
}
