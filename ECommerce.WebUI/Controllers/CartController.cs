using ECommerce.Application.Abstract;
using ECommerce.WebUI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebUI.Controllers;

public class CartController(IProductService productService, ICartSessionService cartSessionService, ICartService cartService) : Controller
{

    private readonly IProductService _productService = productService;
    private readonly ICartSessionService _cartSessionService = cartSessionService;
    private readonly ICartService _cartService = cartService;

    public IActionResult AddToCart(int productId)
    {
        var productToBeAdded = _productService.GetById(productId);
        var cart = _cartSessionService.GetCart();

        _cartService.AddToCart(cart, productToBeAdded);
        _cartSessionService.SetCart(cart);

        if (TempData.ContainsKey("message"))
            TempData["message"] = String.Format("Your product , {0} was added successfully to cart!",productToBeAdded.ProductName);
        TempData.Add("message", String.Format("Your product , {0} was added successfully to cart!",productToBeAdded.ProductName));


        return RedirectToAction("Index", "Product");
    }
}