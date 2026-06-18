# Filmotéka

Desktopová aplikace v Avalonia MVVM pro správu filmové databáze.

## Téma

- Hlavní entita: Film
- Dětská entita 1:N: Recenze
- Číselník: Status filmu

## Funkce

- Seznam filmů: načtení, přidání, úprava, smazání a přechod na detail.
- Detail filmu: zobrazení vybraného filmu a jeho recenzí.
- Recenze: plný CRUD přímo v detailu filmu.
- Formulář filmu: validace povinných polí a výběr statusu přes ComboBox z číselníku.
- PostgreSQL běží v Dockeru a data jsou uložená ve volume.

## Struktura projektu

```text
Filmoteka/
├── Models/
├── Repositories/
├── ViewModels/
├── Views/
├── Services.cs
├── docker-compose.yaml
├── schema.sql
├── seed.sql
├── .env.example
├── .gitignore
└── README.md
```

## Spuštění databáze

1. Zkopíruj `.env.example` jako `.env`.
2. Spusť PostgreSQL:

```powershell
docker compose up -d
```

Při prvním startu se automaticky vytvoří tabulky a vloží se ukázková data.

## Spuštění aplikace

```powershell
dotnet restore
dotnet run
```

## Databázové tabulky

- `watch_statuses`: číselník stavů filmu.
- `movies`: hlavní entita film.
- `reviews`: recenze navázané na film přes `movie_id`.

## Použité technologie

- Avalonia
- MVVM
- PostgreSQL
- Docker Compose
- Repository pattern
- Dependency Injection přes `Microsoft.Extensions.DependencyInjection`
- `.env` konfigurace připojení k databázi
