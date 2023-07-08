using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsQuest
{
    public class NewRows
    {
        public string ProductName { get; set; }
        public string TypeProduct { get; set; }
        public string NameSeller { get; set; }
        public decimal Price { get; set; }
        public int CountProduct { get; set; }
        public string Delivery_date { get; set; }

        public void Refresh(DataRow row)
        {
            this.ProductName = row.Field<string>("ProductName");
            this.TypeProduct = row.Field<string>("TypeProduct");
            this.NameSeller = row.Field<string>("NameSeller");
            this.Price = row.Field<decimal>("Price");
            this.CountProduct = row.Field<int>("CountProduct");
            this.Delivery_date = row.Field<string>("Delivery_date");
        }

        public static NewRows GetRows(DataRow dto)
        {
            var result = new NewRows();
            result.Refresh(dto);
            return result;
        }
    }
}
