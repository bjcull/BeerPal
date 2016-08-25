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
                // Create an Order object to store info about the shopping cart
                var order = new BeerPal.Entities.Order()
                {
                    OrderDate = DateTime.UtcNow,
                    Total = cart.CartItems.Sum(x => x.Price * x.Quantity),
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
                                total = (order.Total/100).ToString(), // PayPal expects string amounts, eg. "20.00"
                            },                            
                            item_list = new ItemList()
                            {                                
                                items =
                                    order.OrderItems.Select(x => new Item()
                                    {
                                        description = x.Name,
                                        currency = "USD",
                                        quantity = x.Quantity.ToString(),                                        
                                        price = (x.Price/100).ToString(), // PayPal expects string amounts, eg. "20.00"
                                    }).ToList()
                            }
                        }
                    },
                    redirect_urls = new RedirectUrls
                    {
                        return_url = Url.Action("Return", "ECommerce", new {orderId = order.Id}, Request.Url.Scheme),
                        cancel_url = Url.Action("Cancel", "ECommerce", new {orderId = order.Id}, Request.Url.Scheme)
                    }
                };

                // Send the payment to PayPal
                var createdPayment = payment.Create(apiContext);

                // Find the Approval URL to send our user to
                var approvalUrl =
                    createdPayment.links.FirstOrDefault(
                        x => x.rel.Equals("approval_url", StringComparison.OrdinalIgnoreCase));

                // Send the user to PayPal to approve the payment
                return Redirect(approvalUrl.href);
            }

            return RedirectToAction("Cart");
        }

        public ActionResult Return(string payerId, string paymentId, int orderId)
        {
            // Fetch the existing order
            var order = _dbContext.Orders.FirstOrDefault(x => x.Id == orderId);

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

            // Set a reference to the PayPal payment so we can look it up later
            order.PayPalReference = executedPayment.id;
            _dbContext.SaveChanges();

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