using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ADO.DataAccessLayer;
using ADO.Models;

namespace ADO.Controllers
{
    public class ProductController : Controller
    {
        Product_DAL pDal = new Product_DAL();
        // GET: Product
        public ActionResult Index()
        {
            var productList = pDal.GetAllProducts();
            if(productList.Count == 0)
            {
                TempData["InfoMessage"] = "Currently products not available";
            }
            return View(productList);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var product = pDal.GetProductsById(id).FirstOrDefault();

                if (product == null)
                {
                    TempData["InfoMessage"] = "";
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                throw;
            }
        }

        // GET: Product/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            bool IsInserted = false;
            try
            {
                if (ModelState.IsValid)
                {
                    IsInserted = pDal.InsertProduct(product);

                    if (IsInserted)
                    {
                        TempData["SuccessMessage"] = "Product details saved successfully..!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Product already present/Unable to save the product details";
                    }
                    
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMesssage"] = ex.Message;
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        { 
            var products = pDal.GetProductsById(id).FirstOrDefault();

            if (products == null)
            {
                TempData["InfoMessage"] = "Product not available with ID "+ id.ToString();
                return RedirectToAction("Index");
            }
            return View(products);
        }

        // POST: Product/Edit/5
        [HttpPost, ActionName("Edit")]
        public ActionResult UpdateProduct(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    bool IsUpdate = pDal.UpdateProduct(product);

                    if (IsUpdate)
                    {
                        TempData["SuccessMessage"] = "Product details updated successfully..!";

                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Product already present/ Unable to save the product details";
                    }
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }

        // GET: Product/Delete/5
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            try
            {
                string result = pDal.DeleteProduct(id);

                if (result.Contains("Deleted"))
                {
                    TempData["SuccessMessage"] = result;
                }
                else
                {
                    TempData["ErrorMessage"] = result;
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
                throw;
            }
        }

        // POST: Product/Delete/5
        
        public ActionResult Delete(int id)
        {
            var product = pDal.GetProductsById(id).FirstOrDefault();
         
            try
            {
                // TODO: Add delete logic here
                if(product == null)
                {
                    TempData["infoMessage"] = "Product not available with id" + id.ToString();
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return View();
            }
        }
    }
}
