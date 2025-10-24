CREATE DATABASE IF NOT EXISTS negozio_test; 
USE negozio_test; 



CREATE TABLE IF NOT EXISTS vendite (
    id INT AUTO_INCREMENT PRIMARY KEY,
    prodotto VARCHAR(100),
    categoria VARCHAR(100),
    quantita INT,
    prezzo_unitario DECIMAL(6,2),
    data_vendita DATE
);

INSERT INTO vendite (prodotto, categoria, quantita, prezzo_unitario, data_vendita) VALUES
('Mouse Logitech M185', 'Periferiche', 5, 14.99, '2025-01-03'),
('Tastiera Meccanica Keychron K2', 'Periferiche', 2, 79.90, '2025-01-05'),
('Monitor LG 27UL500', 'Monitor', 1, 249.00, '2025-01-06'),
('SSD Samsung 1TB', 'Archiviazione', 3, 99.00, '2025-01-08'),
('RAM Corsair Vengeance 16GB', 'Componenti', 4, 75.50, '2025-01-10'),
('CPU Ryzen 7 5800X', 'Componenti', 1, 319.99, '2025-01-11'),
('Scheda Madre ASUS B550', 'Componenti', 1, 149.99, '2025-01-12'),
('Scheda Video RTX 4060', 'Componenti', 2, 379.00, '2025-01-13'),
('Alimentatore Corsair 650W', 'Componenti', 2, 89.90, '2025-01-14'),
('Case NZXT H510', 'Componenti', 1, 99.00, '2025-01-15'),
('Cuffie HyperX Cloud II', 'Audio', 3, 89.90, '2025-01-17'),
('Microfono Blue Yeti', 'Audio', 1, 129.99, '2025-01-18'),
('Webcam Logitech C920', 'Video', 2, 89.99, '2025-01-19'),
('Stampante HP LaserJet', 'Periferiche', 1, 159.00, '2025-01-20'),
('Router TP-Link AX1500', 'Rete', 2, 69.90, '2025-01-21'),
('Hard Disk Seagate 2TB', 'Archiviazione', 3, 69.90, '2025-01-22'),
('Mouse Pad XXL', 'Accessori', 4, 19.99, '2025-01-23'),
('Chiavetta USB 64GB', 'Archiviazione', 6, 12.50, '2025-01-24'),
('Ventola ARGB Cooler Master', 'Componenti', 5, 15.99, '2025-01-25'),
('Notebook Lenovo IdeaPad', 'Portatili', 1, 699.00, '2025-01-26');


-- totale vendita x categoria 
SELECT categoria,  sum(quantita) "Vendita per catrogira" 
FROM vendite
GROUP BY categoria;

-- prezzo medio per cateogira 
SELECT categoria, AVG(prezzo_unitario) "Prezzo medio" 
FROM vendite 
GROUP BY categoria;


-- qta totale venduta per ogni prodotto
SELECT prodotto, SUM(quantita) AS quantita_vendute
FROM vendite
GROUP BY prodotto;

-- prezzo minimo e prezzo massimo
SELECT MIN(prezzo_unitario) "Minimo", MAX(prezzo_unitario) "Massimo"
FROM vendite;

-- visualizza tutte le vendite
SELECT COUNT(*) "Vendite registrate" FROM vendite WHERE quantita <> 0; -- tecnicamente non è mai null, ma è tanto per far capire che la data vendita conferma la vendita

-- prodotti non venduto (cotnrollo inverso)
SELECT COUNT(*) "Vendite registrate" FROM vendite WHERE quantita = 0;
-- visualizza i 5 prodotti più costosi 
SELECT DISTINCT(prodotto), prezzo_unitario FROM vendite  -- DISTINCT perché ho messo più volte i value 
ORDER BY prezzo_unitario DESC 
LIMIT 5;

SELECT DISTINCT(prodotto), quantita FROM vendite 
ORDER BY quantita ASC 
LIMIT 3;
