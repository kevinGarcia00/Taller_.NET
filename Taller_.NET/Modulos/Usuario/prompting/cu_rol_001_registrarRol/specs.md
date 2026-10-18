# Especificaciones Técnicas: registrar rol

## Detalles de Implementación por Capa
### 1. Capa Domain (Domain)
- **Entidades:** 
    - **Rol**
        - **Porpiedades:**
            - `Id` (Guid, PK)
            - `Nombre` (string, max 25 caracteres)
            - `Gestionar_Usuarios` (boolean)
            - `Gestion_Parques` (boolean)
            - `Mantenimiento` (boolean)
