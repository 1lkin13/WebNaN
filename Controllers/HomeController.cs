using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Diagnostics;
using WebNaN.Models;
//using WebNaN.Models;

namespace WebNaN.Controllers
{
    public class HomeController : Controller
    {
        private readonly WebNaNContext _sql;


        public HomeController(WebNaNContext sql)
        {
            _sql = sql;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Products()
        {

            ViewBag.Category = _sql.Categories.ToList();
            return View();

        }

        public IActionResult About()
        {
            return View();
        }


        public IActionResult AddCategory()
        {
            return View();
        }
        //public IActionResult RemoveCategory(int id)
        //{
        //    _sql.Products.RemoveRange(_sql.Products.Where(x => x.ProductCategoryId == id));
        //    _sql.Categories.Remove(_sql.Categories.SingleOrDefault(x => x.CategoryId == id));
        //    _sql.SaveChanges();
        //    return View();
        //}
        public IActionResult Edit(int id)
        {
            return View(_sql.Products.SingleOrDefault(x => x.ProductId == id));
        }
        public IActionResult ReadMore(int id)
        {
            var a = _sql.Products.Include(x => x.Photos).SingleOrDefault(x => x.ProductId == id);
            return View(a);
        }
        public IActionResult Host(int id)
        {
            return View(_sql.Products.Include(x=>x.Photos).OrderByDescending(x=>x.ProductId).Where(x => x.ProductCategoryId == id).ToList());

        }

        public IActionResult Delete(int id)
        {
            _sql.Products.Remove(_sql.Products.SingleOrDefault(x => x.ProductId == id));
            _sql.SaveChanges();
            return RedirectToAction("Host");
        }
        public IActionResult Add()
        {
            ViewBag.Categories = new SelectList(_sql.Categories.ToList(), "CategoryId", "CategoryName");
            return View();
        }
        [HttpPost]
        public IActionResult Add(Product product, IFormFile[] ProductPhoto)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = new SelectList(_sql.Categories.ToList(), "CategoryId", "CategoryName");
                return View();
            }

            _sql.Products.Add(product);
            _sql.SaveChanges();
            foreach (var item in ProductPhoto)
            {
                string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) +  Path.GetExtension(item.FileName);
                using (Stream stream = new FileStream("wwwroot/Image/more/" + filename, FileMode.Create))

                {
                    item.CopyTo(stream);
                }
                Photo p = new Photo();
                p.PhotoUrl = filename;
                p.PhotoProductId = product.ProductId;
                _sql.Photos.Add(p);

                _sql.SaveChanges();

            }


          
            return RedirectToAction("Add");
        }



        [HttpPost]
        public IActionResult AddCategory(Category category, IFormFile CategoryPic)

        {
            string filename = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".jpg";
            using (Stream stream = new FileStream("wwwroot/Image/" + filename, FileMode.Create))

            {
                CategoryPic.CopyTo(stream);
            }
            category.CategoryPic = filename;
            _sql.Categories.Add(category);
            _sql.SaveChanges();
            return RedirectToAction("Products");
        }





        [HttpPost]
        public IActionResult Update(int id, Product product, IFormFile ProductPhoto)

        {
            Product oldproduct = _sql.Products.SingleOrDefault(x => x.ProductId == id);
            if (ProductPhoto.FileName != null)

            {
                //using (Stream stream = new FileStream("wwwroot/Image/" + oldproduct.ProductPhoto, FileMode.Create))
                //{
                //    ProductPhoto.CopyTo(stream);
                //}
            }
            oldproduct.ProductName = product.ProductName;
            oldproduct.ProductInfo = product.ProductInfo;




            _sql.SaveChanges();
            return RedirectToAction("Host");
        }




        [HttpPost]
        [AllowAnonymous]
        public IActionResult SendMessage(string firstname, string lastname, string mail, string subject) { 


        
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(mail);
            email.To.Add(MailboxAddress.Parse("WebNaNCompany@gmail.com"));
            email.Subject = "Contact (" + firstname + " " + lastname +")";
            var builder = new BodyBuilder();
            builder.TextBody = subject;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect);
            smtp.Authenticate("WebNaNCompany@gmail.com", "nyxdqphjjpulwryn");
            smtp.Send(email);
            smtp.Disconnect(true);
            return RedirectToAction("Index");
        }





    }
}