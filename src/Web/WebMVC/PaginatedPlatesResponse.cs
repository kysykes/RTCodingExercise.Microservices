﻿using Catalog.Application.Dtos;

namespace WebMVC
{
    public class PaginatedPlatesResponse
    {
        public List<PlateDto> Items { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }

}
