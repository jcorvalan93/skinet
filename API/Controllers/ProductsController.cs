using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Data;
using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;
        public ProductsController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts() 
        {
            var products = await _repo.GetProductsAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id) {
            return await _repo.GetProductByIdAsync(id);
        }
        
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands() 
        {
            var productBrands = await _repo.GetProductBrandsAsync();

            return Ok(productBrands);
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<ProductBrand>> GetProductBrand(int id) {
        //     return await _repo.GetProductBrandByIdAsync(id);
        // }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes() 
        {
            var productTypes = await _repo.GetProductTypesAsync();

            return Ok(productTypes);
        }

        // [HttpGet("{id}")]
        // public async Task<ActionResult<ProductType>> GetProductType(int id) 
        // {
        //     return await _repo.GetProductTypeByIdAsync(id);
        // }
    }
}