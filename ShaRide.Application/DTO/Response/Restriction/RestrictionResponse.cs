namespace ShaRide.Application.DTO.Response.Restriction
{
    public class RestrictionResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AssetPath { get; set; }
        public bool IsPossible { get; set; }
    }
}