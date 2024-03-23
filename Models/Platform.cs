using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AemAssessment.Models {
  public class Platform {
    public int Id { get; set; }
    [Key]
    public int PlatformId { get; set; }
    public string Name { get; set; }
    public Double Latitude { get; set; }
    public Double Longitude { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }  
    public List<Well> Wells { get; set; }
  }
}