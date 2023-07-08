using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsQuest
{
    public class NewRowsList : List<NewRows>
    {
        public static NewRowsList GetRows(IEnumerable<DataRow> listdto)
        {
            var result = new NewRowsList();
            result.Refresh(listdto);
            return result;
        }

        public void Refresh(IEnumerable<DataRow> listdto)
        {
            Clear();
            foreach (var dto in listdto)
            {
                Add(NewRows.GetRows(dto));
            }
        }
    }
}
