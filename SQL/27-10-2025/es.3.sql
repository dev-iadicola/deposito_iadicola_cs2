# Esercizi svolti il 12/10/2025 
USE world; 

-- cerca tutti i paesi che iniziano con "ita"

SELECT * FROM country WHERE name  LIKE "ita%" LIMIT 10;

-- seleziona solo due 4 id di city

SELECT * FROM city WHERE ID IN (2,10,200, 5);

SELECT * FROM country WHERE `name` LIKE 'united state%';

-- query per togliere città

SELECT * FROM country WHERE `name`  NOT IN ('france','united state');

-- popolazione da 100m a 500m (milioni)
SELECT `name`, Population FROM country WHERE Population BETWEEN 100000 AND 500000 
ORDER BY population DESC;
