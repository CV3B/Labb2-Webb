using Labb2_Api.Data;
using Labb2_Shared.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Labb2_Api.Controllers;

[ApiController]
[Route("categories")]
public class CategoriesController(IUnitOfWork unitOfWork) : ControllerBase
{
    /// <summary>
    /// Gets all the categories from the database
    /// </summary>
    /// <returns>Returns all the categories as a list</returns>
    [HttpGet]
    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await unitOfWork.CategoriesRepository.GetAll();
    }

    /// <summary>
    /// Create a new category
    /// </summary>
    /// <param name="category">Category that is going to be added with Id and Name</param>
    /// <returns></returns>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddCategory(Category category)
    {
        await unitOfWork.CategoriesRepository.Add(category);
        await unitOfWork.Save();

        return Ok(category);
    }

    /// <summary>
    /// Updates a category with new data
    /// </summary>
    /// <param name="id">Id of the category that needs to be updated</param>
    /// <param name="category">Category Name that is the updated name</param>
    /// <returns>Returns 200 Ok if it updated successfully or 404 Not Found if the id does not exist</returns>
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategory(Guid id, Category category)
    {
        var categoryToUpdate = await unitOfWork.CategoriesRepository.GetById(id);
        if (categoryToUpdate == null) return NotFound("Category not found");

        categoryToUpdate.Name = category.Name;

        unitOfWork.CategoriesRepository.Update(categoryToUpdate);
        await unitOfWork.Save();

        return Ok(categoryToUpdate);
    }

    /// <summary>
    /// Deletes a category
    /// </summary>
    /// <param name="id">Id of the category that is going to be deleted</param>
    /// <returns>Returns 200 Ok if it deleted successfully or 404 Not Found if the id does not exist</returns>
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var categoryToDelete = await unitOfWork.CategoriesRepository.GetById(id);
        if (categoryToDelete == null) return NotFound();

        await unitOfWork.CategoriesRepository.Delete(id);
        await unitOfWork.Save();

        return Ok(categoryToDelete);
    }
}