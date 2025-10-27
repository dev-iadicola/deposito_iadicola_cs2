
use world;
CREATE TABLE IF NOT EXISTS ordini (
  id INT PRIMARY KEY AUTO_INCREMENT,
  id_cliente INT NULL,
  data_ordine DATE NOT NULL,
  importo DECIMAL(7,2) NOT NULL,
  FOREIGN KEY (id_cliente) REFERENCES clienti(id) ON DELETE SET NULL
  -- in caso il cliente non sopravvive alla catastrofe, conserviamo l'ordine
);


-- popolamento 
INSERT INTO ordini (id_cliente, data_ordine, importo)
SELECT
  FLOOR(1 + RAND() * 35) AS id_cliente,
  DATE_ADD('2024-01-01', INTERVAL FLOOR(RAND() * 222) DAY) AS data_ordine,
  ROUND(10 + (RAND() * 990), 2) AS importo
FROM
  information_schema.columns AS t1,
  information_schema.columns AS t2
LIMIT 200;

-- popolamneot ordini null
INSERT INTO ordini (id_cliente, data_ordine, importo)
SELECT
  NULL AS id_cliente,
  DATE_ADD('2024-01-01', INTERVAL FLOOR(RAND() * 222) DAY) AS data_ordine,
  ROUND(10 + (RAND() * 990), 2) AS importo
FROM
  information_schema.columns AS t1,
  information_schema.columns AS t2
LIMIT 200;

-- aggiungi clienti senza ordine 
INSERT INTO clienti (city_id, nome, cognome, email, eta) VALUES
(NULL, 'Marco', 'Rossi', 'marco.rossi@example.com', 32),
(1, 'Giulia', 'Bianchi', 'giulia.bianchi@example.com', 27),
(2, 'Luca', 'Verdi', 'luca.verdi@example.com', 45),
(3, 'Sara', 'Neri', 'sara.neri@example.com', 31),
(NULL, 'Francesco', 'Gallo', 'francesco.gallo@example.com', 40),
(4, 'Elisa', 'Romano', 'elisa.romano@example.com', 22),
(5, 'Andrea', 'Greco', 'andrea.greco@example.com', 35),
(6, 'Chiara', 'Conti', 'chiara.conti@example.com', 29),
(NULL, 'Davide', 'Costa', 'davide.costa@example.com', 38),
(7, 'Martina', 'Lombardi', 'martina.lombardi@example.com', 24),
(8, 'Matteo', 'Ferrari', 'matteo.ferrari@example.com', 33),
(9, 'Alessia', 'Esposito', 'alessia.esposito@example.com', 26),
(10, 'Giorgio', 'Russo', 'giorgio.russo@example.com', 41),
(11, 'Veronica', 'Rinaldi', 'veronica.rinaldi@example.com', 28),
(12, 'Stefano', 'Gatti', 'stefano.gatti@example.com', 50),
(13, 'Serena', 'Moretti', 'serena.moretti@example.com', 23),
(NULL, 'Paolo', 'Marino', 'paolo.marino@example.com', 47),
(14, 'Ilaria', 'Fontana', 'ilaria.fontana@example.com', 36),
(15, 'Simone', 'Caruso', 'simone.caruso@example.com', 42),
(16, 'Laura', 'Colombo', 'laura.colombo@example.com', 25),
(17, 'Riccardo', 'Ferraro', 'riccardo.ferraro@example.com', 34),
(NULL, 'Silvia', 'Leone', 'silvia.leone@example.com', 30),
(18, 'Daniele', 'Palmieri', 'daniele.palmieri@example.com', 37),
(19, 'Angela', 'Barbieri', 'angela.barbieri@example.com', 46),
(20, 'Tommaso', 'Martini', 'tommaso.martini@example.com', 33),
(21, 'Marta', 'Fiore', 'marta.fiore@example.com', 21),
(NULL, 'Emanuele', 'Testa', 'emanuele.testa@example.com', 39),
(22, 'Alice', 'Rizzi', 'alice.rizzi@example.com', 27),
(23, 'Carlo', 'Monti', 'carlo.monti@example.com', 49),
(24, 'Federica', 'Serra', 'federica.serra@example.com', 35),
(25, 'Davide', 'Parisi', 'davide.parisi@example.com', 44),
(26, 'Giada', 'Sanna', 'giada.sanna@example.com', 28),
(27, 'Lorenzo', 'Villa', 'lorenzo.villa@example.com', 32),
(NULL, 'Noemi', 'Pellegrini', 'noemi.pellegrini@example.com', 29),
(28, 'Roberto', 'Basile', 'roberto.basile@example.com', 52),
(29, 'Beatrice', 'Grasso', 'beatrice.grasso@example.com', 31),
(30, 'Antonio', 'Ricci', 'antonio.ricci@example.com', 40),
(31, 'Valentina', 'Orlando', 'valentina.orlando@example.com', 26),
(32, 'Cristian', 'Vitali', 'cristian.vitali@example.com', 43),
(NULL, 'Michela', 'De Luca', 'michela.deluca@example.com', 24),
(33, 'Fabio', 'Negri', 'fabio.negri@example.com', 37),
(34, 'Eleonora', 'Mariani', 'eleonora.mariani@example.com', 30),
(35, 'Nicola', 'Piras', 'nicola.piras@example.com', 48),
(36, 'Sofia', 'Costantini', 'sofia.costantini@example.com', 22),
(37, 'Gabriele', 'Longo', 'gabriele.longo@example.com', 39),
(38, 'Aurora', 'Benedetti', 'aurora.benedetti@example.com', 33),
(39, 'Pietro', 'Rossetti', 'pietro.rossetti@example.com', 31),
(NULL, 'Claudia', 'Marchetti', 'claudia.marchetti@example.com', 45),
(40, 'Giovanni', 'Gentile', 'giovanni.gentile@example.com', 50);

SELECT 
o.data_ordine, c.nome
FROM ordini o
left JOIN clienti c ON o.id_cliente = c.id
;

SELECT 
o.data_ordine, c.nome
FROM ordini o
left JOIN clienti c ON o.id_cliente = c.id
;
-- 100 senza clienti
SELECT COUNT(*) FROM ordini 
WHERE id_cliente IS NULL;

-- tutti ordini: anche senza ordini 
SELECT 
o.data_ordine, c.nome
FROM clienti c
right JOIN ordini o ON o.id_cliente = c.id
;

-- caso 3
SELECT 
o.data_ordine, c.nome
FROM ordini o
left JOIN clienti c ON o.id_cliente = c.id
ORDER BY o.id_cliente;

-- ordini con clienti e importo superiore a 200, join nazione e città
SELECT 
o.data_ordine, c.nome, o.importo, 
cy.Name citta, naz.Name nazione
FROM ordini o
RIGHT JOIN clienti c ON o.id_cliente = c.id
INNER JOIN city cy ON c.city_id = cy.ID
INNER JOIN country naz ON cy.CountryCode = naz.Code
WHERE o.importo > 200
ORDER BY o.importo
;


-- clienti senza ordine
SELECT 
  c.id, c.nome, c.cognome, o.id_cliente
FROM clienti c
LEFT JOIN ordini o ON c.id = o.id_cliente
WHERE o.id_cliente IS NULL;

-- ordini orfani (senza clienti) usa right join
SELECT id_cliente, ordini.id, 
data_ordine, 
importo 
FROM clienti AS c
right JOIN ordini ON c.id = ordini.id_cliente
WHERE id_cliente IS null