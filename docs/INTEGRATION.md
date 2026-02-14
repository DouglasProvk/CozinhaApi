# ?? Integração Frontend + Backend

## Como Funciona

O CORS está configurado em `Program.cs` para aceitar requisições do frontend:

```csharp
policy.WithOrigins("http://localhost:5173", "http://localhost:5174")
      .AllowAnyHeader()
      .AllowAnyMethod()
      .AllowCredentials();
```

## Usando a API no Frontend

### Service

```javascript
import pratosService from '@/services/pratosService'

const { data: pratos, error } = await pratosService.listar()
```

### Hook

```javascript
import { useAPI } from '@/hooks/useAPI'
import api from '@/config/api'

const { get, loading, error } = useAPI()
const { data } = await get(api.pratos.list())
```

## Variáveis de Ambiente

Arquivo: `Cozinha-FrontEnd/.env.development`

```env
VITE_API_URL=https://localhost:7000
VITE_API_BASE_URL=https://localhost:7000/api
```

## Estrutura

- **Backend:** `https://localhost:7000`
- **Frontend:** `http://localhost:5173`
- **Swagger:** `https://localhost:7000/swagger`
