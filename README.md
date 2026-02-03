# 🏨 Altaris Backoffice - Gestión de Viajes

Sistema integral para la gestión de hoteles, habitaciones y reservas, con visualización de ocupación en tiempo real mediante mapas de calor.

---

## 🚀 Inicio Rápido (Un solo comando)

Para levantar toda la infraestructura (Base de Datos, Backend API y Frontend Angular), ejecuta el siguiente comando en la raíz del proyecto:

```bash
docker-compose up -d --build
```

> **⚠️ Nota importante:** La primera vez que inicies el proyecto, el sistema ejecutará un **Seeder automático** que genera **100 hoteles** y **miles de registros de ocupación**. Por favor, espera entre **60 y 90 segundos** para que la base de datos esté completamente lista.

---

## 📍 Puntos de Acceso

| Servicio | URL | Descripción |
|----------|-----|-------------|
| **Frontend** | http://localhost:4200 | Panel de administración y Mapas de Calor |
| **Backend API** | http://localhost:8080 | API REST en .NET |
| **Scalar UI** | http://localhost:8080/scalar/v1 | Documentación técnica de endpoints |

---

## 🗄️ Credenciales de Base de Datos

Si deseas conectar una herramienta externa (como **DBeaver** o **TablePlus**), utiliza estos datos:

| Campo | Valor |
|-------|-------|
| **Host** | localhost |
| **Puerto** | 5432 |
| **Usuario** | admin |
| **Password** | altaris_psw |
| **Database** | altairis_db |

---

## 🛠 Tecnologías Utilizadas

### Backend
- **Framework:** ASP.NET Core 10.0 Web API
- **Lenguaje:** C# 13
- **Base de Datos:** PostgreSQL 18.1
- **ORM:** Entity Framework Core 9.0
- **Mapping:** Mapster
- **Validación:** FluentValidation (Integridad de datos de dominio)
- **Arquitectura:** Clean Architecture + Repository Pattern
- **Patrones:** Result Pattern, Dependency Injection

### Frontend
- **Framework:** Angular 20
- **Lenguaje:** TypeScript 5.7
- **Estado:** Angular Signals para comunicación eficiente entre componentes
- **Estilos:** TailwindCSS 3 + DaisyUI 5
- **Tablas:** TanStack Table v8
- **HTTP Client:** HttpClient con RxJS
- **Locale:** Español (es-ES)

### DevOps
- **Orquestación:** Docker & Docker Compose
- **Base de Datos:** PostgreSQL 18.1 en Alpine Linux
- **Web Server:** Nginx (en producción)

---

## 🏗️ Arquitectura

```
┌─────────────────┐      ┌─────────────────┐      ┌─────────────────┐
│   Angular 20    │ ───► │  ASP.NET Core   │ ───► │  PostgreSQL     │
│   Frontend      │      │   10.0 API      │      │   18.1          │
│  (Port 4200)    │      │  (Port 8080)    │      │  (Port 5432)    │
└─────────────────┘      └─────────────────┘      └─────────────────┘
```

---

## 🎉 ¡Listo!

Ejecuta el siguiente comando y espera 60-90 segundos:

```bash
docker-compose up -d --build
```

Luego accede a **http://localhost:4200** y explora el sistema.

