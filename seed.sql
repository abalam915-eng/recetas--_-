-- Archivo semilla con 30 recetas que incluyen imágenes

-- 0. Agregamos la nueva columna de imagen en la tabla si no existe
ALTER TABLE recetas ADD COLUMN IF NOT EXISTS imagen text;

-- 1. Insertamos un usuario administrador/creador base (ignora el error si ya existe)
INSERT INTO usuarios (id, githubid, nombre, email) 
VALUES ('00000000-0000-0000-0000-000000000001', 'chef_supremo', 'Chef Supremo', 'chef@recetas.com')
ON CONFLICT (githubid) DO NOTHING;

-- Si githubid causó conflicto y no sabemos el id real, podemos usarlo con un subquery o insertar condicional,
-- pero para hacerlo infalible usaremos un UUID estático y un nombre distinto si hace falta.

-- 2. Insertamos las 30 recetas
INSERT INTO recetas (usuarioid, titulo, ingredientes, pasos, imagen, estatus, fechacreacion, fechapublicacion) VALUES
('00000000-0000-0000-0000-000000000001', 'Tacos al Pastor', 'Carne de cerdo, piña, tortillas, cilantro, cebolla', '1. Marinar carne. 2. Asar. 3. Servir en tortillas.', 'https://images.unsplash.com/photo-1551504734-5ee1c4a1479b?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Pizza Margarita', 'Masa, salsa de tomate, mozzarella, albahaca', '1. Estirar masa. 2. Poner salsa y queso. 3. Hornear.', 'https://images.unsplash.com/photo-1574071318508-1cdbab80d002?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Hamburguesa Clásica', 'Pan, carne, queso, lechuga, tomate', '1. Cocinar carne. 2. Armar.', 'https://images.unsplash.com/photo-1568901346375-23c9450c58cd?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Sushi Roll', 'Arroz, nori, salmón, aguacate', '1. Hacer arroz. 2. Enrollar y cortar.', 'https://images.unsplash.com/photo-1579871494447-9811cf80d66c?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Ensalada César', 'Lechuga, pollo, crutones, parmesano', '1. Cortar. 2. Mezclar.', 'https://images.unsplash.com/photo-1550304943-4f24f54ddde9?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Pasta Carbonara', 'Pasta, huevo, guanciale, pecorino', '1. Hervir pasta. 2. Mezclar salsa. 3. Unir.', 'https://images.unsplash.com/photo-1612874742237-6526221588e3?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Ramen', 'Fideos, caldo estofado, huevo, cerdo', '1. Calentar caldo. 2. Servir.', 'https://images.unsplash.com/photo-1569718212165-3a8278d5f624?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Hotcakes', 'Harina, leche, huevo, mantequilla, miel', '1. Mezclar batido. 2. Cocinar en sartén.', 'https://images.unsplash.com/photo-1528207776546-365bb710ee93?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Papas Fritas', 'Papas, aceite, sal', '1. Cortar. 2. Freír.', 'https://images.unsplash.com/photo-1630483864448-6a56f6424e4d?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Burrito', 'Tortilla grande, frijoles, arroz, carne', '1. Envolver ingredientes.', 'https://images.unsplash.com/photo-1626804561085-f0ea89b4f056?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Ceviche', 'Pescado, limón, cebolla morada, cilantro', '1. Curtir pescado en limón. 2. Añadir verduras.', 'https://images.unsplash.com/photo-1534482421-64566f976cfa?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Pastel de Chocolate', 'Cacao, harina, huevos, azúcar', '1. Batir. 2. Hornear.', 'https://images.unsplash.com/photo-1578985545062-69928b1d9587?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Guacamole', 'Aguacate, tomate, cebolla, limón, chile', '1. Machacar aguacate. 2. Mezclar.', 'https://images.unsplash.com/photo-1582236873539-7871b0fd5e73?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Helado de Vainilla', 'Crema, leche, vainilla', '1. Congelar y remover.', 'https://images.unsplash.com/photo-1570197781417-0a52377209ce?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Waffles', 'Harina, huevo, leche', '1. Cocinar en wafflera.', 'https://images.unsplash.com/photo-1504544750208-dc0358e63f7f?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Sándwich', 'Pan dulce, jamón, mayonesa', '1. Untar. 2. Cerrar.', 'https://images.unsplash.com/photo-1619881589316-db32279c6da9?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Curry de Pollo', 'Pollo, leche de coco, especias curry', '1. Sofreír especias. 2. Cocer el pollo lentamente en la salsa.', 'https://images.unsplash.com/photo-1565557618462-8411d3319808?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Quesadillas', 'Tortilla de harina, queso asadero fundido', '1. Calentar y doblar en comal.', 'https://images.unsplash.com/photo-1615870216519-2f9fa575fa5c?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Lasaña', 'Carne molida, salsa boloñesa, hojas de pasta, queso', '1. Formar niveles y hornear.', 'https://images.unsplash.com/photo-1574894709920-11b28e7367e3?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Steak', 'Corte ribeye, sal, pimienta', '1. Asar a fuego alto, término medio.', 'https://images.unsplash.com/photo-1600891964092-4316c288032e?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Enchiladas suizas', 'Pollo, tortillas, salsa verde, crema', '1. Rellenar y bañar.', 'https://images.unsplash.com/photo-1584050212351-ad77ec09eabc?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Tarta de Fresa', 'Masa horneada, fresas, crema pastelera', '1. Montar.', 'https://images.unsplash.com/photo-1542484462-80517ba3f5e5?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Empanadas', 'Masa preparada, picadillo', '1. Rellenar y freir.', 'https://images.unsplash.com/photo-1604908176997-125f25cc6f3d?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Pollo a la parrilla', 'Muslos de pollo, adobo, especias', '1. Marinar, asar al carbón.', 'https://images.unsplash.com/photo-1598511726623-d055581b0a68?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Espagueti Rojo', 'Pasta natural, puré de tomate, queso parmesano', '1. Hervir y salsear.', 'https://images.unsplash.com/photo-1551183053-bf91a1d81141?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Smoothie Bowl', 'Frutos del bosque congelados, leche de almendras, plátano', '1. Licuar ingredientes espesos y decorar.', 'https://images.unsplash.com/photo-1490474418585-ba9bad8fd0ea?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Sopa de Tomate', 'Tomates maduros asados, ajo, crema', '1. Licuar y hervir.', 'https://images.unsplash.com/photo-1547592180-85f173990554?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Brownies', 'Chocolate oscuro fundido, nueces, cacao en polvo', '1. Mezclar y hornear 30 mins.', 'https://images.unsplash.com/photo-1606313564200-e75d5e30476c?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Paella Mixta', 'Arroz bomba, pollo, calamares, camarón, azafrán, mejillones', '1. Sofreír condimentos, agregar arroz y caldo, no mover y dejar secar.', 'https://images.unsplash.com/photo-1534080564583-6be75777b70a?w=800&q=80', 'Publicada', now(), now()),
('00000000-0000-0000-0000-000000000001', 'Risotto de Champiñones', 'Arroz arborio, caldo de pollo, champiñones, vino blanco', '1. Sellar champiñones, mover arroz e incorporar caldo de poco a poco.', 'https://images.unsplash.com/photo-1473093295043-cdd812d0e601?w=800&q=80', 'Publicada', now(), now());
