# Prueba Técnica: Analista de Aplicaciones de Software

Repositorio que contiene la resolución de la prueba técnica práctica para el cargo de **Analista de Aplicaciones de Software**.

---

## 🛠️ Tecnologías y Requisitos Previos

Asegúrate de contar con las siguientes herramientas instaladas antes de ejecutar la solución:

* **Backend:** .NET 9 SDK / Visual Studio 2022
* **Frontend:** Node.js (v18+) & Angular CLI (v21)

---

## 📋 Contenido de la Solución

### 1. Base de Datos y Consulta SQL (Ejercicio 1)

El script SQL que da respuesta a las 3 tablas solicitadas y la consulta de distribución de capital vencido por bandas morosas se encuentra a continuación:

#### 🔍 Consulta SQL para Bandas de Vencimiento
```sql
SELECT 
    -- Banda 1: De 1 a 30 días de mora
    ISNULL(SUM(CASE 
        WHEN DATEDIFF(day, cc.fecha_de_vencimiento_de_la_cuota, GETDATE()) BETWEEN 1 AND 30 
        THEN cc.capital ELSE 0 
    END), 0) AS [1_a_30_dias],

    -- Banda 2: De 31 a 90 días de mora
    ISNULL(SUM(CASE 
        WHEN DATEDIFF(day, cc.fecha_de_vencimiento_de_la_cuota, GETDATE()) BETWEEN 31 AND 90 
        THEN cc.capital ELSE 0 
    END), 0) AS [31_a_90_dias],

    -- Banda 3: De 91 a 180 días de mora
    ISNULL(SUM(CASE 
        WHEN DATEDIFF(day, cc.fecha_de_vencimiento_de_la_cuota, GETDATE()) BETWEEN 91 AND 180 
        THEN cc.capital ELSE 0 
    END), 0) AS [91_a_180_dias],

    -- Banda 4: De 181 a 360 días de mora
    ISNULL(SUM(CASE 
        WHEN DATEDIFF(day, cc.fecha_de_vencimiento_de_la_cuota, GETDATE()) BETWEEN 181 AND 360 
        THEN cc.capital ELSE 0 
    END), 0) AS [181_a_360_dias],

    -- Banda 5: Más de 360 días de mora
    ISNULL(SUM(CASE 
        WHEN DATEDIFF(day, cc.fecha_de_vencimiento_de_la_cuota, GETDATE()) > 360 
        THEN cc.capital ELSE 0 
    END), 0) AS [mayor_360_dias]

FROM cuota_credito cc
INNER JOIN credito cr 
    ON cc.numero_de_credito = cr.numero_de_credito 
   AND cc.sucursal = cr.sucursal
INNER JOIN tipo_garantias tg 
    ON cr.tipo_de_garantia = tg.tipo_de_garantia
WHERE cc.pagada = 'NO'
  AND cr.estatus_del_credito = 'Vigente'
  AND tg.nombre_de_la_garantia = 'Prendaria';

## 🚀 Guía de Ejecución (Ejercicios 3 al 7)

Para interactuar con la aplicación y probar la funcionalidad completa descrita en los ejercicios **3 al 7** (Grid View de alumnos, listas de selección, ordenamiento y formulario de registro), es necesario tener corriendo **ambos servicios** (Backend y Frontend) de manera simultánea.

---
Paso 1: Iniciar el Backend (.NET 9)

El backend expone la API REST que gestiona los datos de los alumnos.

Paso 2 Iniciar el Frontend - Angular 21

Este proyecto es la interfaz web desarrollada en **Angular 21** con **Tailwind CSS** para la gestión interactiva de alumnos, dando respuesta a los requerimientos prácticos (puntos 2 al 7) del test de desarrollo.

---

## 🛠️ Requisitos Previos

Antes de ejecutar este proyecto, asegúrate de tener instalado:

* **Node.js**: Versión `18.x` o superior (Recomendado v20+).
* **npm**: Versión `11` o superior.

---

## ⚙️ Instalación y Configuración Local
npm install
ng serve