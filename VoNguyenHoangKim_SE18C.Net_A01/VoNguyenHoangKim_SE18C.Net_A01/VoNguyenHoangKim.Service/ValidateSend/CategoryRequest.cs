using VoNguyenHoangKim.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace VoNguyenHoangKim.Service.ValidateSend
{
    public class CategoryRequest
    {
        [Required(ErrorMessage = "Category Name is required !!!")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required !!!")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Status is required !!!")]
        [EnumDataType(typeof(Status), ErrorMessage = "Invalid status !!!")]
        public Status Status { get; set; }
    }
}