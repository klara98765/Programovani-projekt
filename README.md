# Filmotéka

Desktopová aplikace v Avalonia (MVVM) pro správu filmové databáze s PostgreSQL v Dockeru.

---

## 🎯 Téma

- **Hlavní entita:** Film (1)
- **Dětská entita (1:N):** Recenze
- **Číselník:** Status filmu

---

## 🧩 Funkcionalita

### 📽️ Film (CRUD)
- vytvoření filmu
- zobrazení seznamu filmů
- úprava filmu
- smazání filmu
- detail filmu

### ⭐ Recenze (CRUD – 1:N)
- přidání recenze k filmu
- úprava recenze
- smazání recenze
- zobrazení recenzí v detailu filmu

### 📊 Status (číselník)
- plánuji
- koukám
- dokoukáno
- zrušeno
- výběr přes ComboBox ve formuláři filmu

---

## 🏗️ Architektura

Aplikace je postavena na MVVM:

- **Models** → datové třídy (Film, Review, Status)
- **Views** → Avalonia UI (XAML)
- **ViewModels** → aplikační logika + Commands
- **Repositories** → komunikace s PostgreSQL (SQL mimo ViewModely)
- **Services** → DI registrace

---

## 🗄️ Databáze

### Tabulky

#### `movies`
- id (PK)
- title
- director
- release_year
- status_id (FK → watch_statuses)

#### `reviews`
- id (PK)
- movie_id (FK → movies)
- author
- rating (1–10)
- text
- reviewed_at

#### `watch_statuses` (číselník)
- id (PK)
- name

---

## 🐳 Spuštění databáze (Docker)

### 1. Vytvoření `.env`

```bash
cp .env.example .env

Nastavení:

POSTGRES_DB=filmoteka
POSTGRES_USER=postgres
POSTGRES_PASSWORD=postgres
POSTGRES_HOST=localhost
POSTGRES_PORT=5432
2. Spuštění DB
docker compose up -d
3. Inicializace databáze

Při prvním spuštění se použije:

schema.sql → vytvoření tabulek
seed.sql → naplnění číselníku statusů
🚀 Spuštění aplikace
dotnet restore
dotnet run
📁 Struktura projektu
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
⚙️ Použité technologie
Avalonia UI
MVVM pattern
PostgreSQL
Docker Compose
Repository pattern
Dependency Injection (Microsoft.Extensions.DependencyInjection)
Npgsql
.env konfigurace
⚠️ Důležité požadavky
.env není součástí repozitáře
SQL nesmí být ve ViewModelech (pouze v Repositories)
MVVM musí být dodrženo (žádná logika ve Views)
aplikace musí fungovat po docker compose up bez úprav
👤 Autor

Školní projekt – 2. ročník
