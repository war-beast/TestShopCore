using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestShopCore.Areas.Admin.Utils
{
    public class OrderState : List<String>
    {
        private static OrderState StateList;
        private static object syncRoot = new Object();

        private OrderState()
        {
            
        }

        public static OrderState GelList()
        {
            if (StateList == null)
            {
                lock (syncRoot)
                {
                    if (StateList == null)
                    {
                        StateList = new OrderState();
                        StateList.Add("В обработке");
                        StateList.Add("Выставлен счёт");
                        StateList.Add("Счёт оплачен");
                        StateList.Add("Товар отправлен покупателю");
                        StateList.Add("Товар доставлен по адресу");
                    }
                }
            }
            return StateList;
        }
    }
}