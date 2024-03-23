using System.ComponentModel.DataAnnotations;

namespace AemAssessment.Models {
  public class ExternalPlatformModel {
    public int id { get; set; }
    public string uniqueName { get; set; }
    public Double latitude { get; set; }
    public Double longitude { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    public List<ExternalWellModel> well { get; set; }
}
}