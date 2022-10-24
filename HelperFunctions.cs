using CartWebApp.Models;
using System.Text.Json;

namespace CartWebApp
{
    public class HelperFunctions
    {
        public double CalculateCouponCodeDiscount(string couponCode, List<productCartVM> cartProducts)
        {

            var data1 = File.ReadAllText("../CartWebApp/Data/CartRule.json");
            var CartRules = JsonSerializer.Deserialize<List<CartRule>>(data1);

            var data2 = File.ReadAllText("../CartWebApp/Data/CouponRule.json");
            var CouponRules = JsonSerializer.Deserialize<List<CouponRule>>(data2);

            var data3 = File.ReadAllText("../CartWebApp/Data/CouponCode.json");
            var OfferCouponCodes = JsonSerializer.Deserialize<List<OfferCouponCode>>(data3);

            double totalAmount = 0;

            foreach (productCartVM product in cartProducts)
            {
                totalAmount = totalAmount + (product.Price * product.Quntity);
            }

            double discountedAmount = 0;

            //optimise this
            var offerCode = OfferCouponCodes.Where(x => x.CouponCode == couponCode).FirstOrDefault();
           
            if (offerCode==null)
            {
               return  discountedAmount ;
            }
            
            var CouponDesc = CouponRules.Where(x => x.Id == offerCode.CouponRuleId).FirstOrDefault();
            var Rule = CartRules.Where(x => x.Id == CouponDesc.CartRuleId).FirstOrDefault();

            try
            {
                if (Rule != null)
                {
                    switch (Rule.RuleName.ToLower())
                    {

                        case "flat_off_percentage":
                            if (CouponDesc.IsFlatOffPercentage)
                            {
                                double discount = totalAmount / 10;
                                //discountedAmount = totalAmount - discount;
                                discountedAmount =   discount;
                            }
                            break;
                        case "buy_product_get_off_percentage":


                            if (CouponDesc.IsBuyProductFlatOffPercentage)
                            {

                                bool CartHasPrimeProduct = false;
                                bool CartHasDependentProduct = false;

                                double totalDependentProductAmount = 0;
                                double discount = 0;
                                int primeQty = 0;
                                int dependentQty = 0;

                                foreach (var pro in cartProducts)
                                {
                                    if (pro.ProductId == CouponDesc.PrimeProductId)
                                    {
                                        CartHasPrimeProduct = true;
                                        primeQty=pro.Quntity;
                                    }

                                    if (pro.ProductId == CouponDesc.DependentProductId)
                                    {
                                        CartHasDependentProduct = true;


                                        totalDependentProductAmount = totalDependentProductAmount + pro.Price;


                                        discount = totalDependentProductAmount / CouponDesc.BuyProductFlatOffDiscountPercentage;

                                        dependentQty = pro.Quntity;
                                    }

                                    //for all dependent items



                                }

                                //remove qty for single

                                if (CartHasPrimeProduct && CartHasDependentProduct)
                                {
                                    double qty = 0;//cartProducts.Where(x => x.ProductId == CouponDesc.DependentProductId).FirstOrDefault().Quntity;

                                    if (primeQty>dependentQty)
                                    {
                                        qty = dependentQty;
                                    }
                                    else
                                    {
                                        qty = primeQty;
                                    }


                                    discountedAmount = (discount * qty);
                                }


                            }
                            break;

                        case "off_on_nth_product":

                            if (CouponDesc.NthNumber > 0)
                            {

                                #region Calculation
                                var nthPros = cartProducts.Where(x => x.Quntity > CouponDesc.NthNumber).ToList();

                                double discount = 0;



                                foreach (productCartVM item in nthPros)
                                {

                                    if (item.ProductId == CouponDesc.PrimeProductId)
                                    {
                                        int countToDiscount = Convert.ToInt32((item.Quntity / CouponDesc.NthNumber));
                                        discount = discount+countToDiscount * item.Price;
                                    }

                                    

                                }

                                discountedAmount = discount;

                                #endregion


                            }
                            break;
                        default:
                            discountedAmount = 0;
                            break;
                    }
                }

            }
            catch (Exception ex)
            {

                discountedAmount = 0;
            }

            double val= totalAmount-discountedAmount;
            return val;
        }


        public List<productCartVM> GetProductsAndCart()
        {
            var Cartdata = File.ReadAllText("../CartWebApp/Data/MyCart.json");
            var myCart = JsonSerializer.Deserialize<List<MyCart>>(Cartdata);

            var Productdata = File.ReadAllText("../CartWebApp/Data/Product.json");
            var prod = JsonSerializer.Deserialize<List<Product>>(Productdata);

            var Cart = (from Mc in myCart
                        join p in prod
                        on Mc.ProductId equals p.Id
                        select new productCartVM
                        {
                            ProductName = p.ProductName,
                            Quntity = Mc.Quantity,
                            Price = p.Price,
                            ProductId = p.Id,
                            CategoryName = p.CategoryName
                        }).ToList();

            return Cart;
        }

    }

    public class productCartVM
    {
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int Quntity { get; set; }
        public int ProductId { get; set; }

        public double Price { get; set; }
    }
}
