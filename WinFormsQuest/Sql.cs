using System;

namespace WinFormsQuest
{
    public class Sql
    {
        internal const string grid = @"select distinct ProductName, 
	case when TypeProduct = 1 then 'Яблоко'
		 when TypeProduct = 2 then 'Груша'
		 end TypeProduct,
	NameSeller,
	sup.Price,
	CountProduct,
	Convert(char(16),sup.Delivery_date,120) as Delivery_date
from dbo.Supply sup
left join [dbo].[Articles] art ON sup.idProducts = art.id
left join [dbo].[Sellers] sell ON sell.NameSeller = sup.SellersName
where sup.id > 0 {0} {1} {2} {3}
order by 3, 1";
    }
}
