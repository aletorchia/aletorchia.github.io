---
title: "protocolli routing"
type: "docs"
weight: 1
---
#Internet Control Message Protocol (ICMP)

L’ICMP (Internet Control Message Protocol), **RFC 792**, fornisce un meccanismo di monitoraggio della rete, utilizzato prevalentemente dai router o dagli host destinatari per segnalare agli host mittenti eventuali insuccessi nell’instradamento dei pacchetti.

**#prendinota**
ICMP è spesso considerato parte di IP, in realtà, nell’architettura TCP/IP, è posizionato sopra IP. Infatti i messaggi ICMP sono trasportati all’interno del datagram IP, come payload.

Il pacchetto ICMP viene incapsulato nel pacchetto IP ed è caratterizzato da 4 campi:

* Type
  -  è il più significativo, 8 bit che indicano il tipo di pacchetto ICMP trasmesso.
* Code
	* fornisce indicazioni aggiuntive non comprese nel campo Type
* Checksum
	* contiene i bit per il controllo degli errori di trasmissione
* Type Specific Data
	* contiene informazioni che dipendono dal tipo di servizio che l’ICMP sta offrendo..

---

# Le funzioni svolte da ICMP

Le principali funzioni che il protocollo ICMP può svolgere sono:

* fornire messaggi di eco per verificare la corretta configurazione di un host sulla rete e che quindi una qualsiasi destinazione sia raggiungibile: Echo Request (Type 8) del mittente, Echo Reply (Type 0) del destinatario. Si realizza con il comando **ping**;
* segnalare una destinazione non raggiungibile perché sconosciuta o perché un pacchetto è troppo grande ma non è consentito frammentarlo: Destination Unreachable (Type 3);
* avvertire il mittente di rallentare l’invio dei pacchetti per problemi di congestione: Source Quench (Type 4);
* reindirizzare il traffico per fornire un instradamento efficiente in caso di router congestionato da traffico eccessivo: Routing Redirect (Type 5);
* avvertire il mittente che il tempo di vita di un suo pacchetto è scaduto (TTL = 0) e che quindi il pacchetto viene scartato: Time Exceeded (Type 11);
* valutare le prestazioni di una rete misurando il tempo di attraversamento: Timestamp Request (Type 13) del mittente, Timestamp Reply (Type 14) del destinatario;
* rilevare la lista dei nodi (router) attraversati da un pacchetto per giungere a destinazione: Traceroute (Type 30). Si realizza con il comando **tracert**.

ICMP consente ai router di scambiarsi informazioni di servizio (**messaggi router-to-router**) e di tenere sotto controllo le modalità con cui gli host generano pacchetti, inviando loro messaggi per rallentare o dirottare altrove un flusso di pacchetti (**messaggi router-to-host**).

Per quanto riguarda gli host invece, ICMP consente loro di scambiarsi informazioni di servizio (**messaggi host-to-host**) e di richiedere ai router informazioni utili sul funzionamento e la topologia della rete (**messaggi host-to-router**).

---
# ICMPv6

Una nuova versione di ICMP è stata definita per lavorare con la versione 6 di IP, descritta nella Lezione 1. Infatti, lo sviluppo di IPv6 ha reso necessaria una riorganizzazione dei tipi e dei codici esistenti in ICMP e la definizione di nuovi. Il formato del pacchetto ICMPv6 è, però, rimasto lo stesso di ICMPv4. **ICMPv6** è specificato in **RFC 4443** e successivi aggiornamenti.

La versione ICMPv6 è stata potenziata rispetto alla ICMPv4, aggiungendo nuove funzionalità e incorporando altre derivanti da protocolli IPv4 come IGMP e ARP. I numeri dei messaggi e dei tipi sono diversi da quelli ICMPv4, rendendo così incompatibili i due protocolli.

In ICMPv6 si distinguono due categorie di messaggi:

* **Error message**: riportano errori relativi all’inoltro di pacchetti IPv6, generati dal destinatario o dai nodi intermedi della rete; questi messaggi hanno il campo Type con valori da 0 a 127 (bit più significativo = 0),

* **Informational message**: forniscono informazioni di tipo diagnostico e sugli host della rete; questi messaggi hanno il campo Type con valori da 128 a 255 (bit più significativo = 1),
---

# Il protocollo ARP

## 4.1 Address Resolution Protocol (ARP)

Ad ogni host di una rete TCP/IP viene assegnato un indirizzo logico IP che lo identifica univocamente. In realtà, abbiamo parlato di “interfaccia di rete”, ossia l’indirizzo IP è assegnato alla NIC; se un host ha più schede di rete, potrà essere identificato con più indirizzi IP.

Affinché due host possano comunicare tra loro questo però non basta. Bisogna che le rispettive schede di rete siano capaci di localizzarsi reciprocamente. Occorre cioè che l’indirizzo fisico del destinatario sia noto al mittente, infatti il frame del livello Physical, per esempio il frame Ethernet, richiede sia il MAC address del mittente sia il MAC address del destinatario.

**#prendinota**
Nelle architetture di rete a strati, per realizzare l’indipendenza di un livello dall’altro, è necessario che i diversi strati implementino un proprio schema di indirizzamento. In TCP/IP troviamo il **MAC Address** nel Physical Layer, l’**IP address** nel Network Layer e l’**host name** nell’Application Layer.

In ambito IETF è stato sviluppato il protocollo **ARP** (Address Resolution Protocol), **RFC 826**, che definisce le modalità di comunicazione tra gli host di una rete locale per trovare il MAC address di una scheda di rete della quale si conosce solo l’indirizzo IP. Questa operazione è detta **risoluzione dell’indirizzo IP**. ARP è usato solamente per indirizzi IPv4, non funziona con IPv6.

**#prendinota**
In IPv6, ARP è stato sostituito dal protocollo **Neighbor Discovery** (RFC 4861), che utilizza i nuovi messaggi definiti in ICMPv6.

---

## Il formato del pacchetto ARP

Il protocollo ARP prevede solo due tipi di messaggi: **ARP Request**, per la richiesta di risoluzione di un indirizzo IP, e **ARP Reply**, per la risposta contenente l’indirizzo IP richiesto. Quindi, il formato del pacchetto ARP è molto semplice: nel caso di una rete Ethernet, MAC address di 6 byte, e protocollo IPv4, IP address di 4 byte.

Nel pacchetto ARP, l’header e il payload contengono i seguenti campi:

### 1. Header

* **Hardware Type**
  * indica il tipo di rete a livello Physical,
* **Protocol Type**
  * indica il tipo di protocollo a livello Network, 
* **Hardware Address Length**
  - è la lunghezza in ottetti dell’indirizzo fisico;
* **IP Address Length**
  -0 è la lunghezza, in ottetti, dell’IP address;
* **Operation Code**
  * specifica se il pacchetto è una ARP Request (1) o una ARP Reply (2).

### 2. Payload

Il significato dei campi varia a seconda che il messaggio sia ARP Request o ARP Reply:

* **Sender Hardware Address**

  * ARP Request: indirizzo fisico del mittente, MAC address.
  * ARP Reply: indirizzo fisico dell’host richiesto con la ARP Request.

* **Sender Protocol Address**

  * ARP Request: indirizzo logico del mittente, IP address.
  * ARP Reply: indirizzo logico del mittente.

* **Target Hardware Address**

  * ARP Request: il campo non è valorizzato in quanto è sconosciuto l’indirizzo fisico richiesto con ARP.
  * ARP Reply: indirizzo fisico del destinatario, cioè dell’host che ha inviato l’ARP Request.

* **Target Protocol Address**

  * ARP Request: indirizzo logico del destinatario.
  * ARP Reply: indirizzo logico del destinatario.

---

## La risoluzione dell’indirizzo IP

Un’implementazione TCP/IP utilizza di norma una **cache ARP**, detta anche **ARP Table**, dove ogni host mantiene e aggiorna una tabella con tutte le coppie IP-MAC a lui note.

Quando un host deve inviare dei pacchetti, controlla se nella cache ARP è presente l’indirizzo MAC corrispondente all’indirizzo IP del destinatario. Se non c’è, allora entra in gioco il protocollo ARP che stabilisce il seguente iter basato sui due tipi di messaggi, ARP Request e ARP Reply:

1. il mittente invia un pacchetto ARP contenente una ARP Request in cui specifica l’indirizzo IP del destinatario di cui vuole conoscere il corrispondente indirizzo MAC.

Questo pacchetto ARP viene mandato a tutti i nodi della rete. Inoltre il mittente aggiunge anche il proprio IP e il proprio MAC affinché il destinatario possa aggiungere la coppia di indirizzi nella propria cache ARP;

2. tutti gli host della rete ricevono l’ARP Request e leggono l’IP in esso specificato:

   * se l’indirizzo IP non corrisponde al proprio, gli host ignorano il pacchetto e la sua richiesta;</br>
   * se un host riscontra che l’IP specificato è il proprio indirizzo, allora prepara un pacchetto ARP di risposta contenente una ARP Reply in cui specifica l’indirizzo MAC corrispondente al proprio IP; inoltre aggiunge la coppia di indirizzi IP-MAC del mittente nella propria cache ARP;

3. il mittente, ricevuta la risposta, aggiorna la propria cache ARP e avvia la comunicazione.

Il procedimento ARP è differente se utilizzato su reti remote. Per dialogare con l’host remoto, l’host mittente si affida al gateway predefinito, impostato nelle proprietà del TCP/IP, al quale dirige tutto il traffico indirizzato all’host che non riesce a raggiungere. Se eventualmente poi si trattasse di un’implementazione TCP/IP che non prevede il gateway, il mittente invierebbe i pacchetti al router di rete.

In ogni caso il mittente può usare il pacchetto ARP per individuare gateway o router, qualora il loro MAC non fosse mappato sulla sua cache ARP, nello stesso modo con cui individuava il MAC dell’host destinatario in locale.


### RARP

Esiste anche il protocollo RARP (**Reverse Address Resolution Protocol**) che permette l’operazione inversa, cioè consente a un host della rete che non conosce il proprio indirizzo IP, per esempio periferiche di rete che necessitano di un indirizzo IP, di chiederlo inviando il proprio MAC.

La richiesta va inoltrata però a un server RARP, l’unico in grado di avere nella propria cache ARP il MAC richiesto. Anche il pacchetto RARP è costituito da due tipi di messaggi: RARP Request e RARP Reply.

---

## 4.2 Analisi di un pacchetto ARP con Wireshark

Utilizzando Wireshark possiamo catturare ed esaminare il contenuto di tutti i pacchetti dati in transito sulle interfacce di rete utilizzate.

Vediamo nel dettaglio un pacchetto ARP di tipo ARP Request, campo Opcode = 1, in cui l’host 192.168.1.1 chiede all’host 192.168.1.6 di inviargli il suo MAC address, campo Target MAC address = 00:00:00:00:00:00.

Vediamo poi la risposta di 192.168.1.6 mediante un pacchetto ARP Reply, Opcode = 2, in cui specifica il proprio MAC, campo Sender MAC address = cc:8e:b5:46:ea:33.

---

## 4.3 Le vulnerabilità di ARP

Il protocollo ARP è uno dei più vecchi protocolli sviluppati per la suite TCP/IP, quando ancora non si prevedeva la diffusione capillare di Internet e non sembrava necessario mettere in campo azioni preventive per la protezione delle reti.

Una delle conseguenze è che il compito svolto dal protocollo ARP è stato predisposto senza alcun meccanismo di autenticazione. Questo crea le premesse per numerose vulnerabilità.

**#techwords — Spoofing**
È la falsificazione dell’identità. Questa tecnica può essere utilizzata per falsificare diverse informazioni, come per esempio l’identità di un host all’interno di una rete o il mittente di un messaggio.

Lo **spoofing** risulta relativamente semplice per un pirata informatico: è sufficiente che invii a un host di una rete X un pacchetto ARP contenente una ARP Reply in cui affermi che il proprio indirizzo MAC è associato a un indirizzo IP della rete X stessa.

Poiché non vi è alcun modo di verificare la veridicità di un’identità, chiunque può introdursi in una rete facendo credere di esserne un legittimo utente, ottenendo così accesso alle risorse della rete, per esempio al database aziendale.

Lo scopo di questi attacchi è di ingannare lo switch, inquinando la cache ARP (**ARP cache poisoning**), al punto da indurlo a inoltrare pacchetti verso destinazioni altrimenti non raggiungibili.

La sequenza di attacco di tipo spoofing al protocollo ARP è la seguente:

1. l’host attaccante, 192.168.1.2, invia un pacchetto ARP contenente una ARP Reply in cui afferma che “il mio indirizzo MAC è 00:00:33:00:00:CC, vero, e il mio indirizzo IP è 192.168.1.254, falsa identità”;
2. l’host sotto attacco si ritrova la cache ARP inquinata e di conseguenza invierà il proprio traffico verso l’host attaccante convinto di mandarlo al proprio router;
3. l’host attaccante, dopo aver disposto a suo piacimento dei pacchetti ricevuti, li inoltrerà in modo trasparente al vero host che ha indirizzo 192.168.1.254, cioè il router.

#1 Il routing e la routing table
precedentemente abbiamo visto come il **routing** (instradamento) sia una funzione fondamentale del livello Network dell’architettura TCP/IP.
Tale funzione viene svolta dal **router** (*intermediate system*), che, per poter ottimizzare il percorso dei pacchetti da instradare, deve conoscere ed eventualmente aggiornare una serie di informazioni:

* l’indirizzo del destinatario
* i router adiacenti
* l’insieme dei possibili percorsi (**route**) verso tutte le reti remote
* il percorso migliore per ciascuna rete remota;
* il modo di mantenere e di verificare le informazioni necessarie per il routing.

Solo basandosi su queste informazioni il router può dare l’avvio al processo di **forwarding** per stabilire verso quale linea inviare il pacchetto.
## routing table
Il router deve quindi costruire, nella propria memoria, una tabella di instradamento, detta **routing table**, che gli permetta di memorizzare i dati indispensabili per individuare il percorso ottimale verso le reti remote da raggiungere.

**La routing table** è una lista di tutte le reti che il router può raggiungere insieme a informazioni sulle modalità di instradamento. Quando il router effettua il forwarding di un pacchetto, ricerca nella routing table l’indirizzo di rete corrispondente all’indirizzo IP di destinazione.

Il formato della tabella di routing varia a seconda del protocollo di routing utilizzato. In generale ogni riga (detta **entry**) della tabella presenta 4 campi:

* **network address**: contenente l’indirizzo IP di ciascuna rete raggiungibile;
* **next hop**: l’indirizzo del router successivo nel percorso verso il destinatario;
* **interface**: interfaccia del router a cui deve essere inoltrato il pacchetto per raggiungere il next hop (un router può avere più interfacce di rete);
* **metric**
  - misura utilizzata dal router per decidere quale percorso inserire nella routing table quando ci sono più alternative.
  Alla metrica si associa il concetto intuitivo di “costo”: nella routing table si inserisce il percorso a costo minore, la metrica più semplice è quella del numero di hop necessari per raggiungere la destinazione, detta **hop count**.
  Altre metriche considerano parametri quali: l’ampiezza di banda, il numero di pacchetti persi o errati, il ritardo nella trasmissione.

---

## DEFAULT ROUTER

Il router deve estrarre dall’header IP del datagram l’indirizzo del destinatario e analizzare i bit dedicati all’indirizzo di rete. Nel caso più semplice in cui l’indirizzo di rete del destinatario sia presente nel campo network address della routing table, il pacchetto viene instradato verso la sua destinazione, in modo diretto o indiretto, come visto nell’esercizio precedente.

Può però succedere che il destinatario appartenga a una rete non presente nella tabella di routing. In questo caso il router inoltrerà il pacchetto verso un **router di default** che lo prenderà in carico col compito di farlo giungere a destinazione. Se nella routing table non è definita questa **default route**, il router invia al mittente un messaggio ICMP Destination Unreachable.

---

## ROUTING STATICO E ROUTING DINAMICO

Il funzionamento di un router è caratterizzato dal modo in cui la tabella di routing viene creata:

* **manualmente**
  - se la tabella di routing è inserita dall’amministratore; in questo caso si parla di **routing statico** (utilizzabile per piccole reti);
* **automaticamente**
  - se il router costruisce da solo la tabella di routing in funzione delle informazioni ricevute attraverso i protocolli di routing; in quest’altro caso si parla di **routing dinamico**.

Il **routing statico** ha il vantaggio di non richiedere che i router si scambino informazioni per aggiornare i percorsi o eventualmente individuarne di nuovi, limitando così l’uso di banda. 
Inoltre essendo i percorsi già determinati una volta configurata la tabella, non è richiesto altro sforzo computazionale da parte del router stesso per calcolare i percorsi ottimali.

Il routing statico ha però l’inconveniente di richiedere sempre la riconfigurazione da parte dell’amministratore tutte le volte che si devono modificare le entry, sia in seguito a guasti, sia per inserire nuove route.
Tale metodo comincia a divenire abbastanza oneroso quando l’internetwork contiene parecchi percorsi. Per questi motivi il routing statico viene usato quando il numero dei segmenti di LAN non è elevato.

**Il routing dinamico** permette ai vari router di scambiarsi le informazioni necessarie a determinare i possibili percorsi per raggiungere destinazioni remote mediante dei protocolli, chiamati appunto **routing protocol**, che usano appropriati **algoritmi di routing**.

Il vantaggio di tale metodo è che richiede un minor controllo da parte dell’amministratore; per contro richiede un maggior uso di banda rispetto al routing statico perché, oltre al traffico relativo ai pacchetti, c’è un traffico relativo allo scambio delle informazioni indispensabili ai routing protocol. Inoltre, un altro notevole beneficio è dato dalla capacità di adattarsi automaticamente ai cambiamenti della topologia di rete: se si verifica un guasto lungo una connessione oppure ne viene attivata una nuova, gli aggiornamenti dei vari percorsi vengono automaticamente propagati a tutti i router.

## 1.2 Il problema della ricerca nella routing table

Un problema molto comune è il cosiddetto **Routing Table Lookup Problem (#RTLP)**, cioè il problema di dover decidere molto in fretta dove instradare i pacchetti per evitare rallentamenti e relativo congestion.

La difficoltà di RTLP risiede nel numero estremamente elevato di pacchetti che devono essere esaminati ogni secondo. Supponiamo di avere un canale da 1 Gbps, avremo che possono arrivare 1 milione di pacchetti da 1 kb in un secondo, quindi non è possibile dedicare più di 1 microsecondo a ciascun pacchetto per contenere il ritardo nell’ordine di un secondo. 
Il problema assume dimensioni ancora maggiori se si considera che un qualsiasi router ha più interfacce e che il tempo di accesso per una cache veloce è dell’ordine dei 50 nanosecondi: in queste condizioni, sono sufficienti pochi accessi alla memoria per esaurire il tempo disponibile per il routing.

Dato che l’RTLP è un problema chiave per lo sviluppo della rete Internet, esso è stato affrontato intervenendo su più fattori con lo scopo di velocizzare al massimo il processo di routing: si sono studiate strutture dati per memorizzare la tabella di routing in maniera compatta ed efficiente, si sono ideati algoritmi per velocizzare la consultazione della tabella stessa, si è proposto di utilizzare sistemi paralleli allo scopo di suddividere il carico di lavoro tra più processori e altro ancora. Tuttavia, tutto questo non è ancora sufficiente: negli ultimi anni l’overhead legato alla gestione del traffico di pacchetti (**packet processing**) è diventato così critico per le prestazioni che si è cominciato a sviluppare hardware dedicato a tale gestione. In questo contesto, i network processor sono un promettente tentativo di ottenere elevate prestazioni mantenendo almeno parte della flessibilità dei microprocessori tradizionali e con essa la capacità di adattarsi a mutamenti degli algoritmi e dei protocolli di rete.

L’RTLP si può ricondurre alla ricerca, nella tabella, del più lungo prefix corrispondente all’IP del destinatario (ricerca di prefisso di lunghezza massima).

---

# 2 GLI ALGORITMI E I PROTOCOLLI DI ROUTING

## 2.1 Lo scopo di un protocollo di routing

Lo scopo di un protocollo di routing è quello di aggiornare dinamicamente le routing table. Per fare ciò, i router devono quindi condividere le informazioni sui percorsi (route) che ciascuno conosce. Questo scambio di dati è compiuto mediante pacchetti speciali chiamati **routing update**.

Prima di entrare nel dettaglio vediamo quali sono gli scopi che un routing protocol si deve prefiggere:

* **ottimalità**
  - deve essere in grado di fornire il percorso migliore o più veloce lungo l’internetwork individuando percorsi alternativi, ciascuno con una velocità o livello di traffico diverso. Per esempio, un protocollo userà la banda e il conteggio dei salti (hop count), mentre un altro darà un peso maggiore alla banda;
* **imparzialità**
  - deve utilizzare tutte le linee disponibili per distribuire il traffico evitando le congestioni (occorre mediare tra ottimalità e imparzialità, spesso in conflitto);
* **flessibilità**
  - deve garantire capacità di adattarsi ai cambiamenti della topologia di rete;
* **convergenza veloce**
  - deve far sì che i cambiamenti all’interno dell’internetwork si propaghino verso tutti i router nel minor tempo possibile;
* **robustezza**
  - deve essere in grado di funzionare anche nel caso di configurazioni non corrette e guasti di componenti;
* **semplicità**
  - non ha bisogno di spiegazione :)

La gran parte dei protocolli che regolano il routing moderno utilizzano uno dei due seguenti algoritmi:

* Distance Vector Routing
* Link State Routing

Gli algoritmi di routing calcolano il percorso migliore o a costo minimo.

---

## 2.2 L’algoritmo di routing Distance Vector

L’algoritmo Distance Vector si basa sull’algoritmo **Bellman-Ford** per calcolare il percorso migliore. Crea una tabella di routing costituita essenzialmente da due colonne: una contenente la **distanza** (il costo) stimata per raggiungere ogni nodo della rete e una che specifica l’**interfaccia** (la linea) da utilizzare. La distanza può essere calcolata secondo metriche diverse in base al protocollo in uso. Le righe della tabella saranno invece tante quanti sono i nodi della rete.

Essendo un algoritmo dinamico, le tabelle vengono aggiornate a intervalli di tempo prestabiliti.

Inizialmente ogni router invia ai router vicini (**neighbour**) un pacchetto di **ECHO** per calcolare la distanza che lo separa da ciascuno di essi e inserisce il valore nella tabella. Subito dopo i router vicini si scambiano un **vettore delle distanze**, cioè un array contenente le informazioni che ciascun router ha a disposizione riguardo i costi per raggiungere le varie destinazioni. A quel punto, ricevuti i vettori dai vicini, ciascun router aggiorna la propria tabella mediante un confronto tra i costi risultanti dalla propria tabella e quelli ricevuti, modificando i valori laddove risultino inferiori e aggiornando le relative interfacce (linee d’uscita).

**#prendinota**
La rete può essere considerata come un grafo in cui i router rappresentano i nodi e i canali di comunicazione gli archi pesati, che hanno cioè costi (pesi) diversi. Dunque la ricerca del percorso ottimale per i pacchetti si riconduce al problema del commesso viaggiatore che deve raggiungere la sua destinazione applicando strategie che minimizzino i costi e/o i tempi.

---

### I PRINCIPALI PROBLEMI DEL DISTANCE VECTOR

I due principali problemi che possono presentarsi con il Distance Vector sono:

* **routing loop**: 
  - un pacchetto è inoltrato su un percorso circolare senza mai giungere a destinazione.
    - in questi casi il problema si risolve grazie al contatore TTL (Time To Live) il cui valore si riduce a ogni hop e quando arriva a 0 il pacchetto viene scartato;

* **count to infinity**
  - si verifica quando il costo per il raggiungimento di una destinazione viene progressivamente incrementato (normalmente avviene quando una destinazione non è più raggiungibile per via di un guasto di cui il mittente non è a conoscenza)
    - in questi casi è il percorso che finisce con l’essere scartato visti i costi sempre crescenti.

Entrambi i problemi sono legati al fatto che il Distance Vector **non conosce la topologia della rete** e che la convergenza della rete (cioè la propagazione delle informazioni) può richiedere molto tempo.

---

## LE MODIFICHE ALL’ALGORITMO DISTANCE VECTOR

Si può migliorare l’algoritmo Distance Vector, limitando così i due problemi, mediante modifiche all’algoritmo originale. Le più note varianti sono:

* **split horizon**: serve a prevenire il loop tra due nodi adiacenti. In pratica un router che riceve informazioni relative a una certa destinazione da un router adiacente non può rispedire indietro informazioni su quella stessa destinazione;

* **poison reverse**: può essere considerato come uno split horizon leggermente modificato, infatti talvolta è chiamato **split horizon with poison reverse**. Con questa tecnica il router spedisce ugualmente informazioni su certe route a chi le ha inviate, ma attribuisce loro una metrica infinita, per cui la destinazione viene considerata come irraggiungibile;

* **route poisoning**: blocca tutte le route che aumentano di costo supponendo che si tratti di un loop. Lo svantaggio è che potrebbe non essere un loop ma un legittimo aumento dovuto magari a una temporanea congestione;

* **hold down**: serve a limitare il count to infinity. Tutte le volte che un link è rimosso dalla routing table, il router non accetta aggiornamenti relativi al link stesso, se prima non ha aspettato un certo periodo di tempo (hold down timer);

* **triggered updates**: consente di inviare update non più a intervalli regolari ma non appena si verifica un cambiamento nella rete.

---

## 2.3 L’algoritmo di routing Link State

L’algoritmo Link State supera la principale limitazione del Distance Vector cioè la mancata conoscenza della topologia della rete.

**Ogni router ha una descrizione completa e diretta della topologia della rete poiché scambia le informazioni sulle distanze direttamente con tutti i router della rete e non solo coi vicini.**

Questo avviene tramite l’invio di pacchetti, detti **LSP** (**Link State Packet**), da parte di ogni router a tutti gli altri router della rete. La trasmissione avviene in **flooding**, cioè un pacchetto viene inoltrato verso tutte le linee, tranne quella da cui è arrivato. Il pacchetto LSP è solitamente inviato solo quando avviene un cambiamento nella rete (come guasti o aggiunta di nuovi nodi), anche se alcuni gestori ne prevedono l’invio periodico.

Il pacchetto LSP contiene, per ogni mittente, l’elenco e la distanza da ogni vicino. Ogni router esamina il numero di sequenza del pacchetto in arrivo e se risulta minore o uguale a quello memorizzato nel database, lo scarta. Se invece è maggiore lo memorizza e lo ritrasmette in flooding.

Tramite questi pacchetti, ogni router si costruisce un suo database con le informazioni sull’intera rete e, dopo aver ricevuto i pacchetti da tutti i router, è in grado di costruire un **grafo pesato** che rappresenta la rete stessa. A questo punto è possibile applicare un algoritmo per la ricerca dei cammini a costo minimo, il più noto è quello di **Dijkstra**.

Le **caratteristiche del Link State** si possono riassumere così:

* dispone della mappa della rete;
* ha una convergenza rapida poiché le informazioni si propagano velocemente senza alcuna elaborazione intermedia (comunicazione diretta tra tutti i nodi e non attraverso informazioni di “seconda mano”);
* difficilmente genera loop e comunque è in grado di identificarli e interromperli facilmente;
* tutti i nodi hanno basi di dati identiche;
* è facilmente scalabile all’aumentare del numero di router.

Il principale svantaggio di un algoritmo Link State è la complessità di realizzazione, anche dovuta alla notevole capacità di memoria (il database di tutta la rete) e velocità di elaborazione (ricerca dei cammini a costo minimo) richiesti.

**#prendinota**
Nel Distance Vector ogni nodo dice tutto ciò che sa ai suoi vicini, nel Link State ogni nodo dice ciò che sa dei suoi vicini a tutti.

---

## 2.4 Distance Vector e Link State a confronto

Nella **TABELLA 2** si riassumono le caratteristiche dei due algoritmi di routing.

### TABELLA 2 Confronto tra Distance Vector e Link State

| Caratteristiche                                     | Distance Vector                                                                         | Link State                                                                                                             |
| --------------------------------------------------- | --------------------------------------------------------------------------------------- | ---------------------------------------------------------------------------------------------------------------------- |
| Tipo di routing                                     | Dinamico                                                                                | Dinamico                                                                                                               |
| Convergenza                                         | Lenta                                                                                   | Veloce                                                                                                                 |
| Utilizzo della banda                                | Basso, i pacchetti sono piccoli e non usa il flooding                                   | Alto, usa il flooding e i pacchetti LSP sono di grandi dimensioni                                                      |
| Conoscenza della rete                               | Locale, conoscenza basata sulle informazioni provenienti dai router vicini              | Globale, conosce la topologia dell’intera rete                                                                         |
| Condivisione delle informazioni con i router vicini | A intervalli regolari di tempo                                                          | Solo quando c’è stato un cambiamento nella rete                                                                        |
| Algoritmo con cui è costruita la tabella di routing | Bellman-Ford                                                                            | Dijkstra SPF (Shortest Path First)                                                                                     |
| Problemi                                            | Count to infinity (si risolve con split horizon) Loop persistenti (risolvibili con TTL) | Loop, causati dall’elevato traffico in rete generato con flooding, che può causare loop infiniti (risolvibile con TTL) |
| Protocolli che lo implementano                      | RIP, IGRP                                                                               | OSPF, IS-IS                                                                                                            |

---

**Informazioni prese dal libro _Internetworking_ - Mondadori Education.**
