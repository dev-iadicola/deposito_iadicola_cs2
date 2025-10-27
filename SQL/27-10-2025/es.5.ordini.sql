
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



