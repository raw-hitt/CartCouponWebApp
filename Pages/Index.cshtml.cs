using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CartWebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public string coupon { get; set; }

        public IndexModel()
        {

        }

        public void OnGet()
        {
            HelperFunctions hf = new HelperFunctions();
            ViewData["CartProducts"] = hf.GetProductsAndCart();
        }

        public void OnPost()
        {


            HelperFunctions hf = new HelperFunctions();
            List<productCartVM> prods = hf.GetProductsAndCart();
            double dx = hf.CalculateCouponCodeDiscount(this.coupon, prods);

            ViewData["CartProducts"] = hf.GetProductsAndCart();
            ViewData["DiscountedPrice"]=dx.ToString();
        }
    }
}