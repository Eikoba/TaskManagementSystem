# TaskManagementSystem

## 📌 Описание
Task Management System — веб-API на ASP.NET Core, реализующее CRUD операции над задачами с поддержкой:
- смены статуса (`/api/tasks/{id}/change-status`),
- валидации моделей,
- централизованной обработки ошибок,
- фильтрации и пагинации,
- EF Core + миграции,
- запуска в Docker Compose.

### Архитектура
- **Clean Architecture**: разделение на слои `Domain`, `Application`, `Infrastructure`, `Api`.
- **Domain**: сущности, enum’ы.
- **Application**: интерфейсы, сервисы, DTO, валидация.
- **Infrastructure**: EF Core DbContext, миграции, репозитории.
- **Api**: Web API контроллеры, middleware, DI, Swagger.
- **IntegrationTests**: тесты на основе `WebApplicationFactory`.
- **Tests**: тесты на основе `xUnit`.

---

## 🚀 Запуск проекта
1. Целевая платформа Linux

2. Установите [Docker](https://docs.docker.com/get-docker/) и [Docker Compose](https://docs.docker.com/compose/).

3. В корне репозитория выполните:
   ```bash
   docker compose up --build -d
   ```
   
4. Доступ к приложению

После успешного запуска контейнеров сервисы будут доступны по адресам:

- Swagger UI: [http://localhost/swagger](http://localhost/swagger)  
- API root: [http://localhost/api/tasks](http://localhost/api/tasks)  


