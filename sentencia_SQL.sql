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
-- Unimos con Crédito para saber el estatus del crédito
INNER JOIN credito cr 
    ON cc.numero_de_credito = cr.numero_de_credito 
   AND cc.sucursal = cr.sucursal
-- Unimos con Tipo de Garantías para saber si es Prendaria
INNER JOIN tipo_garantias tg 
    ON cr.tipo_de_garantia = tg.tipo_de_garantia

-- Aplicamos las 3 condiciones del enunciado
WHERE cc.pagada = 'NO'
  AND cr.estatus_del_credito = 'Vigente'
  AND tg.nombre_de_la_garantia = 'Prendaria';