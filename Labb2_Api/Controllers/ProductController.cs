using Labb2_Api.Data;
using Labb2_Shared.DTO;
using Labb2_Shared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb2_Api.Controllers;

[ApiController]
[Route("products")]
public class ProductController(IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>
    /// Gets all the products from the database
    /// </summary>
    /// <returns>Returns a list of all the products</returns>
    [HttpGet]
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await unitOfWork.ProductRepository.GetAll();
    }

    /// <summary>
    /// Gets a product by id or name
    /// </summary>
    /// <param name="id">Id of the product</param>
    /// <param name="name">Name of the product</param>
    /// <returns>Returns a product</returns>
    [HttpGet("search")]
    public async Task<IActionResult> GetProductsBySearch(Guid id, string? name)
    {
        if (!id.Equals(Guid.Empty))
        {
            var product = await unitOfWork.ProductRepository.GetById(id);
            return product is not null ? Ok(product) : NotFound("Product not found");
        }

        if (!string.IsNullOrEmpty(name))
        {
            var product = await unitOfWork.ProductRepository.GetByName(name);
            return product is not null ? Ok(product) : NotFound("Product not found");
        }

        return BadRequest("Wrong Product Id");
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="product">Product data to the new product</param>
    /// <returns>Returns the new product or Not Found if it failed</returns>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddProduct([FromBody] ProductDTO product)
    {
        if (product == null) return NotFound("Product not correct");

        var newProduct = new Product()
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            Status = product.Status,
            ImageLink = product.ImageLink,
        };

        await unitOfWork.ProductRepository.Add(newProduct);
        await unitOfWork.Save();

        return Ok(newProduct);
    }

    /// <summary>
    /// Update a product
    /// </summary>
    /// <param name="id">Id of the product that is going to be updated</param>
    /// <param name="product">Product data for the new product</param>
    /// <returns>Returns the updated product or Not found if it failed</returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductDTO product)
    {
        var productToUpdate = await unitOfWork.ProductRepository.GetById(id);
        if (productToUpdate == null) return NotFound("Something went wrong");

        productToUpdate.Name = product.Name;
        productToUpdate.Description = product.Description;
        productToUpdate.Price = product.Price;
        productToUpdate.CategoryId = product.CategoryId;
        productToUpdate.Status = product.Status;
        productToUpdate.ImageLink = product.ImageLink;

        unitOfWork.ProductRepository.Update(productToUpdate);
        await unitOfWork.Save();

        return Ok(productToUpdate);
    }

    /// <summary>
    /// Mark a product as discontinued
    /// </summary>
    /// <param name="id">Id of the product</param>
    /// <returns>Returns the marked product or not found if it failed</returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("/markAsDiscontinued/{id}")]
    public async Task<IActionResult> PutProductDiscontinued(Guid id)
    {
        var productToUpdate = await unitOfWork.ProductRepository.GetById(id);
        if (productToUpdate == null) return NotFound("Product not found");

        await unitOfWork.ProductRepository.MarkAsDiscontinued(productToUpdate.Id);
        await unitOfWork.Save();

        return Ok(productToUpdate);
    }

    /// <summary>
    /// Delete a Product
    /// </summary>
    /// <param name="id">Id of the product to delete</param>
    /// <returns>Returns Ok if successful or not found if it failed</returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        if (await unitOfWork.ProductRepository.GetById(id) == null) return NotFound("Product not found");

        await unitOfWork.ProductRepository.Delete(id);
        await unitOfWork.Save();

        return Ok("Product deleted");
    }
}