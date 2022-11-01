﻿using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Compass.Wasm.Client.CategoryService;
using Compass.Wasm.Shared.CategoryService;

namespace Compass.Wasm.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
[UnitOfWork(typeof(CSDbContext))]
//[Authorize(Roles = "admin,pm,designer")]
public class ProductController : ControllerBase
{
    private readonly CSDomainService _domainService;
    private readonly CSDbContext _dbContext;
    private readonly ICSRepository _repository;
    private readonly IMapper _mapper;
    public ProductController(CSDomainService domainService, CSDbContext dbContext, ICSRepository repository, IMapper mapper)
    {
        _domainService = domainService;
        _dbContext = dbContext;
        _repository = repository;
        _mapper = mapper;
    }
    [HttpGet("All/{sbu}")]
    public async Task<ProductResponse[]> FindAllBySbu([Required] Sbu sbu)
    {
        //使用AutoMapper将Product转换成ProductResponse（Dto）
        return await _mapper.ProjectTo<ProductResponse>(await _repository.GetProductsAsync(sbu)).ToArrayAsync();
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse?>> FindById([RequiredGuid] Guid id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        if (product == null) return NotFound($"没有Id={id}的Product");
        return _mapper.Map<ProductResponse>(product);
    }
    [HttpPost]
    public async Task<ActionResult<Guid>> Add(AddProductRequest request)
    {
        Product product = await _domainService.AddProductAsync(request.Name, request.Sbu);
        await _dbContext.Products.AddAsync(product);
        return product.Id;
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> Update([RequiredGuid] Guid id, ProductResponse request)
    {
        var product = await _repository.GetProductByIdAsync(id);
        if (product == null) return NotFound($"没有Id={id}的Product");
        product.ChangeName(request.Name).ChangeSbu(request.Sbu);
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([RequiredGuid] Guid id)
    {
        var product = await _repository.GetProductByIdAsync(id);
        //这样做仍然是幂等的，因为“调用N次，确保服务器处于与第一次调用相同的状态。”与响应无关
        if (product == null) return NotFound($"没有Id={id}的Product");
        product.SoftDelete();
        return Ok();
    }
    [HttpPut("Sort/{sbu}")]
    public async Task<ActionResult> Sort([Required] Sbu sbu, CategorySortRequest request)
    {
        await _domainService.SortProductsAsync(sbu, request.SortedIds);
        return Ok();
    }
}