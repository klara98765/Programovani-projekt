INSERT INTO watch_statuses (name)
VALUES
    ('plánuji'),
    ('koukám'),
    ('dokoukáno'),
    ('zrušeno')
ON CONFLICT (name) DO NOTHING;

INSERT INTO movies (title, director, release_year, status_id)
SELECT 'Blade Runner', 'Ridley Scott', 1982, id
FROM watch_statuses
WHERE name = 'dokoukáno'
ON CONFLICT DO NOTHING;

INSERT INTO movies (title, director, release_year, status_id)
SELECT 'Duna: Část druhá', 'Denis Villeneuve', 2024, id
FROM watch_statuses
WHERE name = 'plánuji'
ON CONFLICT DO NOTHING;

INSERT INTO reviews (movie_id, author, rating, text, reviewed_at)
SELECT m.id, 'Ukázkový uživatel', 9, 'Výborná atmosféra a vizuální styl.', DATE '2026-01-15'
FROM movies m
WHERE m.title = 'Blade Runner'
ON CONFLICT DO NOTHING;
