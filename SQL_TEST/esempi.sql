
-- ordina le cittò secondo il numero di popolazione 
SELECT 
	name, COUNT( CountryCode) AS qty, CountryCode
FROM 
	city 
GROUP BY c.CountryCode 
-- ORDER BY name;
-- ritorna le cittò che non sono in italia
SELECT id, NAME, CountryCode, District, info FROM city c
WHERE c.Name <> "italy"
ORDER BY NAME DESC ;

-- conta quante città contiene un continente
SELECT 
  c.code, 
  c.name, 
  (SELECT COUNT(*) 
   FROM city AS ci 
   WHERE ci.CountryCode = c.code) AS cities
FROM country AS c;

-- ritorna la qtù numerica di città evitando di contare duplicati 
SELECT COUNT(DISTINCT NAME),(SELECT COUNT(DISTINCT NAME) FROM country WHERE capital > 100) AS capital100 FROM country;

-- ritorna le città associate al codice per country -- 
SELECT 
	c.name , ci.Name
FROM 
	city AS ci, country AS  c
WHERE c.Code = ci.CountryCode;
