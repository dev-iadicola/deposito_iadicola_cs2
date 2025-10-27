USE world;
# CREAZIONE TABELLE
CREATE TABLE if NOT EXISTS Libri (
 id INT AUTO_INCREMENT PRIMARY KEY,
 titolo VARCHAR(100) NOT NULL,
 autore VARCHAR(100) NOT NULL,
 genere VARCHAR(50),
 anno_pubblicazione INT,
 prezzo DECIMAL(6,2)
);


CREATE TABLE IF NOT EXISTS Vendite (
 id INT AUTO_INCREMENT PRIMARY KEY,
 id_libro INT NOT NULL,
 data_vendita DATE NOT NULL,
 quantita INT NOT NULL,
 negozio VARCHAR(100), FOREIGN KEY (id_libro) REFERENCES Libri(id) ON
DELETE CASCADE ON
UPDATE CASCADE
);



-- ========================================================
-- 2 INSERIMENTO 100 LIBRI (semi-random, realistici)
-- ========================================================
INSERT INTO Libri (titolo, autore, genere, anno_pubblicazione, prezzo)
VALUES
('Il nome della rosa', 'Umberto Eco', 'Storico', 1980, 14.90),
('1984', 'George Orwell', 'Distopico', 1949, 12.50),
('Cronache marziane', 'Ray Bradbury', 'Fantascienza', 1950, 11.00),
('Harry Potter e la pietra filosofale', 'J.K. Rowling', 'Fantasy', 1997, 16.99),
('Il signore degli anelli', 'J.R.R. Tolkien', 'Fantasy', 1954, 24.50),
('Orgoglio e pregiudizio', 'Jane Austen', 'Romanzo', 1813, 10.00),
('Il piccolo principe', 'Antoine de Saint-Exupéry', 'Favola', 1943, 8.90),
('It', 'Stephen King', 'Horror', 1986, 18.75),
('Shining', 'Stephen King', 'Horror', 1977, 17.80),
('Carrie', 'Stephen King', 'Horror', 1974, 15.20),
('Misery', 'Stephen King', 'Thriller', 1987, 16.10),
('La coscienza di Zeno', 'Italo Svevo', 'Romanzo', 1923, 13.40),
('Il Gattopardo', 'Giuseppe Tomasi di Lampedusa', 'Storico', 1958, 15.20),
('La divina commedia', 'Dante Alighieri', 'Classico', 1321, 19.99),
('Inferno', 'Dan Brown', 'Thriller', 2013, 14.60),
('Angeli e demoni', 'Dan Brown', 'Thriller', 2000, 13.99),
('Il codice Da Vinci', 'Dan Brown', 'Thriller', 2003, 15.99),
('La ragazza del treno', 'Paula Hawkins', 'Thriller', 2015, 12.99),
('L’amica geniale', 'Elena Ferrante', 'Romanzo', 2011, 11.80),
('I Malavoglia', 'Giovanni Verga', 'Classico', 1881, 9.90);

-- duplico con variazioni per arrivare a ~100 libri
INSERT INTO Libri (titolo, autore, genere, anno_pubblicazione, prezzo)
SELECT 
    CONCAT(titolo, ' (Edizione ', FLOOR(RAND() * 5 + 1), ')'),
    autore,
    genere,
    anno_pubblicazione + FLOOR(RAND() * 5),
    ROUND(prezzo + (RAND() * 5 - 2.5), 2)
FROM Libri
LIMIT 80;

-- ========================================================
-- 3 worldPOPOLAMENTO DI 600 RIGHE IN VENDITE (casuali)
-- ========================================================
DELIMITER $$

CREATE PROCEDURE PopolaVendite()
BEGIN
    DECLARE i INT DEFAULT 0;
    DECLARE maxLibri INT DEFAULT 0;
    SELECT COUNT(*) INTO maxLibri FROM Libri;

    WHILE i < 600 DO
        INSERT INTO Vendite (id_libro, data_vendita, quantita, negozio)
        VALUES (
            FLOOR(RAND() * maxLibri) + 1,
            DATE_ADD('2024-01-01', INTERVAL FLOOR(RAND() * 365) DAY),
            FLOOR(RAND() * 10) + 1,
            ELT(
                FLOOR(RAND() * 8) + 1,
                'Amazon Online',
                'Feltrinelli Roma',
                'Feltrinelli Milano',
                'IBS.it',
                'Mondadori Store Torino',
                'Libreria Coop Bologna',
                'Libreria Hoepli',
                'Libraccio Firenze'
            )
        );
        SET i = i + 1;
    END WHILE;
END$$

DELIMITER ;

-- esegui la procedura
CALL PopolaVendite();

-- pulizia (facoltativa)
DROP PROCEDURE PopolaVendite;

-- ========================================================
-- 4️ ERIFICA
-- ========================================================
SELECT COUNT(*) AS TotaleLibri FROM Libri;
SELECT COUNT(*) AS TotaleVendite FROM Vendite;

SELECT 
    l.titolo,
    COUNT(v.id) AS num_vendite,
    SUM(v.quantita) AS copie_totali,
    ROUND(SUM(v.quantita * l.prezzo), 2) AS incasso_totale
FROM Libri l
LEFT JOIN Vendite v ON l.id = v.id_libro
GROUP BY l.id
ORDER BY incasso_totale DESC
LIMIT 10;
