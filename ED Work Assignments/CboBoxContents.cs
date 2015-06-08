using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ED_Work_Assignments
{
    public class Users : ObservableCollection<string>
    {
        String[] contents = {"Eli Price"};
        Dictionary<String, String> names = new Dictionary<String, String>();

        public Users()
        {
            names.Add("eliprice", "Eli Price");
            names.Add("miaria","Mike Iaria");

            foreach (KeyValuePair<String, String> user in names)
            {
                Add(user.Value);
            }
        }
        public String getName(string strUserName)
        {
            return names[strUserName];
        }
    }
}
