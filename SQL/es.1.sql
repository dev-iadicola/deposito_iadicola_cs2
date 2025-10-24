
-- es 3 grupp by con funz aggregazione 
-- pr ogn continet su contry numero totale di paesi apparententi di ciascun continete 
-- la popolazione totale del continente
SELECT continent, Population
FROM country
GROUP BY continent;
-- esercizio 1. trova tutti i continenti tranne quelli europeo
SELECT
 DISTINCT(Region) region 
FROM
 country 
WHERE Continent <> "Europe";
-- 
-- es 2 prendi le città più popolose delgi USA
SELECT  name, Population
FROM
 city 
 WHERE CountryCode = "USA"
ORDER BY
Population DESC

SELECT c.Name AS "Nomi Città" , c.Population "Popolazione Cittò", co.Name "Contry" , co.Population "Contry pop." 
	FROM 
		city AS c, country AS co 
	LIMIT 10;
	
SELECT MAX(population)"Popolazione tutte le città" FROM city;
-- max pop x cities
SELECT c1.NAME , (SELECT max(population) FROM city WHERE NAME = c1.NAME)
FROM city AS c1;


