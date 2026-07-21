# 🚗 Rentaly — Enterprise Car Rental Management System

Rentaly is a full-stack car rental platform built with **ASP.NET Core MVC**, designed around a clean **N-Tier Architecture** with a public-facing booking experience and a fully-featured **Admin Fleet Management Dashboard**. The project was built as a portfolio-grade demonstration of production patterns: layered architecture, EF Core relational modeling, server-rendered admin tooling, and real business logic (pricing, availability, fleet analytics) — not just CRUD scaffolding.

---

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Tech Stack](#tech-stack)
- [Domain Model](#domain-model)
- [Public Site Features](#public-site-features)
- [Admin Dashboard Features](#admin-dashboard-features)
- [Reporting & Exports](#reporting--exports)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Engineering Notes & Design Decisions](#engineering-notes--design-decisions)
- [Known Limitations / Roadmap](#known-limitations--roadmap)

---

## Overview

Rentaly simulates a real-world car rental business with two distinct experiences:

1. **Public Site** — customers browse the fleet, view detailed car pages with image galleries, and complete a multi-step booking flow with live price calculation.
2. **Admin Panel** — fleet managers manage cars, brands, models, categories, branches, and bookings; monitor KPIs on a live dashboard; and generate exportable operational reports.

The project intentionally avoids "God controller" anti-patterns — business logic lives in a dedicated Business Layer, data access is abstracted behind repositories, and each admin module (Car, Brand, Model, Category, Branch, Booking) follows a consistent Controller → Service → DAL → EF Core chain.

---

## Architecture

Rentaly follows a **4-layer N-Tier Architecture**:

```
Rentaly.WebUI            → Presentation layer (MVC Controllers, Razor Views, Areas)
Rentaly.BusinessLayer     → Business rules, validation (FluentValidation), orchestration
Rentaly.DataAccessLayer   → Generic Repository Pattern + EF Core implementations
Rentaly.EntityLayer       → POCO entities (no framework dependencies)
```

**Key architectural principles applied:**

- **Generic Repository Pattern** (`IGenericDal<T>` / `GenericRepository<T>`) for baseline CRUD, extended per-entity with specific `Include()`-aware query methods (e.g. `GetCarWithDetailsByIdAsync`, `GetDetailsByIdAsync` for bookings) — avoiding both N+1 query issues and over-fetching on list views.
- **Manager/Service Layer** (`CarManager`, `BookingManager`, etc.) sits between controllers and the DAL, so controllers never talk to EF Core directly.
- **Area-based separation**: the Admin panel lives entirely under `Areas/Admin`, cleanly isolated from the public site's routes, views, and layout.
- **ViewModels over raw entities** wherever a view needs data shaped differently than the domain model (e.g. `CarModelRowViewModel`, `BookingListViewModel`, `ReportsViewModel`) — entities are never bound directly to complex forms, keeping model binding predictable.

---

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | ASP.NET Core MVC (.NET 10) |
| ORM | Entity Framework Core (Code-First + Migrations) |
| Database | Microsoft SQL Server |
| Validation | FluentValidation |
| Mapping | AutoMapper |
| Frontend (Admin) | Tailwind CSS, Google Material Symbols |
| Frontend (Public) | Custom CSS on top of the Rentaly HTML template |
| Excel Export | ClosedXML |
| PDF Export | QuestPDF |
| Icons | Material Symbols Outlined |

---

## Domain Model

Core entities and how they relate:

```
Brand ───┬──< CarModel
         └──< Car >──┬── Category
                     ├── VehicleType
                     ├── Branch
                     ├── CarImage (1-to-many gallery)
                     └── Status (enum: Available / Rented / Maintenance)

Branch ──< Car
Branch ──< Booking (as PickUpBranch)
Branch ──< Booking (as DropOffBranch)   ← dual FK to the same table

Booking >── Car
Booking >── Branch (PickUp)
Booking >── Branch (DropOff)

Activity   → system-wide audit trail (car added, customer created, rental completed, etc.)
```

**Notable modeling decisions:**

- **`Booking` has two foreign keys into `Branch`** (`PickUpBranchId` / `DropOffBranchId`) to support one-way rentals. This required explicitly configuring `DeleteBehavior.Restrict` on both relationships in `OnModelCreating` — SQL Server rejects `CASCADE` when a table has multiple paths to the same parent table (a pattern that recurred with `CarModel → Brand` once `Car` already had a direct FK to `Brand`).
- **`CarStatus` is a proper enum** (`Available`, `Rented`, `Maintenance`), not a pair of overloaded booleans — this eliminates the "is it unavailable because it's rented or because it's in maintenance?" ambiguity that a naive `IsAvailable: bool` design would create.
- **`Activity` is type-driven, not free-text**: an `ActivityType` enum drives icon and color selection via a display-helper pattern, rather than storing icon names/colors per row.

---

## Public Site Features

- **Car Details Page**
  - Image gallery with clickable thumbnails and a main preview, backed by a dedicated `CarImage` table (falls back gracefully to the car's single cover `ImageUrl` if no gallery images exist).
  - "About the vehicle" section, technical specification table, and a booking CTA — all data-driven, no hardcoded copy.
- **Booking Flow**
  - Custom car-select dropdown (image + name + live price, not a native `<select>`) synced with a live total-price calculator (`days × dailyPrice`) that recalculates client-side as pick-up/return dates change.
  - **Server-side price recalculation on submit** — the client-side total is never trusted; the controller independently recomputes `days * car.DailyPrice` before persisting, closing the obvious "tamper the JS" attack vector.
  - Booking confirmation page with a generated reservation code, full itinerary summary, and branch/date/price breakdown.

---

## Admin Dashboard Features

All admin modules follow the same shape: filterable/paginated list view, Create/Edit forms bound to the real entity, soft server-side validation, and delete via a confirmation-guarded POST form (never a bare GET link, to avoid accidental/CSRF-triggerable deletions).

- **Dashboard**
  - Live KPI cards (total cars, total bookings, total branches) computed from real data, not placeholders.
  - **Fleet Growth chart** — a hand-rolled SVG line/area chart (no charting library dependency) driven by `Car.CreatedDate`, computing cumulative fleet size per month over a selectable range (6/12 months) via a JSON endpoint.
  - **Recent Activities** widget as an isolated `ViewComponent`, backed by its own `Activity` table and a relative-time helper (`ToTurkishTimeAgo()`) for "2 dakika önce" / "3 saat önce" style timestamps.
- **Vehicle Management** (`Car`)
  - Server-side filtering (brand, model, category, free-text search across brand/model/plate) combined with pagination — filters and page state persist together via query string, so paging never silently drops an active filter.
  - Create/Edit forms support **both file upload and pasted image URL** for the cover photo, with live client-side preview either way, and a placeholder fallback so a missing image can never violate the `NOT NULL` constraint on `ImageUrl`.
  - Status badges (Available/Rented/Maintenance) rendered via a shared `CarStatusDisplayHelper` so color/label logic exists in exactly one place.
- **Brand / Model / Category / Branch Management**
  - Each supports full CRUD, with list views showing **computed** metrics (car count per brand, fleet share %, base price per category, occupancy per branch) derived live from relationships rather than duplicated/staled fields.
  - Manager-name avatars are generated from initials rather than depending on external headshot URLs.
- **Booking Management**
  - Status (Confirmed / Ongoing / Completed / Cancelled) is **derived from dates at read-time**, not manually maintained — only `Cancelled` is an explicit, persisted state. This means a booking automatically "becomes" Completed the moment its drop-off date passes, with zero admin intervention or background job required.
  - Row-level action menu: **View Details** (full booking breakdown — customer, vehicle, dates, branches, payment) and **Cancel** (guarded, hidden once a booking is already completed/cancelled).
  - Dashboard insights: cancellation rate, upcoming vs. active rentals, top drop-off branches, and live fleet utilization %.
- **Reports**
  - Filterable by branch and car status, paginated on-screen.
  - Per-car computed metrics: occupancy % (rented-days ÷ days-since-added-to-fleet) and lifetime earnings (sum of non-cancelled bookings for that car).
  - Branch-level distribution (available/rented/maintenance split) rendered as both a stacked bar and a donut chart.

---

## Reporting & Exports

- **Excel export** via **ClosedXML** — generates a fully-formatted `.xlsx` with bold headers and auto-sized columns, respecting whatever branch/status filter is currently active on screen.
- **PDF export** via **QuestPDF** — a fluent, code-first PDF layout (landscape A4, header/footer, styled table) with zero native-binary dependencies.
  > The project initially used **DinkToPdf**, which wraps the native `libwkhtmltox` library. This introduced a `DllNotFoundException` in the target environment (a common pain point with DinkToPdf across OS/architecture combinations). The project migrated to **QuestPDF**, which is pure .NET and eliminates the native dependency entirely — a deliberate architectural correction made mid-project once the tooling risk became apparent.

---

## Project Structure

```
Rentaly.EntityLayer/
  Entities/                 → Car, Brand, CarModel, Category, Branch, Booking, CarImage, Activity...

Rentaly.DataAccessLayer/
  Abstract/                 → IGenericDal<T>, ICarDal, IBookingDal, ...
  RepositoryDesignPattern/  → GenericRepository<T>
  EntityFramework/          → EfCarDal, EfBookingDal, ..., RentalyContext

Rentaly.BusinessLayer/
  Abstract/                 → ICarService, IBookingService, ...
  Concreate/                → CarManager, BookingManager, ...
  ValidationRules/           → FluentValidation validators

Rentaly.WebUI/
  Controllers/               → Public-facing controllers (Car, Booking)
  Views/                     → Public Razor views
  Areas/Admin/
    Controllers/             → CarController, BrandController, CarModelController,
                                CategoryController, BranchController, BookingController,
                                DashboardController, ReportsController
    Models/                  → Admin ViewModels (list/form/report models)
    Views/                   → Admin Razor views + Shared/_AdminLayout.cshtml
  Helpers/                   → CarStatusDisplayHelper, ActivityDisplayHelper, TimeAgoHelper
```

---

## Getting Started

### Prerequisites

- .NET 10 SDK
- SQL Server (LocalDB or full instance)
- Visual Studio 2022+ (or any IDE with EF Core tooling)

### Setup

```bash
# 1. Clone the repository
git clone <repo-url>
cd Rentaly

# 2. Update the connection string in appsettings.json
#    "ConnectionStrings:RentalyContext"

# 3. Apply migrations
Add-Migration InitialCreate   # if starting fresh
Update-Database

# 4. Restore packages & run
dotnet restore
dotnet run --project Rentaly.WebUI
```

### First-run checklist

- Seed at least one `Brand`, `CarModel`, `Category`, `VehicleType`, and `Branch` before creating cars — the Create/Edit Car forms populate their dropdowns from these tables.
- The Admin panel is available at `/Admin/Dashboard`.

---

## Engineering Notes & Design Decisions

A few decisions worth calling out for anyone reviewing the codebase:

- **Cascade-delete conflicts were a recurring, deliberately-handled pattern.** Any time a child table has more than one path back to the same parent (`Booking → Branch` via two FKs, `CarModel → Brand` alongside `Car → Brand`), SQL Server refuses `ON DELETE CASCADE` due to "multiple cascade paths." Rentaly resolves each of these explicitly with `DeleteBehavior.Restrict` in `OnModelCreating`, rather than suppressing the error at the database level.
- **`ModelState` and navigation properties.** Entities with collection navigation properties (`Car.Images`, `Brand.Cars`, `Category.Cars`) triggered spurious `"The X field is required"` validation failures on POST, since those collections are never populated by the form. Every Create/Edit POST action explicitly calls `ModelState.Remove(...)` for these properties rather than loosening validation globally.
- **List queries vs. detail queries are intentionally separate.** Generic `TGetListAsync()` methods only `Include()` what a list view needs; a heavier `GetDetailsByIdAsync()`/`GetCarWithDetailsByIdAsync()` exists per-entity for pages that need the full navigation graph. This avoids paying the JOIN cost of a detail page on every list render.
- **No client-side trust for pricing.** All monetary calculations (booking totals, report earnings) are computed server-side from persisted data, even where a client-side preview exists for UX purposes.

---

## Known Limitations / Roadmap

- Booking creation from the Admin panel (as opposed to cancellation/viewing) is not yet implemented.
- Car model → Brand cascading dropdown filtering (client-side, so selecting a Brand narrows the Model list) is a natural next step but not yet wired up.
- No authentication/authorization layer is yet enforced on the Admin Area — intended for a future pass (ASP.NET Core Identity + role-based policies).
- Reports currently compute occupancy against "days since added to fleet"; a rolling 30/90-day window would be a more actionable metric for a real fleet manager.

---

*Built as a hands-on exploration of N-Tier architecture, EF Core relational modeling, and admin-tooling UX in ASP.NET Core MVC.*
