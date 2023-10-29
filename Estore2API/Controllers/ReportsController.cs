using Estore2API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace Estore2API.Controllers
{
    public class ReportsController : ODataController
    {
        private readonly eStoreContext _context;

        public ReportsController(eStoreContext context)
        {
            _context = context;
        }
        [EnableQuery]
        public ActionResult<IEnumerable<Report>> GetReports()
        {
            List<Report> reportStatistics = new List<Report>();
            var O = _context.Orders.ToList();
            var OD = _context.OrderDetails.ToList();
            var ODO = from od in OD
                      join o in O
                      on od.OrderId equals o.OrderId
                      select new
                      {
                          OrderDate = o.OrderDate,
                          Total = od.Quantity * od.UnitPrice * (decimal)(100 - od.Discount)/100
                      };
            var result = ODO.GroupBy(o => o.OrderDate)
                .Select(odo => new
                {
                    Key = odo.Key,
                    SumTotal = odo.Sum(x => x.Total)
                });
            foreach (var odo in result)
            {
                Report a = new Report();
                a.OrderDate = odo.Key;
                a.Total = odo.SumTotal;
                reportStatistics.Add(a);
            }
            return reportStatistics;
        }
    }
}
