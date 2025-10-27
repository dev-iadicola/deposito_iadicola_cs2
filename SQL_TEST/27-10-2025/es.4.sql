USE world; -- uso word
# creo tab
CREATE TABLE if NOT EXISTS clienti (
id INT PRIMARY KEY AUTO_INCREMENT,
city_id INT NULL,
nome VARCHAR(100), 
cognome VARCHAR(100),
email VARCHAR(100),
eta INT,
  FOREIGN KEY (city_id) REFERENCES city(id) ON DELETE SET NULL -- se elimini una città perché esplode o subisce un'epidemia o un attacco zombie, ma il tuo cliente è sopravvisuto, puoi cambiare la sua città, ecco perché nullable
);

# popolo tab
INSERT INTO clienti (city_id, nome, cognome, email, eta) VALUES
(1, 'Luca', 'Bianchi', 'luca.bianchi@gmail.com', 29),
(2, 'Giulia', 'Rossi', 'giulia.rossi@outlook.com', 33),
(3, 'Matteo', 'Verdi', 'matteo.verdi@yahoo.it', 41),
(1, 'Francesca', 'Esposito', 'francesca.esposito@gmail.com', 27),
(4, 'Davide', 'Russo', 'davide.russo@hotmail.com', 36),
(2, 'Elena', 'Ferrari', 'elena.ferrari@live.it', 25),
(5, 'Andrea', 'Gallo', 'andrea.gallo@icloud.com', 31),
(3, 'Simone', 'Romano', 'simone.romano@libero.it', 39),
(4, 'Chiara', 'Colombo', 'chiara.colombo@gmail.com', 28),
(5, 'Marco', 'Ricci', 'marco.ricci@outlook.com', 34),
(2, 'Serena', 'Mariani', 'serena.mariani@yahoo.com', 23),
(1, 'Fabio', 'Greco', 'fabio.greco@gmail.com', 44),
(3, 'Silvia', 'Rinaldi', 'silvia.rinaldi@outlook.com', 30),
(4, 'Stefano', 'Moretti', 'stefano.moretti@hotmail.it', 38),
(5, 'Ilaria', 'De Luca', 'ilaria.deluca@gmail.com', 26)
(2, 'Alessandro', 'Conti', 'alessandro.conti@gmail.com', 32),
(4, 'Valentina', 'Fontana', 'valentina.fontana@outlook.com', 29),
(5, 'Giorgio', 'Caruso', 'giorgio.caruso@yahoo.it', 42),
(1, 'Martina', 'Giordano', 'martina.giordano@gmail.com', 26),
(3, 'Riccardo', 'Mancini', 'riccardo.mancini@icloud.com', 37),
(5, 'Federica', 'De Santis', 'federica.desantis@gmail.com', 30),
(2, 'Michele', 'Rizzi', 'michele.rizzi@hotmail.com', 45),
(4, 'Paola', 'Barbieri', 'paola.barbieri@libero.it', 33),
(1, 'Gabriele', 'Marchetti', 'gabriele.marchetti@gmail.com', 24),
(3, 'Roberta', 'Villa', 'roberta.villa@outlook.com', 28),
(5, 'Daniele', 'Testa', 'daniele.testa@gmail.com', 39),
(2, 'Laura', 'Gentile', 'laura.gentile@yahoo.it', 35),
(4, 'Nicola', 'Serra', 'nicola.serra@outlook.com', 31),
(1, 'Barbara', 'Palmieri', 'barbara.palmieri@gmail.com', 40),
(3, 'Tommaso', 'Leone', 'tommaso.leone@hotmail.com', 27),
(5, 'Beatrice', 'Grassi', 'beatrice.grassi@icloud.com', 25),
(2, 'Emanuele', 'Basile', 'emanuele.basile@gmail.com', 34),
(4, 'Sara', 'Parisi', 'sara.parisi@outlook.it', 22),
(3, 'Carlo', 'Longo', 'carlo.longo@gmail.com', 43),
(1, 'Alice', 'Ferraro', 'alice.ferraro@yahoo.it', 29);

-- clienti solo con dominio Gmail 
SELECT cl.*, c.Name AS "città"   
FROM clienti AS cl, city AS c
 WHERE email LIKE "%@gmail.%" AND
  cl.city_id = c.ID ;

 -- email che finsicono con gmail.com
  SELECT cl.*, c.Name AS "città"   
FROM clienti AS cl, city AS c
 WHERE email LIKE "%@gmail.com" AND
  cl.city_id = c.ID;
  
-- cleinti con nome che iniziano con la lettera A
SELECT cl.nome , c.name as "città" 
FROM clienti AS cl, city AS c
 WHERE
 cl.city_id = c.ID
 AND cl.nome LIKE "A%";
 
 -- clienti con nome che hanno la lettera A
 SELECT cl.nome , c.name as "città" 
FROM clienti AS cl, city AS c
 WHERE
 cl.city_id = c.ID
 AND cl.nome LIKE "%A%";
 
 -- clienti con cognome che contiene esattamente 5 lettere
 SELECT 
  cl.nome, cl.cognome,
  c.name AS citta, 
  LENGTH(cl.cognome) AS lng
FROM clienti AS cl
JOIN city AS c ON cl.city_id = c.id
WHERE LENGTH(cl.cognome) = 5;

SELECT nome, cognome, eta
FROM clienti
WHERE eta BETWEEN 30 AND 40
ORDER BY eta ;

-- clienti di roma 
 SELECT 
  cl.nome, cl.cognome,
  c.name AS citta, 
  LENGTH(cl.cognome) AS lng
FROM clienti AS cl
JOIN city AS c ON cl.city_id = c.id
WHERE c.Name = "roma";
