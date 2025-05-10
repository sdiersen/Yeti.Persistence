namespace Persistence.DTOs.Transaction;
public class ItemDTO
{
    public string Name { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public bool IsExpense { get; set; } = true;
    public decimal BudgetAmount { get; set; } = 0.0m;
    public int CategoryId { get; set; } = -1;
}
