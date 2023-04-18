﻿namespace Compass.Wasm.Shared.Categories;

public class CategoryResponse
{
    public Sbu_e Sbu { get; set; }
    public Guid ProductId { get; set; }
    public Guid ModelId { get; set; }
    public Guid ModelTypeId { get; set; }
    public string ModelName { get; set; } = string.Empty;
    public string ModelTypeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Length { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
}