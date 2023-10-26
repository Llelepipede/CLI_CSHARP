# CLI_CSHARP

Pour crée l'alias sous windows:

1. créer un .bat du nom de votre choix dans votre ordinateur.
2. dans ce .bat, y inclure la commande DOSKEY en concéquence a votre commande (exemple : C:>DOSKEY m = mkdir)
3. ouvrir l'editeur de registre avec un cmd avec la commande "regedit"
4. aller dans HKEY_LOCAL_MACHINE/SOFTWARE/Microsoft/Command Processor
5. créer une nouvelle valeur de chaine appellé AutoRun, et y mettre en donnée l'emplacement de notre fameux .bat

 
 
maintenant , hop , ce .bat sera executé a chaque ouverture de terminal, ce qui executera la commande DOSKEY, et initialisera la nouvelle commande ainsi crée :)


