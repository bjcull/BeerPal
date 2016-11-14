using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeerPal.Entities;
using BeerPal.Models;
using BeerPal.Models.ECommerce;
using Microsoft.AspNet.Identity.Owin;
using PayPal.Api;

namespace BeerPal.Controllers
{
    public class ECommerceController : Controller
    {
        private ApplicationDbContext _dbContext => HttpContext.GetOwinContext().Get<ApplicationDbContext>();

        public ActionResult Index()
        {
            var model = new IndexVm()
            {
                Beers = _dbContext.Beers.ToList()
            };

            return View(model);
        }

        public ActionResult Cart()
        {
            var cart = CreateOrGetCart();

            return View(cart);
        }

        public ActionResult Add(int beerId)
        {
            var beer = _dbContext.Beers.FirstOrDefault(x => x.Id == beerId);

            var cart = CreateOrGetCart();
            var existingItem = cart.CartItems.FirstOrDefault(x => x.BeerId == beer.Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.CartItems.Add(new CartItem()
                {
                    BeerId = beer.Id,
                    Name = beer.Name,
                    Price = beer.Price,
                    Quantity = 1
                });
            }

            SaveCart(cart);

            return RedirectToAction("Cart", "ECommerce");
        }

        public ActionResult Delete(int beerId)
        {
            var beer = _dbContext.Beers.FirstOrDefault(x => x.Id == beerId);

            var cart = CreateOrGetCart();
            var existingItem = cart.CartItems.FirstOrDefault(x => x.BeerId == beer.Id);

            if (existingItem != null)
            {
                cart.CartItems.Remove(existingItem);
            }

            SaveCart(cart);

            return RedirectToAction("Cart", "ECommerce");
        }

        public ActionResult Checkout()
        {
            var cart = CreateOrGetCart();

            if (cart.CartItems.Any())
            {
                // Flat rate shipping
                int shipping = 500;

                // Flat rate tax 10%
                var taxRate = 0.1M;

                var subtotal = cart.CartItems.Sum(x => x.Price*x.Quantity);
                var tax = Convert.ToInt32((subtotal + shipping)*taxRate);
                var total = subtotal + shipping + tax;

                // Create an Order object to store info about the shopping cart
                var order = new BeerPal.Entities.Order()
                {
                    OrderDate = DateTime.UtcNow,
                    Subtotal = subtotal,
                    Shipping = shipping,
                    Tax = tax,
                    Total = total,
                    OrderItems = cart.CartItems.Select(x => new OrderItem()
                    {
                        Name = x.Name,
                        Price = x.Price,
                        Quantity = x.Quantity
                    }).ToList()
                };
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();

                // Get PayPal API Context using configuration from web.config
                var apiContext = GetApiContext();

                // Create a new payment object
                var payment = new Payment
                {
                    intent = "sale",
                    payer = new Payer
                    {
                        payment_method = "paypal"
                    },
                    transactions = new List<Transaction>
                    {
                        new Transaction
                        {
                            description = $"BeerPal Brewery Shopping Cart Purchase",
                            amount = new Amount
                            {
                                currency = "USD",
                                total = (order.Total/100M).ToString(), // PayPal expects string amounts, eg. "20.00",
                                details = new Details()
                                {
                                    subtotal = (order.Subtotal/100M).ToString(),
                                    shipping = (order.Shipping/100M).ToString(),
                                    tax = (order.Tax/100M).ToString()                                    
                                }
                            },                            
                            item_list = new ItemList()
                            {                                
                                items =
                                    order.OrderItems.Select(x => new Item()
                                    {
                                        description = x.Name,
                                        currency = "USD",
                                        quantity = x.Quantity.ToString(),                                        
                                        price = (x.Price/100M).ToString(), // PayPal expects string amounts, eg. "20.00"
                                    }).ToList()
                            }
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = Url.Action("Return", "ECommerce", null, Request.Url.Scheme),
                        cancel_url = Url.Action("Cancel", "ECommerce", null, Request.Url.Scheme)
                    }
                };

                // Send the payment to PayPal
                var createdPayment = payment.Create(apiContext);

                // Save a reference to the paypal payment
                order.PayPalReference = createdPayment.id;
                _dbContext.SaveChanges();

                // Find the Approval URL to send our user to
                var approvalUrl =
                    createdPayment.links.FirstOrDefault(
                        x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));

                // Send the user to PayPal to approve the payment
                return Redirect(approvalUrl.href);
            }

            return RedirectToAction("Cart");
        }

        public ActionResult Return(string payerId, string paymentId)
        {
            // Fetch the existing order
            var order = _dbContext.Orders.FirstOrDefault(x => x.PayPalReference == paymentId);

            // Get PayPal API Context using configuration from web.config
            var apiContext = GetApiContext();

            // Set the payer for the payment
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };

            // Identify the payment to execute
            var payment = new Payment()
            {
                id = paymentId
            };

            // Execute the Payment
            var executedPayment = payment.Execute(apiContext, paymentExecution);

            ClearCart();

            return RedirectToAction("Thankyou");
        }

        public ActionResult Cancel()
        {
            return View();
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        private Cart CreateOrGetCart()
        {
            var cart = Session["Cart"] as Cart;
            if (cart == null)
            {
                cart = new Cart();
                SaveCart(cart);
            }

            return cart;
        }

        private void ClearCart()
        {
            var cart = new Cart();
            SaveCart(cart);
        }

        private void SaveCart(Cart cart)
        {
            Session["Cart"] = cart;
        }

        private APIContext GetApiContext()
        {
            // Authenticate with PayPal
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            return apiContext;
        }
    }
}