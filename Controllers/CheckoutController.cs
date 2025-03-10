﻿using E_CommerceMVC.Configuration;
using E_CommerceMVC.Data;
using E_CommerceMVC.Models;
using E_CommerceMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Razorpay.Api;

namespace E_CommerceMVC.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        public readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRazorPayConfiguration _razorPay;

        public CheckoutController(ApplicationDbContext context, IRazorPayConfiguration razorPay, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            _razorPay = razorPay;
        }





        public async Task<IActionResult> Index()
        {
            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            var addresses = await _context.Addresses
                .Include(x => x.User)
                .Where(x => x.UserId == currentuser.Id)
                .ToListAsync();

            ViewBag.Addresses = addresses;

            return View();
        }


        public async Task<IActionResult> PaymentOptions(int addressId)
        {
            var address = await _context.Addresses.Where(x => x.Id == addressId).FirstOrDefaultAsync();
            if (address == null)
            {
                return BadRequest();
            }

            var currentuser = await _userManager.GetUserAsync(HttpContext.User);

            double orderCost = 0;

            var carts = await _context.Carts
                .Include(x => x.Product)
                .Where(x => x.UserId == currentuser.Id).ToListAsync();

            foreach (var cart in carts)
            {
                orderCost += (cart.Product.Price * cart.Qty);
            }



            var transactionId = Guid.NewGuid().ToString();
            RazorpayClient client = new RazorpayClient(_razorPay.KeyID, _razorPay.KeySecret);

            Dictionary<string, object> options = new Dictionary<string, object>();

            options.Add("amount", orderCost * 100);
            options.Add("receipt", transactionId);
            options.Add("currency", "INR");
            options.Add("payment_capture", "0");


            //Razorpay.Api.Order orderResponse = client.Order.Create(options);
            //string orderId = orderResponse["id"].ToString();

            var paymentOptions = new PaymentOptions
            {
                addressId = addressId,
                //  orderId = orderId,
                razorpayKey = _razorPay.KeyID,
                amount = orderCost * 100,
                currency = "INR",
                name = currentuser.FullName,
                email = currentuser.Email,
                contactNumber = currentuser.PhoneNumber,
            };



            ViewBag.items = orderCost;
            ViewBag.tax = 0;
            ViewBag.delivery = 0;
            ViewBag.subtotal = orderCost;
            ViewBag.ordertotal = orderCost;




            return View(paymentOptions);
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Complete(string rzp_paymentid, string rzp_orderid, int addressId)
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);

            if (user != null)
            {
                var carts = await _context.Carts
                    .Include(x => x.Product)
                    .Where(x => x.UserId == user.Id).ToListAsync();


                // This id is razorpay unique payment id which can be use to get the payment details from razorpay server
                string paymentId = rzp_paymentid;

                // This is orderId
                string orderId = rzp_orderid;

                Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(_razorPay.KeyID, _razorPay.KeySecret);

                Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);

                // This code is for capture the payment 
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", payment.Attributes["amount"]);
                Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
                string amt = paymentCaptured.Attributes["amount"];
                var amount = double.Parse(amt);

                var order = new Models.Order
                {
                    AddressId = addressId,
                    CreatedAt = DateTime.Now,
                    Status = "Order Placed",
                    UserId = user.Id,
                    Amount = amount / 100,
                };

                _context.Orders.Add(order);

                await _context.SaveChangesAsync();

                foreach (var cart in carts)
                {
                    var orderProduct = new OrderProduct
                    {
                        ProductId = cart.ProductId,
                        OrderId = order.Id,
                        Price = cart.Product.Price,
                        Qty = cart.Qty,
                    };
                    _context.Add(orderProduct);
                }

                await _context.SaveChangesAsync();

                _context.RemoveRange(carts);
                await _context.SaveChangesAsync();
            }




            return RedirectToAction("ThankYou");

        }

        public IActionResult ThankYou()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Index(Address address)
        {
            if (ModelState.IsValid)
            {
                var currentuser = await _userManager.GetUserAsync(HttpContext.User);

                address.UserId = currentuser.Id;
                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return View(address);
        }

    }
}
