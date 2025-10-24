
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
WHERE continet <> "Europe";
-- 
-- es 2 prendi le città più popolose delgi USA
SELECT  name, Population
FROM
 city 
 WHERE CountryCode = "USA"
ORDER BY
Population DESC

