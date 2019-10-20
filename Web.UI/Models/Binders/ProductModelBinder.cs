using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.UI.Models.Binders
{
    public class ProductModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            HttpRequestBase request = controllerContext.HttpContext.Request;

            List<string> nameValues = new List<string>();

            int id = Convert.ToInt32(request.Form.Get("Id"));
            string name = request.Form.Get("Name");
            string code = request.Form.Get("Code");
            decimal price = Convert.ToDecimal(request.Form.Get("Price"));
            bool confirmPrice = Convert.ToBoolean(request.Form.Get("ConfirmPrice").Split(",".ToCharArray()).FirstOrDefault());

            return new ProductModel()
            {
                Id = id,
                Name = name,
                Code = code,
                Price = price,
                ConfirmPrice = confirmPrice
            };
        }
    }
}