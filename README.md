# SmallAudioManager
Permet de gérer les I/O Audio par nom en ligne de commande.

Pensez à attribuez un nom unique à vos périphériques audio !
```
usage:
SmallAudioManager.exe <Device_Name> <Action> <Action-Val>
SmallAudioManager.exe <API-Action>
<Device_Name> ::= I/O Device Name
<Action> ::= Mute, Toggle, +, -, plus, minus, +<val>, -<val> or <val>
<val> ::= -100 to 100
<Action-Val> ::= optional int (or bool for Mute)
<API-Action> ::= API-ListAll, API-ListInputs or API-ListOutputs
```
# Limitation
Le programme est lent ... Voir pour changer de lib ou créer un client/serveur efficace. (pas avec du WCF donc :d)
