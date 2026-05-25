---
title: ".NET MAUI"
type: "docs"
weight: 1
---
# Guida tecnica completa a .NET MAUI

## Sintesi

.NET MAUI, cioè **Multi-platform App UI**, è un framework Microsoft per creare applicazioni **mobile e desktop** usando una sola base di codice.

Permette di sviluppare app per:

- Android
- iOS
- Windows
- macOS

I concetti principali sono:

- separazione tra interfaccia grafica e logica;
- uso di **XAML** per creare la UI;
- uso di **C#** per gestire il comportamento dell’app;
- utilizzo del pattern **MVVM**;
- gestione dei dati locali con **SQLite**;
- navigazione semplificata tramite **Shell**.

---

# 1. Architettura e struttura del progetto

Un progetto .NET MAUI contiene diversi file importanti.

## File principali

### `MauiProgram.cs`

Contiene il codice iniziale dell’applicazione.

Serve a configurare:

- servizi;
- font;
- librerie;
- impostazioni principali dell’app.

---

### `App.xaml` e `App.xaml.cs`

Rappresentano l’applicazione nel suo insieme.

`App.xaml` contiene risorse grafiche, stili e impostazioni generali.

`App.xaml.cs` contiene la logica principale dell’app e crea la finestra iniziale.

---

### `AppShell.xaml` e `AppShell.xaml.cs`

Gestiscono la struttura della navigazione.

Servono per definire:

- pagina iniziale;
- menu;
- schede;
- percorsi di navigazione.

---

### `MainPage.xaml` e `MainPage.xaml.cs`

Sono i file della pagina principale dell’app.

`MainPage.xaml` contiene il layout grafico.

`MainPage.xaml.cs` contiene la logica collegata alla pagina.

---

## La classe `Application`

La classe `App` rappresenta l’applicazione completa.

Gestisce anche alcuni eventi del ciclo di vita, ad esempio:

- apertura dell’app;
- chiusura;
- sospensione;
- ritorno in primo piano.

Di base un’app .NET MAUI usa una singola finestra, ma può supportare anche più finestre, soprattutto su desktop e tablet.

---

# 2. Progettazione dell’interfaccia utente

L’interfaccia grafica di .NET MAUI è organizzata in modo gerarchico.

## Gerarchia degli elementi visivi

### Pages

Le **Pages** sono le pagine dell’app.

Esempi:

- `ContentPage`: pagina semplice;
- `TabbedPage`: pagina con schede;
- `FlyoutPage`: pagina con menu laterale.

---

### Views

Le **Views** servono per mostrare contenuti o dati.

Esempi:

- `ScrollView`: permette di scorrere il contenuto;
- `CarouselView`: mostra elementi scorribili;
- `CollectionView`: mostra liste di dati.

---

### Layouts

I **Layouts** servono per posizionare gli elementi nella pagina.

---

### Controls

I **Controls** sono gli elementi grafici singoli.

Esempi:

- `Button`;
- `Label`;
- `Entry`;
- `Editor`.

---

# 3. Tipi di layout

## `StackLayout`

Organizza gli elementi in verticale o in orizzontale.

È utile per layout semplici e lineari.

---

## `Grid`

Organizza gli elementi usando righe e colonne.

È molto utile per interfacce più complesse.

È preferibile rispetto a troppi `StackLayout` annidati.

---

## `FlexLayout`

È simile allo `StackLayout`, ma permette agli elementi di adattarsi meglio allo spazio disponibile.

È utile per design responsive.

---

## `AbsoluteLayout`

Permette di posizionare gli elementi usando coordinate precise.

È utile quando serve un controllo totale sulla posizione degli oggetti.

---

# 4. Spaziatura e ottimizzazione del layout

.NET MAUI usa alcune proprietà per gestire gli spazi.

## `Margin`

Indica lo spazio esterno di un controllo.

Serve ad allontanare un elemento dagli altri.

---

## `Padding`

Indica lo spazio interno tra il bordo di un layout e i suoi elementi figli.

---

## `Spacing`

Indica lo spazio tra gli elementi dentro un `VerticalStackLayout` o un `HorizontalStackLayout`.

---

# 5. Navigazione con .NET MAUI Shell

La **Shell** semplifica la navigazione dell’app.

Permette di gestire:

- gerarchia delle pagine;
- navigazione tra schermate;
- percorsi basati su URI;
- ricerca integrata;
- menu e schede.

---

## Navigazione a schede

La navigazione a schede si crea con `TabBar`.

Di solito è consigliato usare **3 o 4 schede**, perché su mobile troppe schede diventano difficili da visualizzare.

Se le sezioni sono troppe, è meglio usare un menu laterale, cioè una navigazione **Flyout**.

---

# 6. Data Binding

Il **Data Binding** collega i dati alla grafica.

Serve per sincronizzare automaticamente la UI con le informazioni dell’app.

## Componenti principali

### Source

È la sorgente dei dati.

Di solito è un oggetto o un servizio.

---

### Target

È la destinazione dei dati.

Di solito è una proprietà di un controllo grafico.

---

### BindingContext

È la sorgente predefinita usata dai controlli figli.

Permette di collegare facilmente la View al ViewModel.

---

# 7. Converter

A volte i dati della sorgente non sono nello stesso formato richiesto dalla UI.

Per risolvere questo problema si usa un **Converter**.

Un Converter implementa l’interfaccia `IValueConverter`.

Serve a trasformare un dato prima di mostrarlo nella grafica.

Esempio:

- una stringa `"Sunny"` può essere trasformata in un’immagine del sole;
- un valore booleano può essere trasformato in un colore;
- un numero può essere trasformato in testo.

---

# 8. Pattern MVVM

Il pattern **MVVM** significa:

- Model;
- View;
- ViewModel.

Serve a separare la grafica dalla logica dell’app.

---

## Model

Il **Model** rappresenta i dati e la logica principale.

Esempi:

- utenti;
- prodotti;
- dipendenti;
- reparti.

---

## View

La **View** rappresenta l’interfaccia grafica.

Di solito è scritta in XAML.

Non dovrebbe contenere la logica principale dell’app.

---

## ViewModel

Il **ViewModel** collega il Model alla View.

Si occupa di:

- preparare i dati;
- formattarli;
- esporre proprietà;
- gestire comandi;
- aggiornare la UI.

---

# 9. `INotifyPropertyChanged`

Per aggiornare automaticamente la View quando cambia un dato, il ViewModel deve implementare `INotifyPropertyChanged`.

Questa interfaccia permette di notificare alla UI che una proprietà è stata modificata.

Il **MVVM Toolkit** può semplificare questo lavoro usando attributi come:

```csharp
[ObservableProperty]
