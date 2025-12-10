## BookItsUp

Коротко: сервис бронирования для организаций.  
Есть провайдеры, услуги, клиенты, недельные расписания, исключения и бронирования с проверкой пересечений.

### Стек
- .NET 9, ASP.NET Core
- EF Core 9 + Npgsql (PostgreSQL),
- Swashbuckle (Swagger UI)

### Слои
- **Domain** — модели и интерфейсы.
- **DataAccess** — Entities, конфигурации EF, репозитории.
- **Application** — сервисы.
- **API** — контроллеры, Swagger.
