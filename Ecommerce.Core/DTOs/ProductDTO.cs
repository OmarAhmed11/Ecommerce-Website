using Ecommerce.Core.Entities.Product;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Core.DTOs
{
    public record ProductDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public virtual List<ImageDTO> Images { get; set; }
        public string CategoryName { get; set; }

    }
    public record ImageDTO {
        public string ImageName { get; set; }
        public int ProductId { get; set; }
    }
    public record AddProductDTO {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public IFormFileCollection Images { get; set; }
        public int CategoryId { get; set; }
    }
    public record UpdateProductDTO : AddProductDTO
    {
        public int Id { get; set; }
    }
}
