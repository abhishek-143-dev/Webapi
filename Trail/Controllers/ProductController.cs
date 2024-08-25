using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;
using System.Xml.Linq;
using Trail.Data;
using Trail.Model;

namespace Trail.Controllers
{
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly DBdata dBdata;

        public ProductController(DBdata dBdata)
        {
            this.dBdata = dBdata;
        }
        [Route("/Products")]
        [HttpGet]
        public ActionResult Index() {
            return Ok(dBdata.Products);
        }

        [Route("/Products/{id}")]

        [HttpGet]
        public ActionResult GetProduct(int id)
        {
            var product = dBdata.Products.FirstOrDefault(x=>x.id==id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpPost]
        [Route("/Product")]
        public ActionResult AddProduct([FromBody] Product product)
        {

            var productchek=dBdata.Products.FirstOrDefault(x => x.ProductName == product.ProductName || x.SupplierId==product.SupplierId);
            if(productchek != null)
            {
                return BadRequest("Product alredy avalible");
            }
            Product models = new Product()
            {
                ProductName = product.ProductName,
                IsDiscontinued = product.IsDiscontinued,
                SupplierId = product.SupplierId,
                Package = product.Package,
                UnitPrice = product.UnitPrice,
            };
            dBdata.Products.Add(models);
            dBdata.SaveChanges();
            return Ok();

        }
        [HttpPut]
        [Route("/Product/{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {

            var productupdate = dBdata.Products.FirstOrDefault(x => x.id == id);
            if (productupdate == null)
            {
                return NotFound("No product with this id");
            }
            else
            {
                //Product model = new Product()
                //{

                //    ProductName = product.ProductName,
                //    IsDiscontinued = product.IsDiscontinued,
                //    SupplierId = product.SupplierId,
                //    Package = product.Package,
                //    UnitPrice = product.UnitPrice,
                //};
                //if(product.ProductName != null)
                //{
                //    productupdate.ProductName = product.ProductName;
                //    dBdata.Products.Update(productupdate);
                //}
                //if (product.IsDiscontinued != null)
                //{
                //    productupdate.IsDiscontinued = product.IsDiscontinued;
                //    dBdata.Products.Update(productupdate);
                //}
                //if (product.SupplierId != null)
                //{
                //    productupdate.SupplierId = product.SupplierId;
                //    dBdata.Products.Update(productupdate);
                //}
                //if (product.Package != null)
                //{
                //    productupdate.Package = product.Package;
                //    dBdata.Products.Update(productupdate);
                //}

                productupdate.ProductName = product.ProductName;
                productupdate.IsDiscontinued = product.IsDiscontinued;
                productupdate.SupplierId = product.SupplierId;
                productupdate.Package = product.Package;
                productupdate.UnitPrice = product.UnitPrice;
                dBdata.Products.Update(productupdate);
                dBdata.SaveChanges();

                return Ok();

            }
        }
        [HttpDelete]
        [Route("/Product/{id}")]
        public IActionResult Delete(int id)
        {
            var product = dBdata.Products.FirstOrDefault(x => x.id == id);
            if (product == null)
            {
                return NotFound("No product with this id");
            }
            dBdata.Products.Remove(product);
            dBdata.SaveChanges();
            return Ok();
        }


    }
}
