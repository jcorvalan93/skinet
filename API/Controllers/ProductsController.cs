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
using Core.Specifications;
using API.Dtos;
using AutoMapper;
using API.Errors;
using API.Helpers;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _prodRepo;
        private readonly IGenericRepository<ProductBrand> _prodBrandRepo;
        private readonly IGenericRepository<ProductType> _prodTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> prodRepo,
                                  IGenericRepository<ProductBrand> prodBrandRepo,
                                  IGenericRepository<ProductType> prodTypeRepo,
                                  IMapper mapper)
        {
            _prodRepo = prodRepo;
            _prodBrandRepo = prodBrandRepo;
            _prodTypeRepo = prodTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts(
            [FromQuery]ProductsSpecParams productParams) 
        {
            var spec = new ProductWithTypesAndBrandsSpecification(productParams);

            var countSpec = new ProductWithFiltersForCountSpecification(productParams);

            var totalItems = await _prodRepo.CountAsync(countSpec);

            var products = await _prodRepo.ListAsync(spec);

            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);

            return Ok(new Pagination<ProductToReturnDto>(productParams.PageIndex, productParams.PageSize, totalItems, data));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id) 
        {
            var spec = new ProductWithTypesAndBrandsSpecification(id);

            var product = await _prodRepo.GetEntityWithSpec(spec);

            if (product == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<Product, ProductToReturnDto>(product);
        }
        
        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands() 
        {
            var productBrands = await _prodBrandRepo.ListAllAsync();

            return Ok(productBrands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes() 
        {
            var productTypes = await _prodTypeRepo.ListAllAsync();

            return Ok(productTypes);
        }

    }
}