# ?? Referência da API

## Endpoints

### Pratos

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/pratos` | Listar todos |
| GET | `/api/pratos/{id}` | Obter um |
| POST | `/api/pratos` | Criar |
| PUT | `/api/pratos/{id}` | Atualizar |
| DELETE | `/api/pratos/{id}` | Deletar |

### Reservas

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/reservas` | Listar todos |
| GET | `/api/reservas/{id}` | Obter um |
| POST | `/api/reservas` | Criar |
| PUT | `/api/reservas/{id}` | Atualizar |
| DELETE | `/api/reservas/{id}` | Deletar |

## Exemplos

### Criar Prato

```bash
curl -X POST https://localhost:7000/api/pratos \
  -H "Content-Type: application/json" \
  -d '{
    "nome": "Frango à Parmesana",
    "descricao": "Frango empanado",
    "categoria": "frango",
    "disponivel": true
  }'
```

### Listar Pratos

```bash
curl https://localhost:7000/api/pratos
```
