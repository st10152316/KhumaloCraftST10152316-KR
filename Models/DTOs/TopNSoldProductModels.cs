namespace KhumaloCraftST10152316.Models.DTOs;

public record TopNSoldProductModel(string ProductName, string ProductDescription, int TotalUnitSold);
public record TopNSoldProductsVm(DateTime StartDate, DateTime EndDate, IEnumerable<TopNSoldProductModel> TopNSoldProducts);