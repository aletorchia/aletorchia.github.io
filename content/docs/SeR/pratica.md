---
title: "pratica routing"
type: "docs"
weight: 2
---
ip route --> statica
dinamica --> ospf
per entrare in ospf:
```cisco
conf t
router ospf 1
network 'rete da annunciare*' wildcard mask area 0
```
* le reti da annunciare sono le rete point to point dei router adiacenti
``` cisco
default information originate 
passive interface 
```
* le passive interface sono da fare solo nelle default 
