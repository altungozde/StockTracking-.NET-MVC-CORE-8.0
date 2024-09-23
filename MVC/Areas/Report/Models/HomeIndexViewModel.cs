#nullable disable

using Business.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MVC.Areas.Report.Models
{
    public class HomeIndexViewModel
    {
        public List<ReportItemModel> Report { get; set; }

        public FilterModel Filter { get; set; }

        public SelectList Categories { get; set; }
        public MultiSelectList Suppliers { get; set; }
    }
}
