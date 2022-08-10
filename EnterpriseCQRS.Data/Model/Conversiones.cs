using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseCQRS.Data.Model
{
    public class Conversiones
    {
        public string From { get; set; }
        public string To { get; set; }

        //Rate es el valor unitario
        public string Rate { get; set; }

        //Currency si es diferente de EUR se hace el cambio de moneda
        public string Currency { get; set; }

        //Amount corresponde al valor total que vamos a cambiar de moneda segun lo que traiga Currency
        public string Amount { get; set; }

        //Representa la conversion final a EUR
        public string CurrencyFinal { get; set; }

    }
}
