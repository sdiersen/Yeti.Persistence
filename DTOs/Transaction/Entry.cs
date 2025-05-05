namespace Persistence.DTOs.Transaction;

public class EntryDTO
{
    public DateTime EntryDate { get; set; }
    public decimal Amount { get; set; }
    public bool IsExpense { get; set; }
    public string Note { get; set; } = string.Empty;
    public int ItemId { get; set; }
    public int CategoryId { get; set; }

    public static EntryDTO DefaultEntry()
    {
        return new EntryDTO
        {
            EntryDate = DateTime.Now,
            Amount = 0.0m,
            IsExpense = true,
            Note = string.Empty,
            ItemId = -1,
            CategoryId = -1
        };
    }
}


