content = """# Progetto Didattico: MealPlanner Recipes
## Documentazione Tecnica e Roadmap di Sviluppo

### 1. Visione del Progetto
**MealPlanner Recipes** è un'applicazione mobile cross-platform (sviluppata con .NET MAUI) progettata per semplificare la gestione quotidiana dell'alimentazione. L'obiettivo è offrire agli utenti uno strumento completo per scoprire nuove preparazioni, organizzare il calendario dei pasti e gestire la lista della spesa in modo efficiente e locale.

---

### 2. Architettura Funzionale

#### 2.1 Funzionalità Core (MVP)
Per garantire un prodotto minimo funzionante, lo sviluppo si concentrerà su:
* **Motore di Ricerca:** Query asincrone verso l'API esterna filtrate per ingrediente principale o categoria.
* **Scheda Ricetta:** Visualizzazione immersiva con supporto multimediale (immagini), elenco ingredienti normalizzato e procedura di preparazione.
* **Persistence (Local):** Sistema di salvataggio dei preferiti tramite database locale (SQLite).
* **Planner Settimanale:** Interfaccia a calendario per l'assegnazione delle ricette ai giorni della settimana.
* **Smart Shopping List:** Algoritmo di aggregazione degli ingredienti basato sulle ricette pianificate nel calendario.

#### 2.2 Funzionalità Premium / Avanzate
* **Filtri Internazionali:** Ricerca basata sull'area geografica (es. Italiana, Cinese, Messicana).
* **Personal Notes:** Possibilità di aggiungere annotazioni o varianti personali a ricette esistenti.
* **Pianificazione Granulare:** Suddivisione della giornata in Colazione, Pranzo e Cena.
* **User History:** Registro locale delle ultime ricette visualizzate.
* **Export:** Funzione di condivisione della lista della spesa tramite testo semplice (WhatsApp, Email, Note).

---

### 3. Specifiche Tecniche API: TheMealDB

Il backend dell'applicazione si appoggia a **TheMealDB**, un database aperto ed estremamente adatto all'uso didattico.
[TheMealDB](https://www.themealdb.com/api.php)
| Metodo | Endpoint | Parametro | Descrizione |
| :--- | :--- | :--- | :--- |
| **GET** | `/search.php?s={nome}` | Nome ricetta | Ricerca testuale globale |
| **GET** | `/filter.php?i={ingr}` | Nome ingrediente | Filtra ricette che contengono l'elemento |
| **GET** | `/lookup.php?i={id}` | ID Univoco | Restituisce il dettaglio completo |
| **GET** | `/categories.php` | - | Elenco di tutte le categorie disponibili |
| **GET** | `/random.php` | - | Suggerisce una ricetta casuale |

> **⚠️ Nota Implementativa Critica:**
> L'API restituisce gli ingredienti in un formato non normalizzato (20 campi singoli per ingrediente e 20 per la misura). È fondamentale implementare un logic layer nel **Data Transfer Object (DTO)** per mappare questi campi in una `List<Ingredient>` pulita, eliminando i valori nulli o vuoti.

---

### 4. Design dell'Interfaccia (UI/UX)

L'applicazione utilizzerà il pattern **MVVM (Model-View-ViewModel)**. Di seguito i componenti MAUI suggeriti per ogni vista:

| Schermata | Scopo | Componenti Chiave |
| :--- | :--- | :--- |
| **Search** | Discovery delle ricette | `SearchBar`, `CollectionView` con GridItemsLayout |
| **RecipeDetail** | Consultazione e istruzioni | `ScrollView`, `Image` (AspectFill), `Label` formattate |
| **Favorites** | Archivio personale | `CollectionView`, `SwipeView` (per eliminazione rapida) |
| **Planner** | Organizzazione temporale | `CollectionView` con raggruppamento per giorni |
| **ShoppingList** | Utility per acquisti | `CheckBox`, `Label` con TextDecoration (strikethrough) |

---

### 5. Roadmap di Sviluppo (Passo-passo)

#### Fase 1: Struttura e Navigazione
* Inizializzare il progetto .NET MAUI.
* Configurare la **AppShell** con `TabBar` per le sezioni principali (Cerca, Preferiti, Piano, Spesa).
* Creare le View e i rispettivi ViewModel (anche se vuoti).

#### Fase 2: Data Access Layer (REST Service)
Implementazione del servizio di rete.
* **Interfaccia:** `IRecipeService`.
* **Modelli:**
    * `RecipeDto`: Oggetto grezzo per il parsing JSON.
    * `RecipeListItem`: Oggetto leggero per le liste.
    * `Ingredient`: Classe di supporto (Name, Measure).
* **Logica:** Utilizzo di `HttpClient` per chiamate asincrone con gestione degli errori (Try/Catch).

#### Fase 3: UI Ricerca e Dettaglio
* Implementazione della `SearchPage` con binding alla `ObservableCollection` dei risultati.
* Creazione della pagina di dettaglio: passaggio dell'ID ricetta tramite query parameters e caricamento dati all'apertura.

#### Fase 4: Persistenza e Database
* Integrazione di **SQLite-net-PCL**.
* Definizione delle tabelle per `FavoriteRecipes` e `PlannedMeals`.
* Sviluppo della logica per "Aggiungi ai Preferiti" e "Pianifica Pasto".

#### Fase 5: Generazione Lista Spesa
* Sviluppo di un servizio `ShoppingListService` che analizza i pasti pianificati nel DB.
* Algoritmo di "flatting": estrae tutti gli ingredienti e li presenta in una lista di controllo.

#### Fase 6: Testing e Rifinitura
* Gestione degli stati "Empty" (es. nessuna ricetta trovata).
* Visualizzazione di un `ActivityIndicator` durante il caricamento.
* Rifinitura grafica con stili e colori coerenti.
* Generazione del pacchetto APK/AAB per Android.

---

### 6. Esempio Struttura DTO (Suggerimento Codice)
```csharp
public class RecipeDto {
    // Campi API standard...
    public string strIngredient1 { get; set; }
    public string strMeasure1 { get; set; }
    // ... fino a 20

    public List<Ingredient> GetIngredients() {
        var list = new List<Ingredient>();
        for (int i = 1; i <= 20; i++) {
            var name = (string)this.GetType().GetProperty($"strIngredient{i}").GetValue(this);
            var measure = (string)this.GetType().GetProperty($"strMeasure{i}").GetValue(this);
            if (!string.IsNullOrWhiteSpace(name))
                list.Add(new Ingredient { Name = name, Measure = measure });
        }
        return list;
    }
}
