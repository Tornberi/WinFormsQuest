using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormsQuest
{
    public class Rows
    {
        public int TypeProduct { get; set; }
        public string ProductName { get; set; }
        public string NameSeller { get; set; }
        public decimal? Price { get; set; }
        public int CountProduct { get; set; }
        public DateTime? Delivery_date { get; set; }

        public void Refresh(DataRow row)
        {
            this.TypeProduct = row.Field<int>("TypeProduct");
            this.ProductName = row.Field<string>("ProductName");
            this.NameSeller = row.Field<string>("NameSeller");
            this.Price = row.Field<decimal>("Price");
            this.CountProduct = row.Field<int>("CountProduct");
            this.Delivery_date = row.Field<DateTime>("Delivery_date");
        }

        public static Rows GetRows(DataRow dto)
        {
            var result = new Rows();
            result.Refresh(dto);
            return result;
        }
    }
}
