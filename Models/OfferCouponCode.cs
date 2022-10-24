using System.ComponentModel.DataAnnotations.Schema;

namespace CartWebApp.Models
{
    public class OfferCouponCode 
    {
        public int Id { get; set; }
        public int CouponRuleId { get; set; }
        [ForeignKey("CouponRuleId")]
        public CouponRule CouponRule { get; set; }

        public string CouponCode { get; set; }
      

    }
}
