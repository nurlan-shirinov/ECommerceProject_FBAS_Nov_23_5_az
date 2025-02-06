using ECommerce.Application.Abstract;
using ECommerce.Domain.Models;
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
        TempData["message"] = String.Format("Your product , {0} was added successfully to cart!", productToBeAdded.ProductName);
        return RedirectToAction("Index", "Product");
    }

    [HttpGet]
    public IActionResult List()
    {
        var cart = _cartSessionService.GetCart();
        var model = new CartListViewModel()
        {
            Cart = cart
        };
        return View(model);
    }

    public IActionResult Remove(int productId)
    {
        var cart = _cartSessionService.GetCart();
        _cartService.RemoveFromCart(cart, productId);
        _cartSessionService.SetCart(cart);
        TempData.Add("message", "Your Product deleted succesfully from cart");
        return RedirectToAction("List");
    }

    [HttpGet]
    public IActionResult Complete()
    {
        var shippingDetailViewModel = new ShippingDetailsViewModel
        {
            ShippingDetails = new ShippingDetails()
            {
                Address = string.Empty,
                Age = string.Empty,
                City = string.Empty,
                Email = string.Empty,
                Firstname = string.Empty,
                Lastname = string.Empty
            }
        };
        return View(shippingDetailViewModel);
    }

    [HttpPost]
    public IActionResult Complete(ShippingDetailsViewModel model)
    {
        if (!ModelState.IsValid)
            return View();
        TempData.Add("message", String.Format("Thamk you {0} , your order is in progress", model.ShippingDetails.Firstname));
        return View();

    }
}