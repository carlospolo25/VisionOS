### 📦 Sistema de Gestión de Inventario Empresarial

Aplicación web desarrollada bajo una arquitectura desacoplada basada en 
ASP.NET Core MVC + API REST, diseñada para la administración segura,
escalable y eficiente de inventarios.

- Este sistema simula un entorno empresarial real donde:

✔ El frontend y backend están completamente separados
✔ La seguridad se gestiona mediante JWT
✔ El acceso está controlado por roles
✔ La lógica de negocio vive en la API
✔ La aplicación puede escalar y ser consumida por otros clientes

## 🏗️ Arquitectura del Sistema

El proyecto fue construido siguiendo el principio de separación de responsabilidades,
dividiéndose en dos capas independientes:

# 🔹 Inventario.API

Backend REST encargado de:

Lógica de negocio
Acceso a datos
Seguridad con JWT
Refresh automático de sesión
Generación de reportes PDF
Exposición de endpoints reutilizables
Esta API puede ser consumida por:

✔ Aplicaciones web
✔ Aplicaciones móviles
✔ Sistemas externos

# 🔹 Inventario.Web

Aplicación MVC responsable de:

Interfaz de usuario
Consumo de la API
Manejo de sesión
Control de acceso por roles
Experiencia de usuario
La aplicación no accede directamente a la base de datos, todo pasa por la API.

👉 Esto simula arquitectura real empresarial.

# 🔐 Seguridad Implementada

El sistema incorpora múltiples capas de seguridad:

Autenticación con JWT
Refresh automático de token
Expiración por inactividad
Protección de rutas
Manejo de sesión
Control de acceso por roles (Admin)

# Además:

✔ Las rutas sensibles están protegidas
✔ Las acciones críticas requieren rol Admin
✔ El frontend respeta la autorización del backend

# 📊 Dashboard Inteligente

El sistema incluye un dashboard que proporciona visión estratégica del inventario:

Total de productos
Productos con stock
Stock crítico
Productos sin stock
Valor total del inventario
Porcentaje de riesgo
Productos que requieren atención

# ⚙️ Módulo de Gestión

Permite administración completa del inventario:

Crear productos
Editar productos
Eliminar productos
Activar / Desactivar productos
Buscador dinámico

# 📄 Sistema de Reportes

Incluye generación de reportes empresariales:

Resumen completo del inventario
Métricas operativas
Evaluación de riesgo
Generación de PDF
Descarga directa

# 🧠 Reglas de Negocio

El sistema incorpora lógica real de inventario:

Productos inactivos no aportan al valor total
Stock crítico definido como ≤ 5 unidades
Riesgo calculado dinámicamente

Inventario evaluado automáticamente como:

Saludable
Atención requerida
Crítico

# 🛠️ Tecnologías Utilizadas

Backend
ASP.NET Core Web API
Entity Framework Core
SQL Server
JWT Authentication
QuestPDF
Frontend
ASP.NET Core MVC
Bootstrap
HttpClient
Filtros personalizados
Handler JWT

# 🔑 Control de Acceso por Rol

Solo usuarios Admin pueden:

Crear productos
Editar productos
Eliminar productos
Cambiar estado
Generar reportes PDF

# 📦 Estructura del Proyecto

Inventario/
│
├── Inventario.API/
├── Inventario.Web/
├── Inventario.sln
└── README.md


## ▶️ Ejecución del Proyecto

# Clonar repositorio
git clone <url-del-repo>

# Configurar conexión

Editar:

appsettings.json

# Ejecutar migraciones
Update-Database

# Ejecutar solución

Abrir:

Inventario.sln

# 🎯 Enfoque del Proyecto

Este sistema fue construido con el objetivo de demostrar:

✔ Arquitectura desacoplada
✔ Seguridad moderna con JWT
✔ Separación frontend / backend
✔ Reglas de negocio reales
✔ Escalabilidad
✔ Consumo de API

Simula el funcionamiento de un sistema empresarial real.