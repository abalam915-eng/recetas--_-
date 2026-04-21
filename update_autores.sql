-- Actualiza la estructura para incluir creadores.

DO $$
DECLARE
    r RECORD;
    nombres TEXT[] := ARRAY['María Antonieta', 'Julian Casablancas', 'Roberto Gómez', 'Sofía Vergara', 'Guillermo del Toro', 'Elena Poniatowska', 'Jorge Luis', 'Camila Sodi', 'Gael García', 'Diego Luna', 'Salma Hayek', 'Alfonso Cuarón', 'Martha Higareda', 'Eugenio Derbez', 'Lucero', 'Chayanne', 'Luis Miguel', 'Thalía', 'Paulina Rubio', 'Ricky Martin', 'Shakira', 'Carlos Santana', 'Vicente Fernández', 'Juan Gabriel', 'José José', 'Alejandro Fernández', 'Pepe Aguilar', 'Marco Antonio Solís', 'Julión Álvarez', 'Christian Nodal', 'Ángela Aguilar', 'Belinda', 'Danna Paola', 'Yuri', 'Gloria Trevi', 'Alejandra Guzmán', 'Ana Bárbara', 'Lila Downs', 'Natalia Lafourcade', 'Julieta Venegas'];
BEGIN
    -- 1. Agregar columna si no existe
    ALTER TABLE recetas ADD COLUMN IF NOT EXISTS autororiginal text;
    
    -- 2. Asegurarnos que todas las recetas actuales tengan un creador inventado asignado
    FOR r IN SELECT id FROM recetas WHERE autororiginal IS NULL OR autororiginal = '' LOOP
        UPDATE recetas SET autororiginal = nombres[ceil(random() * array_length(nombres, 1))] WHERE id = r.id;
    END LOOP;
END;
$$;
