using System.ComponentModel.DataAnnotations.Schema;

namespace CartWebApp.Models
{
    public class CouponRule
    {
        public int Id { get; set; }
        public int CartRuleId { get; set; }
        [ForeignKey("CartRuleId")]
        public CartRule CartRules { get; set; }

        //offers according to different rules
        #region Flat off 
        public bool IsFlatOffPercentage { get; set; }
        public int? FlatOffDiscountPercentage { get; set; }
        #endregion



        #region  Off Percentage 
        public bool IsBuyProductFlatOffPercentage { get; set; }
        public int BuyProductFlatOffDiscountPercentage { get; set; }

        #endregion

        public int PrimeProductId { get; set; }
        [ForeignKey("productId")]
        public Product Product { get; set; }

        public int DependentProductId { get; set; }
        [ForeignKey("dProductId")]
        public Product Product1 { get; set; }

        public int NthNumber { get; set; }






    }
}
