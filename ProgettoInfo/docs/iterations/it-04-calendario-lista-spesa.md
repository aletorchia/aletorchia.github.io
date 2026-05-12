# IT-04 - Calendario e lista spesa nella build corrente

## Obiettivo

Aggiungere alla build corrente un calendario pasti locale e una lista spesa locale derivata dalle ricette pianificate, mantenendo MVVM e Shell.

## Piano

- aggiungere due sezioni Shell: `Calendario` e `Spesa`;
- introdurre un servizio locale per pianificazione pasti e stato lista spesa;
- estendere il dettaglio ricetta con azione "Aggiungi al calendario";
- implementare pagine e ViewModel per visualizzare/rimuovere pasti pianificati;
- implementare pagine e ViewModel per lista spesa derivata + aggiunta manuale;
- verificare build Android.

## Prompt principali utilizzati

1. "no devi aggiungere il calendario e la lista della spesa in questa build"

## File creati

- `src/MealWise.Mobile/Models/PlannedMeal.cs`
- `src/MealWise.Mobile/Models/ShoppingListItem.cs`
- `src/MealWise.Mobile/Services/IMealPlanService.cs`
- `src/MealWise.Mobile/Services/MealPlanService.cs`
- `src/MealWise.Mobile/ViewModels/MealCalendarViewModel.cs`
- `src/MealWise.Mobile/ViewModels/ShoppingListViewModel.cs`
- `src/MealWise.Mobile/Views/MealCalendarPage.xaml`
- `src/MealWise.Mobile/Views/MealCalendarPage.xaml.cs`
- `src/MealWise.Mobile/Views/ShoppingListPage.xaml`
- `src/MealWise.Mobile/Views/ShoppingListPage.xaml.cs`
- `docs/iterations/it-04-calendario-lista-spesa.md`

## File modificati

- `src/MealWise.Mobile/AppShell.xaml`
- `src/MealWise.Mobile/MauiProgram.cs`
- `src/MealWise.Mobile/ViewModels/HomeViewModel.cs`
- `src/MealWise.Mobile/ViewModels/RecipeDetailViewModel.cs`
- `src/MealWise.Mobile/Views/HomePage.xaml`
- `src/MealWise.Mobile/Views/RecipeDetailPage.xaml`
- `docs/plan.md`
- `docs/test-matrix.md`

## Implementazione

- Persistenza locale con `Preferences` e serializzazione JSON per pasti pianificati e stato lista spesa.
- Da `RecipeDetailPage` e' possibile scegliere una data e aggiungere la ricetta al calendario.
- `MealCalendarPage` mostra i pasti pianificati e permette rimozione.
- `ShoppingListPage` deriva ingredienti dai pasti pianificati, supporta spunta/rimozione e aggiunta manuale.
- Aggiunte due tab Shell dedicate e collegamenti rapidi dalla Home.

## Test eseguiti

- [x] `dotnet build src/MealWise.Mobile/MealWise.Mobile.csproj -c Debug`
- [ ] Test manuale persistenza dopo riavvio app.
- [ ] Test manuale generazione lista spesa dopo pianificazione di almeno due ricette.
- [ ] Test manuale comportamento in assenza rete (le funzioni locali devono restare disponibili).

## Rischi aperti

- La persistenza e' locale ma non usa ancora SQLite: passaggio a SQLite possibile in iterazione successiva senza cambiare UI.
- Aggregazione misure ingredienti semplice (join testuale), da rifinire se si richiede normalizzazione avanzata.

## Esito

Calendario e lista spesa sono inclusi nella build corrente, compilano e sono navigabili da Shell.
