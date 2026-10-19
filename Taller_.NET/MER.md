```mermaid
erDiagram
Rol {
bigint id PK
string nombre
}

    Usuario {
        bigint id PK
        string nombre
        string apellido
        string telefono
        string correo
    }

    ParqueSolar {
        bigint id PK
        string nombre
        string ubicacion
        date fechaCreacion
        bigint cantMaxNodos
    }

    Nodo {
        bigint id PK
        string nombre
        string ubicacionParque
        date fechaCreacion
        decimal energiaGenerada
    }

    Inversor {
        bigint id PK
        string nombre
        date fechaCreacion
    }

    Panel {
        bigint id PK
        string nombre
        date fechaCreacion
        enum estadoPanel
        bool encendido
        decimal potencia
        string vidaUtil
    }

    Historico {
        bigint id PK
        date fecha
        decimal energiaTotal
    }

    %% Relaciones
    Rol }|--|{ Usuario : "Tiene (N:N)"
    Usuario }|--|{ ParqueSolar : "Gestiona (N:N)"
    ParqueSolar ||--|{ Nodo : "Tiene (1:1..N)"
    Nodo ||--|| Inversor : "Contiene (1:1)"
    Nodo ||--|{ Panel : "Dispone (1:N)"
    Usuario }|--|{ Panel : "Mantiene (N:N - attrs: Fecha, Observaciones)"
    Panel ||--|{ Historico : "Genera (1:N)"
```