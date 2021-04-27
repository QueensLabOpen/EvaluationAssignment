
```
 __        ___  ___       __             __  
/  \ |  | |__  |__  |\ | /__` |     /\  |__) 
\__X \__/ |___ |___ | \| .__/ |___ /~~\ |__) 
```
# Evaluation Assignment - LiA Backend

## Scenario
En butik behöver hålla koll på antal besökare, både i butiken som helhet samt på varje avdelning.
Det finns sensorer i butiken som registrerar när besökare kommer in och lämnar avdelningar.
Denna data skickas till ett web API i form av ett "ENTER" eller "EXIT"-anrop tillsammans med info om avdelning och timestamp.

## Uppgift
Skapa det web API som håller koll på hur många besökare det finns i butiken.

**API:et ska kunna:**
 - Ta emot info om besökare som kommer in och som lämnar en zon.
 - Lista alla zoner och antal besökare i varje zon.
 - Nollställa en zon.
 
**Tekniker:** dotnet (.net core, .net5), python, node js

**Bonus:** Bygg en web-sida som visar upp aktuella stats besökare (valfri teknik)

**Tips:**
Keep it simple. Ha i åtanke testbarhet samt att kunna utöka protokoll för ingående trafik.
**Patterns:** Hexagonal, Ports and adapters

Lycka till!
