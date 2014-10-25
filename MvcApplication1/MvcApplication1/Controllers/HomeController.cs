using MvcApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        public readonly static string WeekOverWeek = "http://ahcapi.azurewebsites.net/Api/OrderSummary/WeekOverWeek";
        public readonly static string MonthOverMonth = "http://ahcapi.azurewebsites.net/Api/OrderSummary/MonthOverMonth";
        public readonly static string YearOverYear = "http://ahcapi.azurewebsites.net/Api/OrderSummary/YearOverYear";

        //
        // GET: /Home/

        public ActionResult Index(int type = 0)
        {
            List<OrderSummary> orderSummaries = getOrderSummaries(type);
            return View(orderSummaries);
        }

        private List<OrderSummary> getOrderSummaries(int type)
        {
            string url = string.Empty;
            switch (type)
            {
                case 0:
                    url = WeekOverWeek;
                    break;
                case 1:
                    url = MonthOverMonth;
                    break;
                case 2:
                    url = YearOverYear;
                    break;
                default:
                    break;
            }
            HttpWebRequest wowRequest = (HttpWebRequest)WebRequest.Create(url);
            wowRequest.Method = "GET";
            wowRequest.ContentType = "text/xml; encoding='utf-8'";
            HttpWebResponse wowResponse = (HttpWebResponse)wowRequest.GetResponse();
            XmlDocument xmlDoc = new XmlDocument();
            XmlTextReader xmlReader = null;
            try
            {
                xmlReader = new XmlTextReader(wowResponse.GetResponseStream());
                xmlDoc.Load(xmlReader);
            }
            catch (Exception e)
            {

            }

            XmlNodeList orderSummaries = xmlDoc.GetElementsByTagName("OrderSummary");
            List<OrderSummary> orderSummaryList = new List<OrderSummary>();
            foreach (XmlNode orderSummaryNode in orderSummaries)
            {
                OrderSummary orderSummary = new OrderSummary();
                if (orderSummaryNode["Id"] != null)
                {
                    //var orderSummaryNodeTest = orderSummaryNode["Id"];
                    //var oderSummaryNodeTest2 = orderSummaryNode.ChildNodes.Item(0);
                    orderSummary.Id = int.Parse(orderSummaryNode["Id"].InnerXml);
                }
                if (orderSummaryNode["Ordered"] != null)
                    orderSummary.Ordered = int.Parse(orderSummaryNode["Ordered"].InnerXml);
                if (orderSummaryNode["PeriodDate"] != null)
                    orderSummary.PeriodDate = DateTime.Parse(orderSummaryNode["PeriodDate"].InnerXml);
                if (orderSummaryNode["PeriodLabel"] != null)
                    orderSummary.PeriodLabel = orderSummaryNode["PeriodLabel"].InnerXml;
                if (orderSummaryNode["Shipped"] != null)
                    orderSummary.Shipped = int.Parse(orderSummaryNode["Shipped"].InnerXml);
                if (orderSummaryNode["TotalOrderedAmount"] != null)
                    orderSummary.TotalOrderedAmount = int.Parse(orderSummaryNode["TotalOrderedAmount"].InnerXml);
                if (orderSummaryNode["TotalShippedAmount"] != null)
                    orderSummary.TotalShippedAmount = int.Parse(orderSummaryNode["TotalShippedAmount"].InnerXml);
                orderSummaryList.Add(orderSummary);
            }

            return orderSummaryList.OrderBy(x => x.PeriodDate).ToList();
        }

        public JsonResult FillIndex(int type = 0)
        {
            return Json(getOrderSummaries(type), JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}

