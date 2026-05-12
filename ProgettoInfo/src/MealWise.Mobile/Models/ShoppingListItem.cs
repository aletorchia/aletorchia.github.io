namespace MealWise.Mobile.Models;

public sealed class ShoppingListItem
{
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Measure { get; set; } = string.Empty;

    public bool IsChecked { get; set; }

    public bool IsManual { get; set; }

    public int SourceCount { get; set; }

    public string StatusText => IsChecked ? "Preso" : "Da comprare";

    public string ToggleActionText => IsChecked ? "Ripristina" : "Spunta";

    public string SourceText
    {
        get
        {
            if (IsManual)
            {
                return "Manuale";
            }

            return SourceCount == 1
                ? "Da 1 ricetta pianificata"
                : $"Da {SourceCount} ricette pianificate";
        }
    }
}
