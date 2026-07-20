using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Queries;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Catalog.Application.Mappers;

namespace Catalog.API.Controllers
{
    /// <summary>
    /// API Controller for managing catalog products, brands, and product types.
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="CatalogController"/> class.
        /// </summary>
        /// <param name="mediator">Mediator instance used to send commands and queries.</param>
        public CatalogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Retrieves all products with optional filtering, sorting, searching, and pagination.
        /// </summary>
        /// <param name="catalogSpecParams">Filtering and pagination parameters.</param>
        /// <returns>List of products.</returns>
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IList<ProductDto>>> GetAllProducts([FromQuery] CatalogSpecParams catalogSpecParams)
        {
            // Create query object
            var query = new GetAllProductsQuery(catalogSpecParams);

            // Send query through MediatR
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">Product Id.</param>
        /// <returns>Product details.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(string id)
        {
            var query = new GetProductByIdQuery(id);
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves products matching the specified product name.
        /// </summary>
        /// <param name="productName">Product name.</param>
        /// <returns>List of matching products.</returns>
        [HttpGet("productName/{productName}")]
        public async Task<ActionResult<IList<ProductDto>>> GetProductByProductName(string productName)
        {
            // Create query
            var query = new GetProductByNameQuery(productName);

            // Execute query
            var result = await _mediator.Send(query);

            // Return 404 if no matching products are found
            if (result == null || !result.Any())
            {
                return NotFound();
            }

            // Convert domain entities to DTOs
            var dtoList = result.Select(p => p.ToDto()).ToList();

            return Ok(dtoList);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="command">Product creation request.</param>
        /// <returns>Created product.</returns>
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductCommand command)
        {
            // Execute create command
            var result = await _mediator.Send(command);

            return Ok(result);
        }

        /// <summary>
        /// Deletes a product by its Id.
        /// </summary>
        /// <param name="id">Product Id.</param>
        /// <returns>No content if deleted successfully.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var command = new DeleteProductByIdCommand(id);

            var result = await _mediator.Send(command);

            // Return 404 if product doesn't exist
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">Product Id.</param>
        /// <param name="updateProductDto">Updated product information.</param>
        /// <returns>No content if updated successfully.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductDto updateProductDto)
        {
            // Convert DTO into Update Command
            var command = updateProductDto.ToCommand(id);

            var result = await _mediator.Send(command);

            // Return 404 if product not found
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Retrieves all available product brands.
        /// </summary>
        /// <returns>List of brands.</returns>
        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<IEnumerable<BrandDto>>> GetBrands()
        {
            var query = new GetAllBrandsQuery();

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves all available product types.
        /// </summary>
        /// <returns>List of product types.</returns>
        [HttpGet("GetAllTypes")]
        public async Task<ActionResult<IEnumerable<TypeDto>>> GetTypes()
        {
            var query = new GetAllTypesQuery();

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        /// <summary>
        /// Retrieves products belonging to a specific brand.
        /// </summary>
        /// <param name="brand">Brand name.</param>
        /// <returns>List of products for the specified brand.</returns>
        [HttpGet("/brand/{brand}", Name = "GetProductsByBrandName")]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProductsByBrand(string brand)
        {
            // Create query using brand name
            var query = new GetProductsByBrandQuery(brand);

            // Execute query
            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}