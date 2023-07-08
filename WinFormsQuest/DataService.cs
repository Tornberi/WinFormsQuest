using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsQuest
{
    public class DataService
    {
        public IEnumerable<DataRow> rowList(string znPriceFilter, string znProductFilter, string znSellerFilter, string znDateFilter)
        {
            var dto = SqlHelp.load_table(string.Format(Sql.grid, znPriceFilter, znProductFilter, znSellerFilter, znDateFilter));
            var result = dto.AsEnumerable();
            return result;
        }
    }
}
