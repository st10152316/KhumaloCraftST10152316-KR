using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;
using System.Net.Mail;
using System.Net;
using MimeKit;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace KhumaloCraftST10152316.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepo;
        private CartRepository _cart;

        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }
        public async Task<IActionResult> AddItem(int ProductId, int qty = 1, int redirect = 0)
        {
            var cartCount = await _cartRepo.AddItem(ProductId, qty);
            if (redirect == 0)
                return Ok(cartCount);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> RemoveItem(int ProductId)
        {
            var cartCount = await _cartRepo.RemoveItem(ProductId);
            return RedirectToAction("GetUserCart");
        }
        public async Task<IActionResult> GetUserCart()
        {
            var cart = await _cartRepo.GetUserCart();
            return View(cart);
        }

        public  async Task<IActionResult> GetTotalItemInCart()
        {
            int cartItem = await _cartRepo.GetCartItemCount();
            return Ok(cartItem);
        }

        public  IActionResult Checkout()
        {
            
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CheckoutModel model)
        {
            SmtpClient client = new SmtpClient("smtp.office365.com", 587);
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("khumalocraftst10152316@outlook.com", "Kavzvr@1");
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Timeout = 100000;
            MailMessage msg = new MailMessage();
            msg.To.Add(model.Email);
            msg.From = new MailAddress("khumalocraftst10152316@outlook.com");

            if (!ModelState.IsValid)
                return View(model);
            bool isCheckedOut = await _cartRepo.DoCheckout(model);
            if (!isCheckedOut)
            {
                msg.Subject = "Order has Failed";
                msg.Body = "We apologise for the inconvenience";
                client.Send(msg);
                return RedirectToAction(nameof(OrderFailure));
            }
            msg.Subject = "Order Successfully Confirmed";
            msg.Body = "<p>Your Order has&nbsp;been placed.</p><p><br></p><p><strong>Order Details:</strong></p><p>"
                +"<strong>Name: </strong>"+model.Name
                +"</p><p><strong>Email: </strong>"+model.Email
                +"</p><p><strong>Mobile Number: </strong>"+model.MobileNumber
                +"</p><p><strong>Address: </strong>"+model.Address
                +"</p><p><strong>Payment Method: </strong>"+model.PaymentMethod
                +"</p>";
            client.Send(msg);

            return RedirectToAction(nameof(OrderSuccess));
        }

        public IActionResult OrderSuccess()
        {
            TempData["msg"] = "Updated successfully";
            return View();
        }

        public IActionResult OrderFailure()
        {
            TempData["msg"] = "Updated";
            return View();
        }

    }
}
