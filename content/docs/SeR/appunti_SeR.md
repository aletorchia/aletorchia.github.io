---
title: "Protocolli Routing"
type: "docs"
weight: 1
---
# Protocolli di Routing

I protocolli di routing sono fondamentali per determinare il percorso che i dati devono seguire all'interno di una rete. Si basano principalmente su due tipi di algoritmi:

## Algoritmi di Base
* **Distance Vector**: Utilizza la distanza e il vettore delle distanze per calcolare il percorso, non conoscono la topografia, Si basano sul **costo** (metrica) e sull'**interfaccia di uscita** (*next hop*).
* **Link State**: Si basa sulla conoscenza completa della topologia della rete da parte di ogni router.

---

## Caratteristiche di un Protocollo di Routing
Per essere efficace, un protocollo di routing deve soddisfare i seguenti requisiti:

### 1. Ottimalità
Capacità di fornire il **percorso migliore** attraverso l'Internetwork. La scelta dipende dalla **metrica** definita dal protocollo (es. distanza minore, percorso più veloce, minor numero di hop, ecc.).

### 2. Imparzialità
Il protocollo deve utilizzare tutte le linee disponibili per distribuire il traffico in modo equo, evitando congestioni. 
> **Nota:** Spesso occorre trovare un compromesso tra *ottimalità* e *imparzialità*, poiché possono entrare in conflitto tra loro.

### 3. Flessibilità
Capacità di **adattamento ai cambiamenti** della topologia di rete (ad esempio, se un router si guasta o viene aggiunta una nuova tratta).

### 4. Convergenza
Il processo mediante il quale i cambiamenti della topologia si propagano attraverso tutti i router della rete, affinché tutti abbiano una visione coerente e aggiornata.

### 5. Robustezza
Il protocollo deve continuare a funzionare correttamente anche in presenza di configurazioni errate, guasti ai componenti o in contesti non ordinari.

### 6. Semplicità
La metrica e l'algoritmo devono essere efficienti e "leggeri", in modo da poter girare sui dispositivi senza sovraccaricarne le risorse hardware.