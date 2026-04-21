-- Arreglo de los enlaces de las imágenes que estaban caídas

UPDATE recetas SET imagen = 'https://upload.wikimedia.org/wikipedia/commons/8/83/French_Fries.jpg' WHERE titulo LIKE '%Papas Fritas%';
UPDATE recetas SET imagen = 'https://upload.wikimedia.org/wikipedia/commons/6/60/Burrito.JPG' WHERE titulo LIKE '%Burrito%';
UPDATE recetas SET imagen = 'https://upload.wikimedia.org/wikipedia/commons/3/30/Guacomole.jpg' WHERE titulo LIKE '%Guacamole%';
UPDATE recetas SET imagen = 'https://upload.wikimedia.org/wikipedia/commons/f/ff/Vanilla_ice_cream.jpg' WHERE titulo LIKE '%Helado de Vainilla%';
UPDATE recetas SET imagen = 'https://upload.wikimedia.org/wikipedia/commons/2/25/Club_sandwich.png' WHERE titulo LIKE '%Sándwich%';
UPDATE recetas SET imagen = 'https://upload.wikimedia.org/wikipedia/commons/2/2e/Chicken_Tikka_Masala_Curry.png' WHERE titulo LIKE '%Curry de Pollo%';
UPDATE recetas SET imagen = 'https://upload.wikimedia.org/wikipedia/commons/9/91/Enchiladas_Suizas.jpg' WHERE titulo LIKE '%Enchiladas suizas%';
UPDATE recetas SET imagen = 'https://upload.wikimedia.org/wikipedia/commons/3/31/Strawberry_tart_1.jpg' WHERE titulo LIKE '%Tarta de Fresa%';
UPDATE recetas SET imagen = 'https://upload.wikimedia.org/wikipedia/commons/1/1a/Pollo_a_la_brasa_%28Poulet_R%C3%B4ti%29.jpg' WHERE titulo LIKE '%Pollo a la parrilla%';
