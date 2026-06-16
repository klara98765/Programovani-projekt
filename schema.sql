CREATE TABLE Status (
    id SERIAL PRIMARY KEY,
    name TEXT NOT NULL
);

-- Hlavní entita
CREATE TABLE Film (
    id SERIAL PRIMARY KEY,
    title TEXT NOT NULL,
    year INT,
    statusid INT NOT NULL,
    FOREIGN KEY (statusid) REFERENCES Status(id)
);

-- Dětská entita
CREATE TABLE Review (
    id SERIAL PRIMARY KEY,
    filmid INT NOT NULL,
    rating INT CHECK (rating BETWEEN 1 AND 10),
    comment TEXT,
    created DATE DEFAULT CURRENT_DATE,
    FOREIGN KEY (filmid) REFERENCES Film(id) ON DELETE CASCADE
);