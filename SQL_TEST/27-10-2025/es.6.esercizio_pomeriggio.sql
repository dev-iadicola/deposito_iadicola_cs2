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


# Libri non venduti 

INSERT INTO Libri (titolo, autore, genere, anno_pubblicazione, prezzo)
VALUES
('Echoes of Eternity', 'Mara Silvestri', 'Fantascienza', 2025, 18.50),
('Whispers in the Wind', 'Laura Bianchi', 'Romanzo', 2024, 15.20),
('Shadows of the Forgotten', 'Giorgio Rossi', 'Thriller', 2023, 14.90),
('The Silent Symphony', 'Elena Verdi', 'Narrativa', 2025, 16.75),
('Rise of the Phoenix', 'Davide Moretti', 'Fantasy', 2022, 17.30),
('Lost Horizons', 'Francesca Greco', 'Avventura', 2024, 13.40),
('The Last Ember', 'Roberto Marino', 'Post-apocalittico', 2023, 19.10),
('Beneath the Crimson Sky', 'Valentina Conti', 'Storico', 2021, 14.80),
('When the Stars Align', 'Andrea Bruni', 'Fantascienza', 2025, 16.20),
('The Forgotten Garden', 'Simone De Luca', 'Romanzo', 2022, 12.90),
('Paths of Destiny', 'Chiara Ferrari', 'Fantasy', 2023, 15.50),
('The Hidden Key', 'Luca Esposito', 'Thriller', 2024, 13.70),
('Waves of Time', 'Martina Ricci', 'Fantascienza', 2025, 17.00),
('The Shadow Keeper', 'Alessandro Romano', 'Horror', 2023, 14.60),
('Mysteries of the Deep', 'Federica Longo', 'Avventura', 2022, 13.20),
('The Golden Promise', 'Giulia Fontana', 'Storico', 2024, 16.40),
('Flight of the Sparrow', 'Michele Rinaldi', 'Romanzo', 2023, 11.90),
('Dance of the Moon', 'Sara Gallo', 'Narrativa', 2025, 15.80),
('The Iron Fortress', 'Vittorio Lombardi', 'Fantasy', 2022, 18.40),
('Winds of Change', 'Lucia Battaglia', 'Romanzo', 2024, 14.10),
('Echoes in the Dark', 'Giovanna Testa', 'Thriller', 2023, 13.90),
('Quest for the Lost City', 'Carlo Vitale', 'Avventura', 2025, 17.60),
('Veil of Secrets', 'Elisa Ricciardelli', 'Horror', 2024, 14.20),
('The Midnight Hour', 'Francesco Greco', 'Fantasy', 2023, 16.90),
('Tides of Fate', 'Roberta Messina', 'Storico', 2022, 12.70),
('The Forgotten Realm', 'Paolo Gallo', 'Fantascienza', 2024, 15.35),
('Echoes of the Past', 'Anna Serra', 'Narrativa', 2025, 14.95),
('The Crystal Garden', 'Stefano Russo', 'Fantasy', 2023, 18.00),
('Nightfall Rising', 'Chiara Marino', 'Horror', 2024, 15.25),
('The Silver Thread', 'Marco D’Amico', 'Storico', 2022, 13.50),
('Journey to the Unknown', 'Valeria Greco', 'Avventura', 2025, 16.15),
('Whispers of the Forest', 'Gabriele Morelli', 'Fantascienza', 2024, 14.30),
('The Last Horizon', 'Eleonora Romano', 'Romanzo', 2023, 12.80),
('Secrets of the Valley', 'Riccardo De Santis', 'Narrativa', 2025, 13.95),
('The Burning Sky', 'Ilaria Fontana', 'Fantasy', 2024, 17.45),
('Echoes of Silence', 'Leonardo Ricci', 'Thriller', 2023, 14.05),
('The Forgotten Shores', 'Martina Colombo', 'Avventura', 2022, 11.60),
('Winds of the Wild', 'Federico Barbieri', 'Storico', 2025, 16.85),
('The Phantom Library', 'Silvia Moretti', 'Horror', 2024, 13.75),
('Journey of the Stars', 'Daniele Costa', 'Fantascienza', 2023, 15.60),
('The Hidden Sanctuary', 'Giada Neri', 'Romanzo', 2025, 14.45),
('Reflections of Time', 'Claudio Ferri', 'Narrativa', 2024, 13.30),
('Rise of the Shadows', 'Paola Marchetti', 'Fantasy', 2023, 18.20),
('The Silent Voyage', 'Giorgio Benedetti', 'Avventura', 2022, 12.50),
('Veil of the Moon', 'Elena Barbieri', 'Horror', 2025, 15.10),
('The Crystal Path', 'Luca Marino', 'Romanzo', 2024, 14.75),
('Echoes from Beyond', 'Alicia Rossi', 'Fantascienza', 2023, 17.25),
('The Last Sanctuary', 'Federica Gentile', 'Storico', 2025, 13.55),
('Whispers at Dawn', 'Matteo Lombardo', 'Narrativa', 2024, 12.40),
('Tides of the Past', 'Anna Russo', 'Avventura', 2023, 11.95),
('Shadows of Time', 'Davide Serra', 'Fantasy', 2025, 16.65),
('The Hidden Realm', 'Sofia Esposito', 'Thriller', 2024, 14.85),
('Echoes of Destiny', 'Giorgio Fabbri', 'Romanzo', 2023, 13.10);


# QUERY 
-- auotori libri venduti con "king"
USE world;
SELECT l.titolo, l.autore, v.negozio, v.data_vendita 
FROM vendite v
JOIN libri l ON l.id = v.id_libro
WHERE l.autore LIKE "%king%";

-- libri non venduto nell'anno 2000 fino al 2002
SELECT *
FROM Libri l
LEFT JOIN Vendite v ON l.id = v.id_libro
WHERE v.id_libro IS NULL
  AND l.anno_pubblicazione IS NOT NULL
  AND l.anno_pubblicazione BETWEEN 2000 AND 2022
  ORDER BY l.anno_pubblicazione desc
  ;
  
-- punti vendita con libri venduto
SELECT v.negozio, l.titolo, l.autore
	FROM libri l
	INNER JOIN vendite v ON v.id_libro = l.id
	
WHERE v.negozio IN('Amazon Online', 
					'Feltrinelli Roma',
					'Libraccio Firenze');
 
 
 -- mostrare titoli, autore prezzo e data vendita dei libri 
 -- con genere fantasi, horror, giallo
 -- pubblicati dopo il 2015
 -- venduto in engozi cui nome coniente Feltrinelli
 SELECT l.titolo, l.autore, l.prezzo, v.data_vendita, v.negozio FROM libri l
 inner JOIN vendite v ON l.id = v.id_libro
 WHERE l.anno_pubblicazione > 2015
 AND v.negozio LIKE "%Feltrinelli%"

 ORDER BY v.data_vendita DESC 
	
 ;