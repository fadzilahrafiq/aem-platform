using System.ComponentModel.DataAnnotations;

namespace AemAssessment.Models {
  public class ExternalWellModel {
    public int id { get; set; }
    public int platformId { get; set; }
    public string uniqueName { get; set; }
    public Double latitude { get; set; }
    public Double longitude { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
  }
}